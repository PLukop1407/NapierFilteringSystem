/*  This is the URL class written for the Napier Bank Message Filtering System. It contains the getters, setters, class constructor and method for the URL class.
 *  The only attribute for this class is Url, as that's the only data the program needs to store with this class.
 *  The only method for this class is ProcessURL. It will parse through the body of a message and replace any URLs with "<URL Quarantined>", writing whatever URL it quarantined to a JSON file.
 * 
 *  This class was written by Patrikas Lukosius, 40405699@live.napier.ac.uk for the Napier Bank Messaging Filtering System.
 */




using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace NapierFilteringSystem
{
    public class URL
    {
        //Getter and Setter
        public string Url
        {
            get;
            set;
        }

        //Class Constructor
        public URL(string msgUrl)
        {
            Url = msgUrl;
        }

        /*  This method uses Regex to parse through the message body in order to find URLs. This method is only used for emails.
         *  Using a foreach loop, the program detects every URL in the message body using Regex.Matches.
         *  Detected URLs are turned into URL objects and stored in a list of URLs. In the message body, they're replaced with <Quarantined URL>
         *  Once the foreach loop is done, the List of URLs is serialized and written to the URL.json file, whereas the message body is returned so that it can be used in a Message object.
         */
        public static string ProcessURL(string msgBody)
        {
            string jsonFilepath = @"C:\Napier Filtering System\QuarantinedURLs.json"; //Filepath for the JSON file
            Regex urlRegex = new Regex(@"(http(s)?|ftp):\/\/(www.)?([\da-zA-Z\-_]{0,2184})(\.[a-z]{2,5})(\.[a-z]{2,5})?(/)?([\da-zA-Z\-\?\,\'\/\+^&%\$#_@=]+]{0,2184})?(\.[a-z]{3,5})?"); //URL Regex, this was a rough one, won't explain it.
            List<URL> jsonurlList = new List<URL>(); //Initialise List of URLs for Deserializing / Serializing.

            if(!Directory.Exists(@"C:\Napier Filtering System")) //Create the Directory for the JSON file if it doesn't exist.
            {
                Directory.CreateDirectory(@"C:\Napier Filtering System");
            }

            //If the file exists, we can write to it.
            if (File.Exists(jsonFilepath))
            {
                jsonurlList = JsonConvert.DeserializeObject<List<URL>>(File.ReadAllText(jsonFilepath)); //Deserialize the contents of the JSON file into a List of URLs.

                foreach (var link in urlRegex.Matches(msgBody)) //For each match,create a URL object and add it to the list. Quarantine the matching link in the body.
                {
                    URL QuarantinedURL = new URL(link.ToString()); //Create new URL, convert var to string for the object.
                    jsonurlList.Add(QuarantinedURL); //Add found URL to list 
                    string URLPattern = string.Format(@"\b{0}\b", link); //Create pattern for the specific URL that was found
                    msgBody = Regex.Replace(msgBody, URLPattern, "<Quarantined URL>", RegexOptions.IgnoreCase); //Replace the URL in the body with <Quarantined URL>

                }

                File.WriteAllText(jsonFilepath, JsonConvert.SerializeObject(jsonurlList, Formatting.Indented) + "\r\n"); //Serialize the List of URLs and write it to the JSON file.
            }
            else //If the file doesn't exist, we've to create it and then write to it.
            {
                File.WriteAllText(jsonFilepath, "[]"); //Format the file for List of Objects.
                jsonurlList = JsonConvert.DeserializeObject<List<URL>>(File.ReadAllText(jsonFilepath)); //Deserialize the JSON file into a List of URLs

                foreach (var link in urlRegex.Matches(msgBody)) //For each match, create a URL object and add it to the list, replace it with <Quarantined URL> in the body.
                {
                    URL QuarantinedURL = new URL(link.ToString());
                    jsonurlList.Add(QuarantinedURL);
                    string URLPattern = string.Format(@"\b{0}\b", link);
                    msgBody = Regex.Replace(msgBody, URLPattern, " <Quarantined URL> ", RegexOptions.IgnoreCase); 


                }
                File.WriteAllText(jsonFilepath, JsonConvert.SerializeObject(jsonurlList, Formatting.Indented) + "\r\n"); //Serialize the list and write it to the JSON file.
            }
            return msgBody;
        }
    }
}
