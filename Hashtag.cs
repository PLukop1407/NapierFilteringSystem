/*  This is the Hashtag Class for the Napier Bank Message Filtering System. It contains the getters, setters, class constructor and method for the Hashtag class.
 *  The attributes for this class are hashtag and count, with the count incrementing if the hashtag already exists in the Hashtags.json file.
 *  The only method for this class is WriteHashtag, which will receive a new Hashtag object - only storing it in the JSON file if that particular hashtag doesn't already exist.
 * 
 *  This class was written by Patrikas Lukosius, 40405699@live.napier.ac.uk for the Napier Bank Message Filtering System.
 */


using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace NapierFilteringSystem
{
    public class Hashtag
    {   
        //Getters and Setters
        public string hashtag
        {
            get;
            set;
        }
        public int count
        {
            get;
            set;
        }


        //Class Consturctor
        public Hashtag (string hashIn, int countIn)
        {
            hashtag = hashIn;
            count = countIn;
        }
        
        /*  The WriteHashtag method will write the Hashtag object to the JSON file. This method is only used for Tweets if they contain hashtags.
         *  The Hashtag will only be written to the file if it doesn't already exist, otherwise that hashtag will have its use count incremented.
         */

        public static Hashtag WriteHashtag(Hashtag trend)
        {
            string hashJsonfilepath = @"C:\Napier Filtering System\Hashtags.json"; //Filepath for the Hashtags JSON file
            List<Hashtag> listOfTrends = new List<Hashtag>(); //Create a new list of Hashtags to deserialize the JSON file into.

            if (!Directory.Exists(@"C:\Napier Filtering System")) //Create directory for the JSON file if it doesn't exist.
            {
                Directory.CreateDirectory(@"C:\Napier Filtering System");
            }

            //If the JSON file exists, it'll get deserialized into a List of Hashtags.
            if(File.Exists(hashJsonfilepath)) 
            {
                listOfTrends = JsonConvert.DeserializeObject<List<Hashtag>>(File.ReadAllText(hashJsonfilepath)); //Deserialize the JSON file into the List of Hashtags.
                if ((listOfTrends.Find(listOfTrends => listOfTrends.hashtag.Contains(trend.hashtag)) != null)) //If the new hashtag is already in the JSON file, increment the count of that specific hashtag.
                {

                    int listIndex = listOfTrends.FindIndex(listOfTrends => listOfTrends.hashtag == trend.hashtag); //Store the index of the List where the new hashtag = hashtag in the json file.
                    listOfTrends[listIndex].count += 1; //Increment the count of the list index where new hashtag = hashtag in the json file.
                } else //If the new hashtag is not in the file, add it.
                {
                    listOfTrends.Add(trend); //Add the hashtag to the list
                }
                File.WriteAllText(hashJsonfilepath, JsonConvert.SerializeObject(listOfTrends, Formatting.Indented) + "\r\n"); //Serialize the list of hashtags and write it to the file.



            } else //If the file doesn't exist, create a new file with the correct formatting. Don't bother checking if the new hashtag exists in the file, since it needs to create a new file anyway.
            {
                File.WriteAllText(hashJsonfilepath, "[]"); //List of Objects formatting for the hashtags.json file.

                listOfTrends = JsonConvert.DeserializeObject<List<Hashtag>>(File.ReadAllText(hashJsonfilepath)); //Deserialize JSON file into List of Hashtags
                listOfTrends.Add(trend); //Add new hashtag to list
                File.WriteAllText(hashJsonfilepath, JsonConvert.SerializeObject(listOfTrends, Formatting.Indented) + "\r\n"); //Serialize the list, write it to file

            }
            return trend;
        }
    }
}
