using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Core.Net.HTTP.Formatting
{
    public class MediaTypeFormatterTextPlain : MediaTypeFormatter 
    {
        private const string MimeType = "text/plain";

        public MediaTypeFormatterTextPlain() 
        {
            SupportedMediaTypes.Add
                                (
                                   new MediaTypeHeaderValue(MediaTypeFormatterTextPlain.MimeType)
                                );
            MediaTypeMappings.Add
                                (
                                    new RequestHeaderMapping
                                            (
                                                "Accept",
                                                MediaTypeFormatterTextPlain.MimeType,
                                                StringComparison.OrdinalIgnoreCase,
                                                false,
                                                MediaTypeFormatterTextPlain.MimeType
                                            )
                                );

            return;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override Task<object> ReadFromStreamAsync
                                            (
                                                Type type, 
                                                Stream stream, 
                                                System.Net.Http.HttpContent content, 
                                                IFormatterLogger formatterLogger
                                            )
        {
            return
                new Task<object>
                (
                    () => 
                    {
                        return new StreamReader(stream).ReadToEnd();
                    }
                );
        }

        public override Task WriteToStreamAsync
                                            (
                                                Type type, object value, 
                                                Stream stream,
                                                System.Net.Http.HttpContent content,
                                                System.Net.TransportContext transportContext
                                            )
        {
            return
                new Task
                (
                    () => 
                    {
                        StreamWriter writer = new StreamWriter(stream);
                        writer.Write(value.ToString());
                    }
                );
        }
    }
}
