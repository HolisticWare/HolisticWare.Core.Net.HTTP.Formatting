﻿using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Net.HTTP.Formatting
{
    public class MediaTypeFormatterApacheAvro : MediaTypeFormatter
    {
        private const string MimeType = "application/avro";

        public MediaTypeFormatterApacheAvro()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(MediaTypeFormatterApacheAvro.MimeType));
            MediaTypeMappings.Add
                (
                    new RequestHeaderMapping
                            (
                                "Accept",
                                MediaTypeFormatterApacheAvro.MimeType, 
                                StringComparison.OrdinalIgnoreCase,
                                false,
                                MediaTypeFormatterApacheAvro.MimeType
                            )
                );

            return;
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            return ReadFromStreamAsync(type, readStream, content, formatterLogger, CancellationToken.None);
        }

        public override async Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var avroSerializer = new AvroMediaTypeFormatter.AvroSerializerEx(type);
            using (var memoryStream = new MemoryStream())
            {
                await readStream.CopyToAsync(memoryStream).ConfigureAwait(false);
                return avroSerializer.Deserialize(memoryStream.ToArray());
            }
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content,
            TransportContext transportContext)
        {
            return WriteToStreamAsync(type, value, writeStream, content, transportContext, CancellationToken.None);
        }

        public override async Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content,
            TransportContext transportContext, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var avroSerializer = new AvroMediaTypeFormatter.AvroSerializerEx(type);
            var serializedBytes = avroSerializer.Serialize(value);
            await
                writeStream.WriteAsync(serializedBytes, 0, serializedBytes.Length, cancellationToken)
                    .ConfigureAwait(false);
        }

        public override bool CanReadType(Type type)
        {
            return type.GetCustomAttribute<DataContractAttribute>(true) != null;
        }

        public override bool CanWriteType(Type type)
        {
            return type.GetCustomAttribute<DataContractAttribute>(true) != null;
        }
    }
}