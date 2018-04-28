using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using System.Json;

// https://weblog.west-wind.com/posts/2012/Mar/09/Using-an-alternate-JSON-Serializer-in-ASPNET-Web-API

namespace Core.Net.HTTP.Formatting
{

    public class MediaTypeFormatterJSONSystem : MediaTypeFormatter 
    {
        private const string MimeType = "application/json";

        public MediaTypeFormatterJSONSystem() 
        {
            SupportedMediaTypes.Add
                                (
                                   new MediaTypeHeaderValue(MediaTypeFormatterJSONSystem.MimeType)
                                );
            
            return;
        }

        public override bool CanWriteType(Type type)
        {
            // don't serialize JsonValue structure use default for that
            if (type == typeof(JsonValue) || type == typeof(JsonObject) || type == typeof(JsonArray))
            {
                return false;
            }

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
            Task<object> task = null;

            task = Task<object>.Factory.StartNew
                               (
                                   () =>
                                   {
                                       System.Web.Script.Serialization.JavaScriptSerializer ser = null;

                                       ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                                       string json;

                                       using (var sr = new StreamReader(stream))
                                       {
                                           json = sr.ReadToEnd();
                                       }

                                       object val = ser.Deserialize(json, type);

                                       return val;
                                   }
                                );

            return task;
        }

        public override Task WriteToStreamAsync
                                        (
                                            Type type, 
                                            object value, 
                                            Stream stream,
                                            System.Net.Http.HttpContent content,
                                            System.Net.TransportContext transportContext
                                        )
        {
            Task task = null;
            task = Task.Factory.StartNew
                                (
                                    () =>
                                    {
                                        System.Web.Script.Serialization.JavaScriptSerializer ser = null;
                                        ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                                        string json = ser.Serialize(value);

                                        byte[] b = Encoding.UTF8.GetBytes(json);
                                        stream.Write(b, 0, b.Length);
                                        stream.Flush();
                                    }
                                );

            return task;
        }
    }
}