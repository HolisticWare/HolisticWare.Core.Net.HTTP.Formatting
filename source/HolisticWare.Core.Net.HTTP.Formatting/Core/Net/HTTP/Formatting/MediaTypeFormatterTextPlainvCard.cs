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
    /// <summary>
    /// Media type formatter text plainv card.
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/VCard"/>
    /// 
    public class MediaTypeFormatterTextPlainvCard : MediaTypeFormatter 
    {
        private const string MimeType = "text/vcard";

        public MediaTypeFormatterTextPlainvCard() 
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(MimeType));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);

            SupportedMediaTypes.Add
                                (
                                   new MediaTypeHeaderValue(MimeType)
                                );
            MediaTypeMappings.Add
                                (
                                    new RequestHeaderMapping
                                            (
                                                "Accept",
                                                MimeType,
                                                StringComparison.OrdinalIgnoreCase,
                                                false,  
                                                MimeType
                                            )
                                );

            return;
        }

        public override bool CanReadType(Type type)
        {
            //if (typeof(Contact).IsAssignableFrom(type)
            //    || typeof(IEnumerable<Contact>).IsAssignableFrom(type))
            //{
            //    return base.CanWriteType(type);
            //}

            return true;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

        // Wrong docs?
        // public override bool CanWriteResult(Type type)
        // {
            /*
            In some scenarios you have to override CanWriteResult instead of CanWriteType.Use 
            CanWriteResult if the following conditions are true:

            Your action method returns a model class.
            There are derived classes which might be returned at runtime.
            You need to know at runtime which derived class was returned by the action.
            */

            // return true;
        // }

        public override Task<object> ReadFromStreamAsync
                                            (
                                                Type type, 
                                                Stream readStream, 
                                                System.Net.Http.HttpContent content, 
                                                IFormatterLogger formatterLogger
                                            )
        {
            return
                new Task<object>
                (
                    () => 
                    {
                        return new StreamReader(readStream).ReadToEnd();
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

        /*
        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            IServiceProvider serviceProvider = context.HttpContext.RequestServices;
            var logger = serviceProvider.GetService(typeof(ILogger<VcardOutputFormatter>)) as ILogger;

            var response = context.HttpContext.Response;

            var buffer = new StringBuilder();
            if (context.Object is IEnumerable<Contact>)
            {
                foreach (Contact contact in context.Object as IEnumerable<Contact>)
                {
                    FormatVcard(buffer, contact, logger);
                }
            }
            else
            {
                var contact = context.Object as Contact;
                FormatVcard(buffer, contact, logger);
            }
            return response.WriteAsync(buffer.ToString());
        }

        private static void FormatVcard(StringBuilder buffer, Contact contact, ILogger logger)
        {
            buffer.AppendLine("BEGIN:VCARD");
            buffer.AppendLine("VERSION:2.1");
            buffer.AppendFormat($"N:{contact.LastName};{contact.FirstName}\r\n");
            buffer.AppendFormat($"FN:{contact.FirstName} {contact.LastName}\r\n");
            buffer.AppendFormat($"UID:{contact.ID}\r\n");
            buffer.AppendLine("END:VCARD");
            logger.LogInformation($"Writing {contact.FirstName} {contact.LastName}");
        }
        */


    }
}
