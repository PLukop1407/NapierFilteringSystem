using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace NapierFilteringSystem
{
    public class Mention
    {
        public string senderID
        {
            get;
            set;
        }

        public string mentionID
        {
            get;
            set;
        }

        public Mention (string sender, string mention)
        {
            senderID = sender;
            mentionID = mention;
        }


        public static Mention WriteMention(Mention tweet)
        {
            string mentionJsonfilepath = @"C:\Napier Filtering System\Mentions.json";
            List<Mention> listOfMentions = new List<Mention>();

            if(!Directory.Exists(@"C:\Napier Filtering System"))
            {
                Directory.CreateDirectory(@"C:\Napier Filtering System");
            }

            if (File.Exists(mentionJsonfilepath))
            {
                listOfMentions = JsonConvert.DeserializeObject<List<Mention>>(File.ReadAllText(mentionJsonfilepath));
                listOfMentions.Add(tweet);
                File.WriteAllText(mentionJsonfilepath, JsonConvert.SerializeObject(listOfMentions, Formatting.Indented) + "\r\n");

            } else
            {
                File.WriteAllText(mentionJsonfilepath, "[]");
                listOfMentions = JsonConvert.DeserializeObject<List<Mention>>(File.ReadAllText(mentionJsonfilepath));
                listOfMentions.Add(tweet);
                File.WriteAllText(mentionJsonfilepath, JsonConvert.SerializeObject(listOfMentions, Formatting.Indented) + "\r\n");

            }
           return tweet;
        }
    }
}
