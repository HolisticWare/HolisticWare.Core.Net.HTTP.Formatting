using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;

namespace Core.Net.HTTP.Formatting
{
    public class RouteDataMapping : MediaTypeMapping 
    {

        private readonly string _routeDataValueName;
        private readonly string _routeDataValueValue;

        public RouteDataMapping
                        (
                            string routeDataValueName, 
                            string routeDataValueValue, 
                            MediaTypeHeaderValue mediaType
                        )
                        : base(mediaType) 
        {
            _routeDataValueName = routeDataValueName;
            _routeDataValueValue = routeDataValueValue;

            return;
        }

        public RouteDataMapping(string routeDataValueName, string routeDataValueValue, string mediaType) : 
            base(mediaType) {

            _routeDataValueName = routeDataValueName;
            _routeDataValueValue = routeDataValueValue;

        }

        public override double TryMatchMediaType(HttpRequestMessage request)
        {
            throw new NotImplementedException();
        }
    }
}