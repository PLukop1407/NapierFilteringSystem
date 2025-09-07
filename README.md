# Message Filtering System

A **C# WPF** project for demonstrating regular expression handling and JSON object serialization.

This project was created as part of my university coursework, wherein the brief outlined an application that can be used in a hypothetical bank setting, allowing users to filter internal bank messages. This was to be done by utilising regular expressions, and a JSON serialization library in-order to store the messages.

---
## Features
- Dynamic **WPF UI** that allows users to input three different types of messages: SMS Text Messages (S), Emails (E), and Tweets (T)
- Each message type is filtered accordingly, with SMS messages using regular expressions to validate the sender's number, also utilising an external .csv table of abbreviations in-order to expand abbreviations such as LOL to Laugh Out Loud in the message.
- Emails validate the email address, and will detect and filter links - storing them in a separate JSON file and replacing the link in the message with a quarantined link.
- Tweets will validate the Twitter handle(s) and store trend counts (i.e. #Github - 1), as well as store mentions (i.e. @User1 mentioned @User2)
- Emails also include a special case called Significant Incident Reports (SIR), which are detected if the Email Subject header starts with SIRdd/mm/yy, and the Email Body contains Sort Code: xx-xx-xx, and Nature of Incident: type.
- Message storage is handled using **Newtonsoft.Json** for JSON Object serialization and deserialization. 
- Separate window for displaying the various different messages that are stored.
---
## Technologies Used
- **C# WPF** (Windows Presentation Foundation)
- **.NET**
- **Newtonsoft.Json** - For JSON serialization/deserialization in message storage

---
## Areas for improvement
- Better use for OOP principles in handling the messages (Message Class, Email/Text/Tweet Subclass) would help with the reusability of code and readability.
- Three-tier architecture would help isolate important aspects of the application (Presentation, Logic, Data) would help with the separation of logic, scalability, and flexibility.
---
## Setup & Installation

### 1. Clone repo
### 2. Open the C# WPF Project (``NapierFilteringSystem.sln``)
### 3. Ensure Netwonsoft.Json is present with NuGet
### 4. Build and run WPF project in Visual Studio.
---

### License
This is one of my university projects and it serves portfolio and demonstration purposes only. Feel free to use it for learning or experimentation. There's no formal license, feel free to reach out if you've any questions. 
