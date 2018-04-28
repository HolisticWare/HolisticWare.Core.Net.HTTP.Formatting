using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Net.HTTP.Formatting
{
    public class MediaTypeFormatterTextCSV : MediaTypeFormatter 
    {
        private const string MimeType = "text/csv";

        public MediaTypeFormatterTextCSV() 
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(""));

            return;
        }

        public MediaTypeFormatterTextCSV(MediaTypeMapping mediaTypeMapping) : this() 
        {
            MediaTypeMappings.Add(mediaTypeMapping);
        }

        public MediaTypeFormatterTextCSV(IEnumerable<MediaTypeMapping> mediaTypeMappings) : this() 
        {
            foreach (var mediaTypeMapping in mediaTypeMappings) 
            {
                MediaTypeMappings.Add(mediaTypeMapping);
            }

            return;
        }

        public override bool CanWriteType(Type type) 
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
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
            object value = null;
            HttpContentHeaders ch = content.Headers;
            return Task<object>.Factory.StartNew
                                            (
                                                () =>
                                                {
                                                    //writeStream(type, value, stream, ch);
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

        //private utils
        private object writeStream(Type type, object value, Stream stream, HttpContentHeaders contentHeaders) 
        {
            /*
            //NOTE: We have check the type inside CanWriteType method
            //If request comes this far, the type is IEnumerable. We are safe.

            Type itemType = type.GetGenericArguments()[0];

            StringWriter _stringWriter = new StringWriter();

            _stringWriter.WriteLine(
                string.Join<string>(
                    ",", itemType.GetProperties().Select(x => x.Name )
                )
            );

            foreach (var obj in (IEnumerable<object>)value) {

                var vals = obj.GetType().GetProperties().Select(
                    pi => new { 
                        Value = pi.GetValue(obj, null)
                    }
                );

                string _valueLine = string.Empty;

                foreach (var val in vals) {

                    if (val.Value != null) {

                        var _val = val.Value.ToString();

                        //Check if the value contans a comma and place it in quotes if so
                        if (_val.Contains(","))
                            _val = string.Concat("\"", _val, "\"");

                        //Replace any \r or \n special characters from a new line with a space
                        if (_val.Contains("\r"))
                            _val = _val.Replace("\r", " ");
                        if (_val.Contains("\n"))
                            _val = _val.Replace("\n", " ");

                        _valueLine = string.Concat(_valueLine, _val, ",");

                    } else {

                        _valueLine = string.Concat(string.Empty, ",");
                    }
                }

                _stringWriter.WriteLine(_valueLine.TrimEnd(','));
            }

            var streamWriter = new StreamWriter(stream);
                streamWriter.Write(_stringWriter.ToString());
            */

            return null;
        }

        private bool isTypeOfIEnumerable(Type type) 
        {
            /*
            foreach (Type interfaceType in type.GetInterfaces()) 
            {
                if (interfaceType == typeof(IEnumerable))
                {
                    return true;
                }
            }
            */

            return false;
        }
    }
}