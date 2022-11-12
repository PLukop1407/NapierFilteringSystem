/*  Program developed by Patrikas Lukosius, 40405699@live.napier.ac.uk
 *  The purpose of this program is to process three types of messages that can be input by the user and save those messages in corresponding JSON files.
 *  
 *  SMS Messages have their abbreviations processed and are then stored in a JSON file (Messages.json)
 *  
 *  Emails can either be Significant Incident Reports or ordinary emails. SIRs have their details stored in a separate JSON file (SIR.json).
 *  Both, ordinary emails and SIRs are stored in the Messages json file.
 * 
 *  Tweets have their abbreviations processed, and then any mentions and hashtags are stored in their respective files (Mentions.json & Hashtags.json)
 *  
 *  The SIRs, Mentions and Hashtags can be viewed in a separate Lists window.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.IO;
using Newtonsoft.Json; //Using the Newtonsoft JSON API. JsonConvert.Deserialize / JsonConvert.Serialize are used from this library.

namespace NapierFilteringSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow() 
        {
            //When the Main Window is opened, all of the UI components are initialised. Only the Body and Header fields are visible.
            InitializeComponent();
            fldHeader.Visibility = Visibility.Visible;
            fldSubject.Visibility = Visibility.Hidden;
            txtSubject.Visibility = Visibility.Hidden;
            fldSender.Visibility = Visibility.Hidden;
            txtSender.Visibility = Visibility.Hidden;
            fldBody.Visibility = Visibility.Visible;
            fldBody.IsEnabled = false;
            txtType.Visibility = Visibility.Hidden;
            fldHeader.MaxLength = 10;
            fldSIRType.Visibility = Visibility.Hidden;
            txtSIR.Visibility = Visibility.Hidden;
            fldSortCode.Visibility = Visibility.Hidden;
            txtSortCode.Visibility = Visibility.Hidden;
        }

        private void fldHeader_TextChanged(object sender, TextChangedEventArgs e)
            //This is the logic for when the header field changes. The goal is to switch the visibility of UI elements depending on what kind of message the user is entering.
        {
            string header = fldHeader.Text;
            string headerType;

            if (!String.IsNullOrEmpty(header)) {
                headerType = header.Substring(0, 1).ToUpper();


                switch(headerType)
                {
                    case "S": //If the message is an SMS Text, hide the Subject and set the max lengths of fields accordingly.
                        fldHeader.Visibility = Visibility.Visible;

                        fldSubject.Visibility = Visibility.Hidden;
                        txtSubject.Visibility = Visibility.Hidden;

                        fldSender.Visibility = Visibility.Visible;
                        txtSender.Visibility = Visibility.Visible;
                        fldSender.MaxLength = 15; //Max number length is 15 characters

                        fldBody.Visibility = Visibility.Visible;
                        fldBody.IsEnabled = true;
                        fldBody.MaxLength = 140; //Max length for a text is 140 characters.

                        txtCharLimit.Text = fldBody.Text.Length + " / " + fldBody.MaxLength; //Display character limit below the body field.

                        txtType.Text = "SMS Text Message"; //Display what kind of message is being entered.
                        txtType.Visibility = Visibility.Visible;

                        fldSIRType.Visibility = Visibility.Hidden;
                        txtSIR.Visibility = Visibility.Hidden;
                        fldSortCode.Visibility = Visibility.Hidden;
                        txtSortCode.Visibility = Visibility.Hidden;

                        break;

                    case "E": //If the message is an Email, make all of the fields visible.
                        fldHeader.Visibility = Visibility.Visible;

                        fldSubject.Visibility = Visibility.Visible;
                        fldSubject.MaxLength = 20; //Max length for the subject is 20 characters.
                        txtSubject.Visibility = Visibility.Visible;

                        fldSender.Visibility = Visibility.Visible;
                        txtSender.Visibility = Visibility.Visible;

                        fldBody.Visibility = Visibility.Visible;
                        fldBody.IsEnabled = true;
                        fldBody.MaxLength = 1028; //Max length for an email is 1028 characters.

                        txtCharLimit.Text = fldBody.Text.Length + " / " + fldBody.MaxLength;

                        txtType.Text = "Email";
                        txtType.Visibility = Visibility.Visible;
                        break;

                    case "T": //If the message is a tweet, hide the subject.
                        fldHeader.Visibility = Visibility.Visible;

                        fldSubject.Visibility = Visibility.Hidden;
                        txtSubject.Visibility = Visibility.Hidden;

                        fldSender.Visibility = Visibility.Visible;
                        fldSender.MaxLength = 15; //Max handle length for a Twitter user
                        txtSender.Visibility = Visibility.Visible;

                        fldBody.Visibility = Visibility.Visible;
                        fldBody.IsEnabled = true;
                        fldBody.MaxLength = 140; //Max length for a tweet is 140 characters.

                        txtCharLimit.Text = fldBody.Text.Length + " / " + fldBody.MaxLength;

                        txtType.Text = "Tweet";
                        txtType.Visibility = Visibility.Visible;

                        fldSIRType.Visibility = Visibility.Hidden;
                        txtSIR.Visibility = Visibility.Hidden;
                        fldSortCode.Visibility = Visibility.Hidden;
                        txtSortCode.Visibility = Visibility.Hidden;
                        break;
                } 
            }
            else //The else case here will set the UI to its default state if the user's input doesn't match the format.
            { 
                fldHeader.Visibility = Visibility.Visible;
                fldSubject.Visibility = Visibility.Hidden;
                txtSubject.Visibility = Visibility.Hidden;
                fldSender.Visibility = Visibility.Hidden;
                txtSender.Visibility = Visibility.Hidden;
                fldBody.Visibility = Visibility.Visible;
                txtType.Visibility = Visibility.Hidden;
                fldBody.MaxLength = 0;
                fldBody.IsEnabled = false;
                fldBody.Text = "";
                txtCharLimit.Text = fldBody.Text.Length + " / " + fldBody.MaxLength;
                fldSIRType.Visibility = Visibility.Hidden;
                txtSIR.Visibility = Visibility.Hidden;
                fldSortCode.Visibility = Visibility.Hidden;
                txtSortCode.Visibility = Visibility.Hidden;

            }
        }

        private void Save_Click(object sender, RoutedEventArgs e) //This is the method for when the Save button is clicked, it calls various methods based on the message type.
        {
            string header = fldHeader.Text;
            string headerType;
            Regex headerRegex = new Regex(@"^(E|T|S|)[0-9]{9}$"); //Regex for the header. Checks if the header is in the format E123456789


            if (!String.IsNullOrEmpty(header))
            {
                if (headerRegex.IsMatch(header)) //Checking if the header matches the Regex
                {
                    headerType = header.Substring(0, 1).ToUpper(); //Store the first character of the header so it can be switched

                    switch (headerType)
                    {
                       case "S": //The program will process abbreviations and store the message if it's an SMS
                           string smsSender = fldSender.Text;
                           string smsBody = fldBody.Text;
                            Regex SMSRegexSender = new Regex(@"^[\+][0-9]{7,14}$"); //Regex for the sender's phone number (+447575786787 etc)

                           if (!String.IsNullOrEmpty(smsSender)) //Nested IF statements used to check if all required fields are filled out, giving the user a context-sensitive error
                           {
                               if (SMSRegexSender.IsMatch(smsSender))
                               {
                                    if(!String.IsNullOrEmpty(smsBody))
                                    {
                                        //If all of the input validation passes, the SMS text is used to create a message object
                                        //The body is sent to the ProcessAbbreviations method of the Message class, which will expand all abbreviations.
                                        Message sms = new Message(header, smsSender, fldSubject.Text, Message.ProcessAbbreviations(smsBody)); 
                                        SaveMessage(sms); //The message is then sent to the SaveMessage method which will create a new JSON file and store the SMS message.
                                        Clear_Fields(); //Clear the fields after the message is saved.
                                    } else
                                    {
                                        MessageBox.Show("SMS body cannot be empty!", caption: "Error");
                                    }
                               }
                               else
                               {
                                   MessageBox.Show("Sender number doesn't match international number format!", caption: "Error");
                               }
                           }
                            else
                            {
                                MessageBox.Show("Sender number cannot be empty!", caption: "Error");
                            }
                            break;



                        case "E": //The program will differentiate between regular email and SIR email and quarantine URLs if the message is an Email
                            string emailSender = fldSender.Text;
                            string emailSubject = fldSubject.Text;
                            string emailBody = fldBody.Text;
                            Regex EmailRegexSender = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"); //Email regex
                            bool emailType = Regex.IsMatch(emailSubject, @"^SIR([0-2][0-9]|[3][0-1])/([0][1-9]|[1][0-2])/[0-99]{2}$"); //The boolean here is used to determine if the email is ordinary or a SIR

                            if (!String.IsNullOrEmpty(emailSender))
                            {
                                if (EmailRegexSender.IsMatch(emailSender))
                                {
                                    if(!String.IsNullOrEmpty(emailSubject))
                                    {
                                        if(!String.IsNullOrEmpty(emailBody))
                                        {
                                            switch (emailType) //Switch based on emailType. True is a SIR email, false is a regular email.
                                            {
                                                case true: //SIR email
                                                    if(!String.IsNullOrEmpty(fldSortCode.Text))
                                                    {
                                                        if(!String.IsNullOrEmpty(fldSIRType.Text)) //If the Sort Code and Incident Type are present, the method can save the SIR email.
                                                        {
                                                            SIR SIRdata = new SIR(fldSortCode.Text, fldSIRType.Text); //Create new SIR object with the field data.
                                                            Message SIRemail = new Message(header, emailSender, emailSubject, URL.ProcessURL(emailBody)); //Create new Message object, process URLs in the body.
                                                            SIR.WriteSIR(SIRdata); //Calls the Write method for SIR objects, writing them to the relevant JSON file.
                                                            SaveMessage(SIRemail); //Calls the method to write the Message object to the relevant JSON file.

                                                            Clear_Fields(); //Clear fields after the SIR data and Email data are stored.

                                                            if (Application.Current.Windows.OfType<ListWindow>().Any()) //This checks if the list window is open, if so, it updates the lists in that window.
                                                            {
                                                                ListWindow listWindow = Application.Current.Windows.OfType<ListWindow>().First();
                                                                listWindow.Update_Lists();
                                                            }

                                                        } else
                                                        {
                                                            MessageBox.Show("Incident Type cannot be empty!", caption: "Error");
                                                        }
                                                    } else
                                                    {
                                                        MessageBox.Show("Sort Code cannot be empty!", caption: "Error");
                                                    }
                                                    break;

                                                case false: //Regular Email
                                                    Message email = new Message(header, emailSender, emailSubject, URL.ProcessURL(emailBody)); //Create a new Message object with the field values, processing the URLs in the body.
                                                    SaveMessage(email); //Call the method to write the Message object to relevant JSON file.
                                                    Clear_Fields(); //Clear fields after the message is saved.
                                                    break;
                                            }
                                        } else //Various errors for failing input validation.
                                        {
                                            MessageBox.Show("Email body cannot be empty!", caption: "Error");
                                        }
                                    } else
                                    {
                                        MessageBox.Show("Email subject cannot be empty!", caption: "Error");
                                    }
                                } else
                                {
                                    MessageBox.Show("Sender email must be in a valid format! (john@gmail.com)", caption: "Error");
                                }
                            } 
                            else
                            {
                                MessageBox.Show("Sender field cannot be empty!", caption: "Error");
                            }

                            break;



                        case "T": //If the message is a Tweet, the program needs to check if the handle is valid, then parse the body for Mentions and Hashtags, so that they can be stored.
                            string tweetSender = fldSender.Text;
                            string tweetBody = fldBody.Text;
                            Regex TweetHandleRegex = new Regex(@"(@)[A-Za-z0-9_]{1,15}"); //Twitter Handle Regex, checks that the handle starts with '@' and is followed by 1-15 characters.
                            //Regex TweetRegexBody = new Regex(@"(@)[A-Za-z0-9_]{1,15}"); //
                            Regex TweetRegexHashtag = new Regex(@"(#)\w+"); //Hashtag regex, checks for strings of characters that start with '#' followed by letters and numbers.

                            if (!String.IsNullOrEmpty(tweetSender)) //Input Validation in the form of Nested IF statements, used to provide context-sensitive errors.
                            {
                                if (TweetHandleRegex.IsMatch(tweetSender)) 
                                {
                                    if(!String.IsNullOrEmpty(tweetBody)) 
                                    {
                                        foreach(var mention in TweetHandleRegex.Matches(tweetBody)) //This loop will iterate through the mentions in the body (if there are any) and it'll create Mention objects to be stored in the Mention JSON file.
                                        {
                                            Mention tweetMention = new Mention(tweetSender, mention.ToString()); //Create mention object, turn var mention into string for the object.
                                            Mention.WriteMention(tweetMention); //Call the Write method for the Mention.
                                        }

                                        foreach(var hashtag in TweetRegexHashtag.Matches(tweetBody)) //This loop will iterate through every hashtag in the body (if there are any), creating a Hashtag object for them.
                                        {
                                            Hashtag trend = new Hashtag(hashtag.ToString(), 1); //Create hashtag object, turn var trend into string for the object.
                                            Hashtag.WriteHashtag(trend); //Call the write method for the hashtag.
                                        }

                                        if (Application.Current.Windows.OfType<ListWindow>().Any()) //If the Lists window is open, this will update the lists.
                                        {
                                            ListWindow listWindow = Application.Current.Windows.OfType<ListWindow>().First();
                                            listWindow.Update_Lists(); //Call update method in the Lists window.
                                        }



                                        Message tweet = new Message(header, tweetSender, fldSubject.Text, Message.ProcessAbbreviations(tweetBody)); //After the Mentions and Hashtags are processed, the message then has the abbreviations in the body processed, before being stored.
                                        SaveMessage(tweet); //This method will save the tweet, once all the processing is done.
                                        Clear_Fields(); //Clear the fields after saving the message.


                                    } 
                                    else //Various errors for failing input validation.
                                    {
                                        MessageBox.Show("Tweet body cannot be empty!", caption: "Error");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Twitter ID doesn't match format! (@Person)", caption: "Error");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Twitter ID cannot be empty!", caption: "Error");
                            }
                            break;
                    }
                } 
                else
                {
                    MessageBox.Show("Header must be ten characters in the correct format! (E123456789)", caption: "Error");
                }
            }
            else
            {
                MessageBox.Show("Header cannot be empty!", caption: "Error");
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e) //Calls the Clear_Fields method so that it can.. clear the fields.
        {

            Clear_Fields();

        }
        /*  This is a weird one. This is how I get the Sort Code and Incident Type without having the user to enter it multiple times.
         *  I thought it was a bit more intuitive and stylish, however it might be a bit confusing to someone who didn't make the program.
         *  If the subject field matches SIR Regex, the body checks for a Sort Code and Incident Type whenever the body text changes.
         *  If it finds the Sort Code or Incident Type in the body, these will be displayed in hidden (now visible) fields.
         *  This is useful as it means the program has the Sort Code and Incident automatically stored in a field, for when the SIR object is created.
         */
        private void fldBody_TextChanged(object sender, TextChangedEventArgs e) 
        {


            if(Regex.IsMatch(fldSubject.Text,@"^SIR\d{2}/\d{2}/\d{2}$")) //Check if the subject matches SIR regex
            {
                Regex SortCodeRegex = new Regex(@"[S|s]ort\s[C|c]ode:\s\b\d{2}-\d{2}-\d{2}\b"); //Sort code regex for when it's in the body
                List<string> incidents = new List<string>() //List of incidents for checking if the body contains one of them
                {
                    "Theft",
                    "Staff Attack",
                    "ATM Theft",
                    "Raid",
                    "Customer Abuse",
                    "Staff Abuse",
                    "Bomb Threat",
                    "Terrorism",
                    "Suspicious Incident",
                    "Intelligence",
                    "Cash Loss",
                };

                foreach (var incident in incidents) //Loop through the incidents list to see if the user entered a valid incident.
                {
                    string incidentpattern = string.Format(@"[N|n]ature\s[O|o]f\s[I|i]ncident:\s\b{0}\b", incident); //Creating an incident pattern where incidents from the list are substituted into the Regex

                    if (Regex.IsMatch(fldBody.Text, incidentpattern)) //If the loop finds a match with one of the incidents in the list, the Incident Type field is made visible and the incident is listed in that field.
                    {
                        fldSIRType.Text = incident;
                        fldSIRType.Visibility = Visibility.Visible;
                        txtSIR.Visibility = Visibility.Visible;
                        
                    }
                }

                if (SortCodeRegex.IsMatch(fldBody.Text)) //If the body matches the Sort Code Regex, the sort code field is made visible and the sort code is inserted into the field.
                {
                    MatchCollection sortmatches = SortCodeRegex.Matches(fldBody.Text);

                    fldSortCode.Visibility = Visibility.Visible;
                    fldSortCode.Text = sortmatches[0].Value.Substring(11); //Cut off the "Sort Code:" part, so that only the code itself is in the field.
                    txtSortCode.Visibility = Visibility.Visible;

                }
                else 
                {
                    fldSortCode.Visibility = Visibility.Hidden;
                    fldSortCode.Text = "";
                    txtSortCode.Visibility = Visibility.Hidden;

                    txtSIR.Visibility = Visibility.Hidden;
                    fldSIRType.Visibility = Visibility.Hidden;
                    fldSIRType.Text = "";
                }
            } else
            {
                fldSortCode.Visibility = Visibility.Hidden;
                fldSortCode.Text = "";
                txtSortCode.Visibility = Visibility.Hidden;

                txtSIR.Visibility = Visibility.Hidden;
                fldSIRType.Visibility = Visibility.Hidden;
                fldSIRType.Text = "";
            }

            txtCharLimit.Text = fldBody.Text.Length + " / " + fldBody.MaxLength;

        }
        /*  This is the method for writing the message. I'm not really sure why I didn't put it in the Message class.
         *  This method takes a Message object and writes it to a JSON file (Messages.json), thanks to the Netwonsoft JSON library.
         */
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

        public void Clear_Fields() //Clear_Fields()
        {
            fldHeader.Clear();
            fldSubject.Clear();
            fldSender.Clear();
            fldBody.Clear();
            fldSIRType.Clear();
            fldSortCode.Clear();

        }

        private void Open_Lists(object sender, RoutedEventArgs e) //This is the method for opening the List Window, it checks if a window of that type is already open and if not, it opens one. Otherwise it gives the user an error.
        {
            if(!Application.Current.Windows.OfType<ListWindow>().Any()) //Check if any ListWindows are already open
            {
                ListWindow listWindow = new ListWindow(); //Create new reference to List Window, using that reference to open a new list window.
                listWindow.Show();
            } else
            {
                MessageBox.Show("List Window already open!", caption: "Error"); //Give the user an error if they try to open another list window.
            }
        }
    }
}
