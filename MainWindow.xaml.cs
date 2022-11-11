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
using Newtonsoft.Json;

namespace NapierFilteringSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            fldHeader.Visibility = Visibility.Visible;
            fldSubject.Visibility = Visibility.Hidden;
            txtSubject.Visibility = Visibility.Hidden;
            fldSender.Visibility = Visibility.Hidden;
            txtSender.Visibility = Visibility.Hidden;
            fldBody.Visibility = Visibility.Visible;
            txtType.Visibility = Visibility.Hidden;
            fldHeader.MaxLength = 10;
            fldSIRType.Visibility = Visibility.Hidden;
            txtSIR.Visibility = Visibility.Hidden;
            fldSortCode.Visibility = Visibility.Hidden;
            txtSortCode.Visibility = Visibility.Hidden;
        }

        private void fldHeader_TextChanged(object sender, TextChangedEventArgs e)
        {
            string header = fldHeader.Text;
            string headerType;

            if (!String.IsNullOrEmpty(header)) {
                headerType = header.Substring(0, 1).ToUpper();


                switch(headerType)
                {
                    case "S":
                        fldHeader.Visibility = Visibility.Visible;

                        fldSubject.Visibility = Visibility.Hidden;
                        txtSubject.Visibility = Visibility.Hidden;

                        fldSender.Visibility = Visibility.Visible;
                        txtSender.Visibility = Visibility.Visible;
                        fldSender.MaxLength = 15;

                        fldBody.Visibility = Visibility.Visible;
                        fldBody.MaxLength = 140;

                        txtCharLimit.Text = fldBody.Text.Length + " / " + fldBody.MaxLength;

                        txtType.Text = "SMS Text Message";
                        txtType.Visibility = Visibility.Visible;

                        fldSIRType.Visibility = Visibility.Hidden;
                        txtSIR.Visibility = Visibility.Hidden;
                        fldSortCode.Visibility = Visibility.Hidden;
                        txtSortCode.Visibility = Visibility.Hidden;

                        break;

                    case "E":
                        fldHeader.Visibility = Visibility.Visible;

                        fldSubject.Visibility = Visibility.Visible;
                        fldSubject.MaxLength = 20;
                        txtSubject.Visibility = Visibility.Visible;

                        fldSender.Visibility = Visibility.Visible;
                        txtSender.Visibility = Visibility.Visible;

                        fldBody.Visibility = Visibility.Visible;
                        fldBody.MaxLength = 1028;

                        txtCharLimit.Text = fldBody.Text.Length + " / " + fldBody.MaxLength;

                        txtType.Text = "Email";
                        txtType.Visibility = Visibility.Visible;
                        break;

                    case "T":
                        fldHeader.Visibility = Visibility.Visible;

                        fldSubject.Visibility = Visibility.Hidden;
                        txtSubject.Visibility = Visibility.Hidden;

                        fldSender.Visibility = Visibility.Visible;
                        fldSender.MaxLength = 15;
                        txtSender.Visibility = Visibility.Visible;

                        fldBody.Visibility = Visibility.Visible;
                        fldBody.MaxLength = 140;

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
            else
            {
                fldHeader.Visibility = Visibility.Visible;
                fldSubject.Visibility = Visibility.Hidden;
                txtSubject.Visibility = Visibility.Hidden;
                fldSender.Visibility = Visibility.Hidden;
                txtSender.Visibility = Visibility.Hidden;
                fldBody.Visibility = Visibility.Visible;
                txtType.Visibility = Visibility.Hidden;
                fldBody.MaxLength = 0;
                txtCharLimit.Text = fldBody.Text.Length + " / " + fldBody.MaxLength;
                fldSIRType.Visibility = Visibility.Hidden;
                txtSIR.Visibility = Visibility.Hidden;
                fldSortCode.Visibility = Visibility.Hidden;
                txtSortCode.Visibility = Visibility.Hidden;

            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string header = fldHeader.Text;
            string headerType;
            Regex headerRegex = new Regex(@"^(E|T|S|)[0-9]{9}$");


            if (!String.IsNullOrEmpty(header))
            {
                if (headerRegex.IsMatch(header))
                {
                    headerType = header.Substring(0, 1).ToUpper();

                    switch (headerType)
                    {
                       case "S":
                           string smsSender = fldSender.Text;
                           string smsBody = fldBody.Text;
                            Regex SMSRegexSender = new Regex(@"^[\+][0-9]{7,14}$");

                           if (!String.IsNullOrEmpty(smsSender))
                           {
                               if (SMSRegexSender.IsMatch(smsSender))
                               {
                                    if(!String.IsNullOrEmpty(smsBody))
                                    {
                                        string smsSubject = fldSubject.Text;
                                        Message sms = new Message(header, smsSender, smsSubject, Message.ProcessAbbreviations(smsBody));
                                        SaveMessage(sms);
                                        Clear_Fields();
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



                        case "E":
                            string emailSender = fldSender.Text;
                            string emailSubject = fldSubject.Text;
                            string emailBody = fldBody.Text;
                            Regex EmailRegexSender = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

                            if (!String.IsNullOrEmpty(emailSender))
                            {
                                if (EmailRegexSender.IsMatch(emailSender))
                                {
                                    if(!String.IsNullOrEmpty(emailSubject))
                                    {
                                        if(!String.IsNullOrEmpty(emailBody))
                                        {
                                            if(!String.IsNullOrEmpty(fldSortCode.Text) && !String.IsNullOrEmpty(fldSIRType.Text))
                                            {
                                                SIR SIRdata = new SIR(fldSortCode.Text, fldSIRType.Text);
                                                Message email = new Message(header, emailSender, emailSubject, URL.ProcessURL(emailBody));

                                                SIR.WriteSIR(SIRdata);
                                                SaveMessage(email);
                                                
                                                Clear_Fields();


                                            } else
                                            {
                                                Message email = new Message(header, emailSender, emailSubject, URL.ProcessURL(emailBody));
                                                SaveMessage(email);
                                                Clear_Fields();
                                            }
                                        } else
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



                        case "T":
                            string tweetSender = fldSender.Text;
                            string tweetBody = fldBody.Text;
                            Regex TweetRegexSender = new Regex(@"^(@)[A-Za-z0-9_]{1,15}$");
                            Regex TweetRegexBody = new Regex(@"(@)[A-Za-z0-9_]{1,15}");

                            if (!String.IsNullOrEmpty(tweetSender))
                            {
                                if (TweetRegexSender.IsMatch(tweetSender))
                                {
                                    if(!String.IsNullOrEmpty(tweetBody))
                                    {
                                        foreach(var mention in TweetRegexBody.Matches(tweetBody))
                                        {
                                            Mention tweetMention = new Mention(tweetSender, mention.ToString());
                                            Mention.WriteMention(tweetMention);
                                        }
                                        Message tweet = new Message(header, tweetSender, fldSubject.Text, Message.ProcessAbbreviations(tweetBody));
                                        SaveMessage(tweet);
                                        Clear_Fields();



                                    } 
                                    else
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

        private void Clear_Click(object sender, RoutedEventArgs e)
        {

            Clear_Fields();

        }

        private void fldBody_TextChanged(object sender, TextChangedEventArgs e)
        {


            if(Regex.IsMatch(fldSubject.Text,@"^SIR\d{2}/\d{2}/\d{2}$"))
            {
                Regex SortCodeRegex = new Regex(@"[S|s]ort\s[C|c]ode:\s\b\d{2}-\d{2}-\d{2}\b");
                //Regex IncidentRegex = new Regex(@"Nature\sOf\sIncident:\s\b{0}\b");
                List<string> incidents = new List<string>()
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

                foreach (var incident in incidents)
                {
                    string incidentpattern = string.Format(@"[N|n]ature\s[O|o]f\s[I|i]ncident:\s\b{0}\b", incident);

                    if (Regex.IsMatch(fldBody.Text, incidentpattern))
                    {
                        fldSIRType.Text = incident;
                        fldSIRType.Visibility = Visibility.Visible;
                        txtSIR.Visibility = Visibility.Visible;
                        
                    }
                }

                if (SortCodeRegex.IsMatch(fldBody.Text)) 
                {
                    MatchCollection sortmatches = SortCodeRegex.Matches(fldBody.Text);

                    fldSortCode.Visibility = Visibility.Visible;
                    fldSortCode.Text = sortmatches[0].Value.Substring(11);
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

        public void SaveMessage(Message writeableMessage)
        {
            //MessageList jsonmsgList = new MessageList { };
            List<Message> jsonmsgList = new List<Message>();
            string jsonFilepath = @"C:\Napier Filtering System\Messages.json";

            if (!Directory.Exists(@"C:\Napier Filtering System"))
            {
                Directory.CreateDirectory(@"C:\Napier Filtering System");
            }
                
            if (File.Exists(@"C:\Napier Filtering System\Messages.json"))
                {
                jsonmsgList = JsonConvert.DeserializeObject<List<Message>>(File.ReadAllText(jsonFilepath));
                jsonmsgList.Add(writeableMessage);
                File.WriteAllText(jsonFilepath, JsonConvert.SerializeObject(jsonmsgList, Formatting.Indented) + "\r\n");

                MessageBox.Show("Message saved to file!" + "\r\n" + "(" + jsonFilepath + ")", caption: "Napier Filtering System");

                
            } else
                {
                    File.WriteAllText(jsonFilepath,"[]");
                    jsonmsgList = JsonConvert.DeserializeObject<List<Message>>(File.ReadAllText(jsonFilepath));
                    jsonmsgList.Add(writeableMessage);
                    File.WriteAllText(jsonFilepath, JsonConvert.SerializeObject(jsonmsgList, Formatting.Indented) + "\r\n");

                    MessageBox.Show("Message saved to new json file!" + "\r\n" + "(" + jsonFilepath + ")", caption: "Napier Filtering System");
                
                }
           



        }

        public void Clear_Fields()
        {
            fldHeader.Clear();
            fldSubject.Clear();
            fldSender.Clear();
            fldBody.Clear();

        }


    }
}
