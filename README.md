# Napier Bank Message Filtering System

Project for the Year 3 Software Engineering Class at Napier University.


Program can read, process and save three types of messages: SMS, Emails and Tweets, as indicated by the first character of the header field (S,E,T), followed by nine numbers.

>SMS messages have their abbreviations processed and expaned using regex, however a textspeak.csv file is required.

>Emails have their hyperlinks quarantined and saved to a file. These messages can come in two different types, Significant Incident Reports and ordinary Emails - the program differentiates between them two and processes them accordingly, with SIR emails having their specific details (Sort Code, Incident) stored in a separate file.

>Tweets have their abbreviations processed via Regex, and the mentions and hashtags in tweet messages are stored in separate files.

This program uses the Newtonsoft JSON API for JSON file serialization. 

