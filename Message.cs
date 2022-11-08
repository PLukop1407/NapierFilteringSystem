using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

namespace NapierFilteringSystem
{
    public class Message
    {

        public string Header 
        { 
            get;
            set;
        }
        public string Sender 
        { 
            get; 
            set; 
        }
        public string Subject 
        { 
            get; 
            set; 
        }
        public string Body 
        { 
            get; 
            set; 
        }



        public Message(string msgHeader, string msgSender, string msgSubject, string msgBody)
        {
            Header = msgHeader;
            Sender = msgSender;
            Subject = msgSubject;
            Body = msgBody;

        }


        public static string ProcessAbbreviations(string msgBody)
        {
            Dictionary<string, string> abbreviations = File.ReadAllLines(@"C:\textwords.csv").Select(line => line.Split(',')).ToDictionary(line => line[0], line => line[1]);

            foreach(var abbrev in abbreviations)
            {
                string abbrevPattern = string.Format(@"\b{0}\b", abbrev.Key);
                string abbrevExpanded = abbrev.Key +  " <" + abbrev.Value + ">";
                msgBody = Regex.Replace(msgBody, abbrevPattern, abbrevExpanded, RegexOptions.IgnoreCase);
            }
            return msgBody;

        }


        public static string ProcessURLs(string msgBody)
        {
            string emailBody = msgBody;
            Regex urlRegex = new Regex(@"(http(s)?|ftp):\/\/(www.)?([\da-zA-Z\-_]{0,2184})(.[a-z]{2,5})(.[a-z]{2,5})?(/)?([\da-zA-Z\-\?\,\'\/\+^&%\$#_@]+]{0,2184})?(.[a-z]{3,5})?");

            foreach(var link in urlRegex.Matches(msgBody)) {

                URL QuarantinedURL = new URL(link.ToString());




            }



            return emailBody;
        }

    }
}
