using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using Newtonsoft.Json;

//https://github.com/ChristianWeyer/Thinktecture.Web.Http/blob/master/Thinktecture.Web.Http/Formatters/JsonpFormatter.cs
namespace Core.Net.HTTP.Formatting
{
    public class MediaTypeFormatterJsonp : MediaTypeFormatterJSONSystem
    {
        private const string MimeType = "application/json";

        private string callbackQueryParameter;

        public MediaTypeFormatterJsonp() 
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(MimeType));

            //MediaTypeMappings.Add
                            //(
                            //    new UriPathExtensionMapping("jsonp", DefaultMediaType)
                            //);

            return;
        }

        public string CallbackQueryParameter 
        {
            get { return callbackQueryParameter ?? "callback"; }
            set { callbackQueryParameter = value; }
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override Task WriteToStreamAsync
                                            (
                                                Type type, object value,
                                                Stream stream,
                                                System.Net.Http.HttpContent content,
                                                System.Net.TransportContext transportContext
                                            )
        {
            string callback;

            /*
            if (IsJsonpRequest(formatterContext.Response.RequestMessage, out callback))
            {
                return Task.Factory.StartNew
                (
                    () => 
                    {
                        var writer = new StreamWriter(stream);
                        writer.Write(string.Format("callback{0}", "("));
                        writer.Flush();
                        base.OnWriteToStreamAsync
                            (
                                type, 
                                value, 
                                stream,
                                contentHeaders, 
                                formatterContext, 
                                transportContext
                            ).Wait();

                        writer.Write(")");
                        writer.Flush();

                    }
                );
            }
            else
            {
                return base.OnWriteToStreamAsync
                            (
                                type, 
                                value, 
                                stream, 
                                contentHeaders, 
                                formatterContext, 
                                transportContext
                            );
            }
            */

            return default(Task);
        }

        private bool IsJsonpRequest(HttpRequestMessage request, out string callback) 
        {
            callback = null;

            if (request.Method != HttpMethod.Get) 
            {
                return false;
            }

            //var query = HttpUtility.ParseQueryString(request.RequestUri.Query);
            //callback = query[CallbackQueryParameter];

            return 
                !string.IsNullOrEmpty(callback);
        }
    }
}