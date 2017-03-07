using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace sselOnLine.AppCode
{
    public static class SecureDocument
    {
        public static string GetContentType(string filepath)
        {
            string result;

            FileInfo fi = new FileInfo(filepath);

            switch(fi.Extension)
            {
                case ".pdf":
                    result = "application/pdf";
                    break;
                case ".doc":
                    result = "application/msword";
                    break;
                case ".xls":
                    result = "application/vnd.ms-excel";
                    break;
                case ".xml":
                    result = "text/xml";
                    break;
                case ".zip":
                    result = "application/zip";
                    break;
                default:
                    result = "text/plain";
                    break;
            }

            return result;
        }

        public static string LookupContentType(string extension)
        {
            string result = "text/plain";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://svn.apache.org/repos/asf/httpd/httpd/branches/2.0.x/docs/conf/mime.types");
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            StreamReader reader = new StreamReader(resp.GetResponseStream());
            List<string> lines = new List<string>();
            while (!reader.EndOfStream)
            {
                string temp = reader.ReadLine();
                if(!temp.StartsWith("#"))
                    lines.Add(temp);
            }
            reader.Close();

            if (extension.StartsWith(".")) extension = extension.Substring(1);

            Dictionary<string, string> lookup = new Dictionary<string, string>();

            foreach (string item in lines)
            {
                string[] parts = item.Split(Convert.ToChar(9));
                if (parts.Length > 0)
                {
                    string mime = parts[0];
                    for (int x = 1; x < parts.Length; x++)
                    {
                        string ext = parts[x].Trim();
                        if (!string.IsNullOrEmpty(ext))
                        {
                            if (lookup.ContainsKey(ext))
                                lookup[ext] = mime;
                            else
                                lookup.Add(ext, mime);
                        }
                    }
                }
            }

            if (lookup.ContainsKey(extension))
                result = lookup[extension];

            return result;
        }
    }
}
