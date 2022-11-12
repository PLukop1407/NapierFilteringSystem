/*  This is the code for the Lists Window, which reads the SIR, Mention and Hashtag JSON files - listing their contents in a separate window for the user to read.
 *  The only real function in this file is Update_Lists(), which is called when this window intialises and whenever the user saves a new message.
 * 
 *  The lists are printed into textfields via loops that are limited by the count of entries in the Lists of Objects (SIR, Mention, Hashtag).
 *  The XAML code for the window denotes the textfields as read only, so the user can't edit them, and adds a vertical scrollbar if any of the text in the lists go off screen.
 *  If there are no files or if the files are empty, the text fields will tell the user that no data is stored.
 *  
 *  This was programmed by Patrikas Lukosius, 40405699@live.napier.ac.uk for the Napier Bank Message Filtering System.
 */





using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NapierFilteringSystem
{
    /// <summary>
    /// Interaction logic for ListWindow.xaml
    /// </summary>
    public partial class ListWindow : Window
    {


        public ListWindow()
        {
            InitializeComponent();
            Update_Lists(); //Call the Update_Lists function when the window is opened.

        }

        private void Button_Click(object sender, RoutedEventArgs e) //Close the window if the Close button is clicked.
        {
            Close();
        }

        public void Update_Lists()
        {
            //Filepaths for all the files
            string SIRfilepath = @"C:\Napier Filtering System\SIR.json";
            string mentionfilepath = @"C:\Napier Filtering System\Mentions.json";
            string hashfilepath = @"C:\Napier Filtering System\Hashtags.json";

            //Clear the fields before updating them
            fldSIRlist.Text = "";
            fldMentionList.Text = "";
            fldHashtagList.Text = "";


            //Check if the SIR JSON file exists.
            if(File.Exists(SIRfilepath))
            {
                List<SIR> listOfSIRs = JsonConvert.DeserializeObject<List<SIR>>(File.ReadAllText(SIRfilepath)); //Create the list of SIRs and deserialize the file so that the contents can be displayed.

                if(listOfSIRs.Count > 0) //If there is at least one entry in the list, display it.
                {
                    //This loop will iterate for every object in the list.
                    for (int i = 0; i < listOfSIRs.Count; i++) 
                    {

                        fldSIRlist.AppendText("Sort Code: " + listOfSIRs[i].SortCode + "\r\n" + "Nature of Incident: " + listOfSIRs[i].IncidentType + "\r\n"); //Display both attributes of the SIR object for every object.
                        fldSIRlist.AppendText("-------------------------------"); //Formatting
                        fldSIRlist.AppendText("\r\n"); //Formatting
                    }
                } else //If there are no entries in the List of SIRs, print No SIRs stored.
                {
                    fldSIRlist.Text = "\t       [ No SIRs stored ]";
                }
            } else //If there is no SIR.json file, print No SIRs stored.
            {
                fldSIRlist.Text = "\t       [ No SIRs stored ]";
            }

            //Check if the Mention JSON file exists.
            if(File.Exists(mentionfilepath))
            {
                List<Mention> listOfMentions = JsonConvert.DeserializeObject<List<Mention>>(File.ReadAllText(mentionfilepath)); //Deserialize file into List of Mentions so that the contents can be displayed.
                if (listOfMentions.Count > 0) //Check if there's at least one entry in the list.
                {
                    //Iterates for every entry in the list.
                    for (int i = 0; i < listOfMentions.Count; i++)
                    {
                        fldMentionList.AppendText("From " + listOfMentions[i].mentionID + " to " + listOfMentions[i].senderID + "\r\n"); //Display both attributes of the Mention object.
                        fldMentionList.AppendText("-------------------------------"); //Formatting
                        fldMentionList.AppendText("\r\n"); //Formatting
                    }
                } else //If there aren't any entries in the List of Mentions, print No Mentions Stored.
                {
                    fldMentionList.Text = "\t [ No Mentions stored ]";
                }
            } else //If there isn't a Mentions JSON file, print No Mentions Stored.
            {
                fldMentionList.Text = "\t [ No Mentions stored ]";
            }

            //Check if the Hashtag JSON file exists.
            if(File.Exists(hashfilepath))
            {
                List<Hashtag> listOfTrends = JsonConvert.DeserializeObject<List<Hashtag>>(File.ReadAllText(hashfilepath)); //Deserialize the file into a List of Hashtags so that the contents can be displayed.

                if(listOfTrends.Count > 0) //Check if there's at least one entry in the list.
                {
                    //Iterate for every entry in the list of hashtags.
                    for (int i = 0; i < listOfTrends.Count; i++)
                    {
                        fldHashtagList.AppendText("Trend: " + listOfTrends[i].hashtag + "\r\n" + "Number of Instances: " + listOfTrends[i].count + "\r\n"); //Output both attributes of the Hashtag object
                        fldHashtagList.AppendText("-------------------------------"); //Formatting
                        fldHashtagList.AppendText("\r\n"); //Formatting
                    }
                } else //if there are no entries in the list, print No Trends Stored
                {
                    fldHashtagList.Text = "\t     [No Trends stored ]";
                }
            } else //If there isn't a Hashtag JSON file, print No Trends Stored.
            {
                fldHashtagList.Text = "\t     [No Trends stored ]";
            }
        }
    }
}
