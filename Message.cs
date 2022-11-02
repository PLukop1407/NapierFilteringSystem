using System;
using System.Collections.Generic;
using System.Text;

namespace NapierFilteringSystem
{
    class Message
    {

        public string Header { get; set; }
        public string Sender { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }



        public Message(string msgHeader, string msgSender, string msgSubject, string msgBody)
        {
            Header = msgHeader;
            Sender = msgSender;
            Subject = msgSubject;
            Body = msgBody;

        }


        public static string ProcessAbbreviations(string msgBody)
        {
            string smsBody = msgBody;




        }


    }
}
