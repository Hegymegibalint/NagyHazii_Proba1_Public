using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Nagyhazii_Proba1.Persistence.MsSql
{
    public static class XmlDataLoader
    {
        public static T LoadFromXml<T>(string filePath) where T : class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                return (T)serializer.Deserialize(fs);
            }
        }
    }
}
