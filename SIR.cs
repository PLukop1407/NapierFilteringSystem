/*  This is the SIR class for the Napier Bank Message Filtering System. It contains the getters, setters, class constructor and method for the SIR class.
 *  The attributes of this class are SortCode and IncidentType. These are the two bits of data the program needs to store whenever a SIR email is saved.
 *  The only method for this class is WriteSIR which stores a SIR object in the SIR.json file.
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
using System.Windows.Controls;
using System.Windows.Navigation;

namespace NapierFilteringSystem
{
    public class SIR
    {
        //Getters and Setters
        public string SortCode
        {
            get;
            set;
        }
        public string IncidentType
        {
            get;
            set;
        }


        //Class Constructor
        public SIR(string sirSortcode, string sirIncident)
        {
            SortCode = sirSortcode;
            IncidentType = sirIncident;

        }


        /*  The WriteSIR method will create a new JSON file for storing SIRs, if it doesn't exist. This method is only used for SIR emails specifically.
         *  The file is deserialized into a List of SIRs, so that a new SIR object can be added.
         *  Once the new SIR object is added, the list is serialized and written to the JSON file.
         */
        public void WriteSIR(SIR email)
        {
            string SIRjsonFilepath = @"C:\Napier Filtering System\SIR.json"; //Filepath for the SIR JSON file.
            List<SIR> jsonSIRlist = new List<SIR>(); //Initialising the List of SIRs for deserializing the JSON file into.

            //Create directory for the JSON file if it doesn't exist.
            if(!Directory.Exists(@"C:\Napier Filtering System")) {
                Directory.CreateDirectory(@"C:\Napier Filtering System");
            }


            //If the JSON file exists, the program deserializes it into the List of SIR objects, so that a new one can be added.
            if(File.Exists(SIRjsonFilepath))
            {
                jsonSIRlist = JsonConvert.DeserializeObject<List<SIR>>(File.ReadAllText(SIRjsonFilepath));
                jsonSIRlist.Add(email); //Add new SIR object to the list.
                File.WriteAllText(SIRjsonFilepath, JsonConvert.SerializeObject(jsonSIRlist, Formatting.Indented) + "\r\n"); //Serialize the list and write it to the JSON file.

            } else //If the JSON file doesn't exist, then a new one is created.
            {
                File.WriteAllText(SIRjsonFilepath, "[]"); //Create a new JSON file with the List of Objects formatting.
                jsonSIRlist = JsonConvert.DeserializeObject<List<SIR>>(File.ReadAllText(SIRjsonFilepath)); //Deserialize the JSON file into a List of SIR objects.
                jsonSIRlist.Add(email); //Add new SIR to the List
                File.WriteAllText(SIRjsonFilepath, JsonConvert.SerializeObject(jsonSIRlist, Formatting.Indented) + "\r\n"); //Serialize the list and then write it to the JSON file.

            }
        }
    }
}
