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
                            Regex SMSRegexSender = new Regex(@"^[\+][0-9]{7,14}$");

                            if(!String.IsNullOrEmpty(smsSender))
                            {
                                if (SMSRegexSender.IsMatch(smsSender))
                                {
                                    MessageBox.Show("Worked");
                                }
                                else
                                {
                                    MessageBox.Show("Sender number doesn't match international number format!");
                                }

                            } else
                            {
                                MessageBox.Show("Sender number cannot be empty!");
                            }
                            break;



                        case "E":
                            MessageBox.Show("Email");
                            break;



                        case "T":
                            string tweetSender = fldSender.Text;
                            Regex TweetRegexSender = new Regex(@"^(@)[A-Za-z0-9_]{1,15}$");

                            if(!String.IsNullOrEmpty(tweetSender))
                            {
                                if(TweetRegexSender.IsMatch(tweetSender))
                                {
                                    MessageBox.Show("Worked!");
                                } else
                                {
                                    MessageBox.Show("Twitter ID doesn't match format! (@Person)");
                                }
                            } else
                            {
                                MessageBox.Show("Twitter ID cannot be empty!");
                            }
                            break;



                        default:
                            MessageBox.Show("Error");
                            break;
                    }
                } else
                {
                    MessageBox.Show("Header must be ten characters in the correct format! (E123456789)");
                }
            } else
            {
                MessageBox.Show("Header cannot be empty!");
            }

        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            fldHeader.Clear();
            fldSubject.Clear();
            fldSender.Clear();
            fldBody.Clear();
        }

        private void fldBody_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtCharLimit.Text = fldBody.Text.Length + " / " + fldBody.MaxLength;
        }
    }
}
