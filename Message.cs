/*  This is the Message Class for the Napier Bank Message Filtering System. It contains the getters, setters, class constructor and method for the Message Class
 *  The attributes of the class are Header, Sender, Subject and Body
 *  The only method of this class is ProcessAbbreviations, which takes msgBody as its parameter, returning the variable once the abbreviations are expanded.
 *  
 *  This class was written by Patrikas Lukosius, 40405699@live.napier.ac.uk for the Napier Bank Message Filtering System.
 */


using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Windows;
using Newtonsoft.Json;

namespace NapierFilteringSystem
{

    //The message class is used every time the user tries to save a message. It has four attributes which are Header, Sender, Subject and Body - although not all of them are used for every message.
    public class Message 
    {
        //Getters and Setters
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


        //Class Constructor
        public Message(string msgHeader, string msgSender, string msgSubject, string msgBody)
        {
            Header = msgHeader;
            Sender = msgSender;
            Subject = msgSubject;
            Body = msgBody;

        }

        /*  The Process Abbreviations method is called whenever a message type may have abbreviations (SMS, Tweet).
         *  This method reads the textwords csv file into a Dictionary and uses Regex in conjunction with a loop to parse through the body, and expand any abbreviations found.
         */
        public static string ProcessAbbreviations(string msgBody)
        {
            //Read the abbrevations file into a Dictionary, with the key being an abbreviation and the value being the meaning of said abbreviation.
            Dictionary<string, string> abbreviations = File.ReadAllLines(@"C:\textwords.csv").Select(line => line.Split(',')).ToDictionary(line => line[0], line => line[1]);

            //Iterate through the Dictionary, checking if any abbreviation is found in the body of the message.
            foreach(var abbrev in abbreviations)
            {
                string abbrevPattern = string.Format(@"\b{0}\b", abbrev.Key); //Insert the abbreviation into a regex variable, which is then used to replace any abbreviations found.
                string abbrevExpanded = abbrev.Key +  " <" + abbrev.Value + ">"; //This variable stores the abbreviation and the expansion, replacing any abbreviation found in the body. This means that "LOL" becomes "LOL <Laugh Out Loud>"
                msgBody = Regex.Replace(msgBody, abbrevPattern, abbrevExpanded, RegexOptions.IgnoreCase); //Replace any abbreviations found in the message body using Regex.Replace
            }

            return msgBody; //Return the message body when finished.

        }

         public void SaveMessage(Message writeableMessage) 
        {
            List<Message> jsonmsgList = new List<Message>(); //Create a List of Messages to deserialize the json file into.
            string jsonFilepath = @"C:\Napier Filtering System\Messages.json"; //Filepath to Messages.json, I wanted to add a way for the user to change where this is stored but I don't have more time to spend on this.

            if (!Directory.Exists(@"C:\Napier Filtering System")) //Checks if the directory for the json file exists, if not, this check creates that directory.
            {
                Directory.CreateDirectory(@"C:\Napier Filtering System");
            }
                
            if (File.Exists(@"C:\Napier Filtering System\Messages.json")) //If the file already exists, the program can deserialize it into the List of Messages.
                {
                jsonmsgList = JsonConvert.DeserializeObject<List<Message>>(File.ReadAllText(jsonFilepath)); //Deserialize the json file
                jsonmsgList.Add(writeableMessage); //Add the new Message to the list
                File.WriteAllText(jsonFilepath, JsonConvert.SerializeObject(jsonmsgList, Formatting.Indented) + "\r\n"); //Serialize the list and write it to the file

                MessageBox.Show("Message saved to file!" + "\r\n" + "(" + jsonFilepath + ")", caption: "Napier Bank - Message Filtering System"); //Messagebox letting the user know that the message has been saved

                
            } else //If the file doesn't exist, then there's some code to create a json file with a tiny bit of formatting
                {
                    File.WriteAllText(jsonFilepath,"[]"); //[] defines a list of Objects in the JSON file, meaning we can serialize our message objects into it with the Newtonsoft library.
                    jsonmsgList = JsonConvert.DeserializeObject<List<Message>>(File.ReadAllText(jsonFilepath)); //Deserialize newly-made JSON file into List of Messages
                    jsonmsgList.Add(writeableMessage); //Add the new Message to the list
                    File.WriteAllText(jsonFilepath, JsonConvert.SerializeObject(jsonmsgList, Formatting.Indented) + "\r\n"); //Serialize the list and write it to the file

                    MessageBox.Show("Message saved to new json file!" + "\r\n" + "(" + jsonFilepath + ")", caption: "Napier Bank - Message Filtering System"); //Different messagebox letting the user know that a JSON file was created and the message was stored in it.
                
                }
        }




    }
}
