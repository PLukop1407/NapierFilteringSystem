using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace NapierFilteringSystem
{
    public class SIR
    {

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



        public SIR(string sirSortcode, string sirIncident)
        {
            SortCode = sirSortcode;
            IncidentType = sirIncident;

        }

        public static SIR WriteSIR(SIR email)
        {
            string SIRjsonFilepath = @"C:\Napier Filtering System\SIR.json";
            List<SIR> jsonSIRlist = new List<SIR>();

            if(!Directory.Exists(@"C:\Napier Filtering System")) {
                Directory.CreateDirectory(@"C:\Napier Filtering System");
            }

            if(File.Exists(SIRjsonFilepath))
            {
                jsonSIRlist = JsonConvert.DeserializeObject<List<SIR>>(File.ReadAllText(SIRjsonFilepath));
                jsonSIRlist.Add(email);
                File.WriteAllText(SIRjsonFilepath, JsonConvert.SerializeObject(jsonSIRlist, Formatting.Indented) + "\r\n");

                MessageBox.Show("SIR saved to file!" + "\r\n" + "(" + SIRjsonFilepath + ")", caption: "Napier Filtering System");

            } else
            {
                File.WriteAllText(SIRjsonFilepath, "[]");
                jsonSIRlist = JsonConvert.DeserializeObject<List<SIR>>(File.ReadAllText(SIRjsonFilepath));
                jsonSIRlist.Add(email);
                File.WriteAllText(SIRjsonFilepath, JsonConvert.SerializeObject(jsonSIRlist, Formatting.Indented) + "\r\n");

                MessageBox.Show("SIR saved to new file!" + "\r\n" + "(" + SIRjsonFilepath + ")", caption: "Napier Filtering System");
            }
            return email;
        }
       

    }
}
