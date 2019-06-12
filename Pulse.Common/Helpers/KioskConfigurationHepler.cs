namespace Pulse.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    public static class KioskConfigurationHepler
    {
        private static XDocument _doc = null;
        private const string KIOSKS = "Kiosks";
        private const string SECURITY = "Security";
        private const string PATH = "Configurations\\KioskSecurity.xml";

        public static string GetValueFromSecurity(string localName)
        {
            InitXDocument();
            if (_doc != null)
            {
                IEnumerable<XElement> kiosks = _doc.Elements(KIOSKS);
                return kiosks.Elements(SECURITY).Elements().Where(x => x.Name.LocalName.ToLower().Equals(localName.ToLower()))
                    .Select(x => x.Value)
                    .FirstOrDefault();
            }
            return string.Empty;
        }

        public static string GetValueAttributeOfElement(string localName, string attributeName)
        {
            InitXDocument();
            if (_doc != null)
            {
                IEnumerable<XElement> kiosks = _doc.Elements(KIOSKS);
                var elm = kiosks.Elements(SECURITY).Elements().FirstOrDefault(x => x.Name.LocalName.ToLower().Equals(localName.ToLower()));
                return elm.Attribute(attributeName).Value;
            }

            return string.Empty;
        }

        public static void UpdateValueForElement(string elmName, string value)
        {
            InitXDocument();
            if (_doc != null)
            {
                IEnumerable<XElement> kiosks = _doc.Elements(KIOSKS);
                var elm = kiosks.Elements(SECURITY).Elements().FirstOrDefault(x => x.Name.LocalName.ToLower().Equals(elmName.ToLower()));
                elm.SetValue(value);
                _doc.Save(string.Format(@"{0}\{1}", AppDomain.CurrentDomain.BaseDirectory, PATH));
            }
        }

        private static void InitXDocument()
        {
            if (_doc == null)
            {
                string path = string.Format(@"{0}\{1}", AppDomain.CurrentDomain.BaseDirectory, PATH);

                if (File.Exists(path)) _doc = XDocument.Load(path);
            }
        }
    }
}
