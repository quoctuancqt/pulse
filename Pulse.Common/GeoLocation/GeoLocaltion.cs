namespace Pulse.Common.GeoLocation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Xml.Linq;

    public static class GeoLocaltion
    {
        private const string URL_GET_LOCATION = "http://freegeoip.net/xml/";

        private const string URL_GET_IP_DNS = "https://api.ipify.org/";

        private const string URL_GOOGLE_API = "http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false";

        private static IDictionary<string, string> dic;

        private static XElement _xml;

        public static string GetDnsIpAddress()
        {
            WebClient client = new WebClient();
            return client.DownloadString(URL_GET_IP_DNS);
        }

        public static string GetCountryName()
        {
            if (_xml == null) BuildXml();

            return _xml.Elements("CountryName").Select(x => x.Value).FirstOrDefault();
        }

        public static string GetLatitude()
        {
            if (_xml == null) BuildXml();

            return dic["Latitude"];
        }

        public static string GetLongitude()
        {
            if (_xml == null) BuildXml();

            return dic["Longitude"];
        }

        private static void BuildXml()
        {
            WebClient client = new WebClient();
            var content = client.DownloadString(URL_GET_LOCATION + GetDnsIpAddress());
            _xml = XElement.Parse(content);
            RequestToGoogleApi(GetCountryName());
        }

        private static void RequestToGoogleApi(string address)
        {
            if (dic == null)
            {
                var requestUri = string.Format(URL_GOOGLE_API, Uri.EscapeDataString(address));

                var request = WebRequest.Create(requestUri);
                var response = request.GetResponse();
                var xdoc = XDocument.Load(response.GetResponseStream());

                var result = xdoc.Element("GeocodeResponse").Element("result");
                var locationElement = result.Element("geometry").Element("location");
                dic = new Dictionary<string, string>();
                dic.Add("Latitude", locationElement.Element("lat").Value);
                dic.Add("Longitude", locationElement.Element("lng").Value);
            }

        }
    }

}
