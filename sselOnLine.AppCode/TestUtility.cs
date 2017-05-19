using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;

namespace sselOnLine.AppCode
{
    public static class TestUtility
    {
        public static TestRoot GetTestRoot(string testPath)
        {
            if (string.IsNullOrEmpty(testPath))
                throw new ArgumentNullException("testPath");

            if (!File.Exists(testPath))
                return null;

            using (StreamReader reader = new StreamReader(testPath))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(TestRoot));
                var result = (TestRoot)deserializer.Deserialize(reader);
                reader.Close();
                return result;
            }
        }

        public static IEnumerable<TestRoot> GetTests()
        {
            string securePath = Path.Combine(ConfigurationManager.AppSettings["SecurePath"], "testfiles");

            if (!Directory.Exists(securePath))
                throw new InvalidOperationException(string.Format("Cannot find path: {0}", securePath));

            foreach (string p in Directory.GetFiles(securePath, "*.xml"))
            {
                yield return GetTestRoot(p);
            }
        }

        public static string SerializeTest(TestRoot root)
        {
            using (TextWriter writer = new StringWriter())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TestRoot));
                serializer.Serialize(writer, root);
                return writer.ToString();
            }
        }
    }
}
