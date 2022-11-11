using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NapierFilteringSystem
{
    public class Hashtag
    {
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



        public Hashtag (string hashIn, int countIn)
        {
            hashtag = hashIn;
            count = countIn;
        }

        public static Hashtag WriteHashtag(Hashtag trend)
        {
            string hashJsonfilepath = @"C:\Napier Filtering System\Hashtags.json";
            List<Hashtag> listOfTrends = new List<Hashtag>();

            if (!Directory.Exists(@"C:\Napier Filtering System"))
            {
                Directory.CreateDirectory(@"C:\Napier Filtering System");
            }

            if(File.Exists(hashJsonfilepath))
            {
                listOfTrends = JsonConvert.DeserializeObject<List<Hashtag>>(File.ReadAllText(hashJsonfilepath));
                if ((listOfTrends.Find(listOfTrends => listOfTrends.hashtag.Contains(trend.hashtag)) != null))
                {

                    int listIndex = listOfTrends.FindIndex(listOfTrends => listOfTrends.hashtag == trend.hashtag);
                    listOfTrends[listIndex].count += 1;
                } else
                {
                    listOfTrends.Add(trend);
                }
                File.WriteAllText(hashJsonfilepath, JsonConvert.SerializeObject(listOfTrends, Formatting.Indented) + "\r\n");



            } else
            {
                File.WriteAllText(hashJsonfilepath, "[]");

                listOfTrends = JsonConvert.DeserializeObject<List<Hashtag>>(File.ReadAllText(hashJsonfilepath));
                listOfTrends.Add(trend);
                File.WriteAllText(hashJsonfilepath, JsonConvert.SerializeObject(listOfTrends, Formatting.Indented) + "\r\n");

            }








            return trend;
        }





    }
}
