/*  This is the Mention Class for the Napier Bank Message Filtering System. It contains the getters, setters, class constructor and method for the Mention class
 *  The attributes of this class are senderID and mentionID, which indicate who the sender mentioned in their tweet.
 *  The method for this class will write a mention object to the Mentions.json file, so that it can be accessed and viewed in the ListWindow.
 * 
 *  This class was written by Patrikas Lukosius, 40405699@live.napier.ac.uk for the Napier Bank Message Filtering System.
 */




using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace NapierFilteringSystem
{
    //The mention class is used specifically for Tweet messages, so that any mentions in the message body can be found and stored in a separate file.
    public class Mention
    {
        //Getters and Setters
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

        //Class Constructor
        public Mention (string sender, string mention)
        {
            senderID = sender;
            mentionID = mention;
        }

        /*  The WriteMention method is called whenever a Mention object is created for a Tweet message.
         *  This method will create Mentions.json with the correct formatting, if the file doesn't exist
         *  The file is deserialized into a list of Mentions, and the Mention passed into this method is added to this list, before being serialized and written to the JSON file again.
         */

        public static Mention WriteMention(Mention tweet)
        {
            string mentionJsonfilepath = @"C:\Napier Filtering System\Mentions.json"; //Filepath for the JSON file.
            List<Mention> listOfMentions = new List<Mention>(); //List of Mentions to store the contents of the JSON file after deserialization.

            if(!Directory.Exists(@"C:\Napier Filtering System")) //Check for the directory of the JSON files, if it doesn't exist, it's created.
            {
                Directory.CreateDirectory(@"C:\Napier Filtering System");
            }

            //If the JSON file exists, it's deserialized into the List of Mentions.
            if (File.Exists(mentionJsonfilepath))
            {
                listOfMentions = JsonConvert.DeserializeObject<List<Mention>>(File.ReadAllText(mentionJsonfilepath));
                listOfMentions.Add(tweet); //Add the tweet mention to the list
                File.WriteAllText(mentionJsonfilepath, JsonConvert.SerializeObject(listOfMentions, Formatting.Indented) + "\r\n"); //Serialize the list and write it to the file.

            } else //If the JSON file doesn't exist, create a new one with list of objects formatting.
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
