using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

// https://weblog.west-wind.com/posts/2012/Mar/09/Using-an-alternate-JSON-Serializer-in-ASPNET-Web-API
// https://gist.github.com/Stephanvs/1993158
// https://code.msdn.microsoft.com/Using-JSONNET-with-ASPNET-b2423706/view/Discussions
// http://www.c-sharpcorner.com/UploadFile/2b481f/using-json-net-with-Asp-Net-api3/

namespace Core.Net.HTTP.Formatting
{

    public class MediaTypeFormatterJSONnetNewtonsoft : MediaTypeFormatter 
    {
        private const string MimeType = "text/plain";

        private JsonSerializerSettings _jsonSerializerSettings;

        public MediaTypeFormatterJSONnetNewtonsoft(JsonSerializerSettings jsonSerializerSettings) 
        {
            _jsonSerializerSettings = jsonSerializerSettings ?? new JsonSerializerSettings();

            SupportedMediaTypes.Add(new MediaTypeHeaderValue(MediaTypeFormatterJSONnetNewtonsoft.MimeType));
            //Encoding = new UTF8Encoding(false, true);

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
            JsonSerializer serializer = JsonSerializer.Create(_jsonSerializerSettings);

            return Task.Factory.StartNew
                       (
                           () =>
                            {
                                using (StreamReader streamReader = new StreamReader(stream))
                                using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
                                {
                                   return
                                        serializer.Deserialize(jsonTextReader, type);
                                    
                                }
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
            JsonSerializer serializer = JsonSerializer.Create(_jsonSerializerSettings);

            return Task.Factory.StartNew
                       (
                           () =>
                           {
                               using
                                (
                                    JsonTextWriter jsonTextWriter = new JsonTextWriter
                                                                            (
                                                                                new StreamWriter(stream, Encoding.UTF8)
                                                                            )
                                                                    {
                                                                        CloseOutput = false
                                                                    }
                                )
                                {
                                   serializer.Serialize(jsonTextWriter, value);
                                   jsonTextWriter.Flush();
                                }
                            }
                        );
        }

    }
}