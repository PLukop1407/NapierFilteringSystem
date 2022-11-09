using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using static NapierFilteringSystem.MainWindow;
using System.Windows;

namespace NapierFilteringSystem
{
    public class URL
    {
        public string Url
        {
            get;
            set;
        }

        public URL(string msgUrl)
        {
            Url = msgUrl;
        }

        public class QuarantineList
        {
            public List<URL> URLs { get; set; }
        }



        public static string ProcessURL(string msgBody)
        {
            string jsonFilepath = @"C:\Napier Filtering System\QuarantinedURLs.json";
            Regex urlRegex = new Regex(@"(http(s)?|ftp):\/\/(www.)?([\da-zA-Z\-_]{0,2184})(\.[a-z]{2,5})(\.[a-z]{2,5})?(/)?([\da-zA-Z\-\?\,\'\/\+^&%\$#_@]+]{0,2184})?(\.[a-z]{3,5})?");
            QuarantineList jsonurlList = new QuarantineList();

            if(!Directory.Exists(@"C:\Napier Filtering System"))
            {
                Directory.CreateDirectory(@"C:\Napier Filtering System");
            }

            if (File.Exists(jsonFilepath))
            {
                jsonurlList = JsonConvert.DeserializeObject<QuarantineList>(File.ReadAllText(jsonFilepath));

                foreach (var link in urlRegex.Matches(msgBody))
                {
                    URL QuarantinedURL = new URL(link.ToString());
                    jsonurlList.URLs.Add(QuarantinedURL);
                    string URLPattern = string.Format(@"\b{0}\b", link);
                    msgBody = Regex.Replace(msgBody, URLPattern, "<Quarantined URL>", RegexOptions.IgnoreCase);

                }

                File.WriteAllText(jsonFilepath, JsonConvert.SerializeObject(jsonurlList, Formatting.Indented) + "\r\n");
            }
            else
            {
                File.WriteAllText(jsonFilepath, "{\"URLs\": []}");
                jsonurlList = JsonConvert.DeserializeObject<QuarantineList>(File.ReadAllText(jsonFilepath));

                foreach (var link in urlRegex.Matches(msgBody))
                {
                    URL QuarantinedURL = new URL(link.ToString());
                    jsonurlList.URLs.Add(QuarantinedURL);
                    string URLPattern = string.Format(@"\b{0}\b", link);
                    msgBody = Regex.Replace(msgBody, URLPattern, " <Quarantined URL> ", RegexOptions.IgnoreCase);


                }
                File.WriteAllText(jsonFilepath, JsonConvert.SerializeObject(jsonurlList, Formatting.Indented) + "\r\n");
            }
            return msgBody;
        }
    }
}
