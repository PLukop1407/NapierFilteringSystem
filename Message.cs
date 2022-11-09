using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Windows;

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
    }
}
