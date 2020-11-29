# Napier Bank Messaging System
Napier Bank Messaging System (Edinburgh Napier, Software Engineering Module Coursework).

This system allows both users and employees to create enquiiries and post updates about the company. Managers can also access the system and create incident reports about their branch.

Prerequisits in order to run the:
* Application:
	* Copy the "textwords.csv" into the working directory.
	* If you have any pre-existing messages (in "messages.json"), copy the file into the working directory.
* UnitTests:
	* Copy the "textwords.csv" into the working directory.
	* Copy the "import-test.txt" into the working directory.

If you're unsure where the working directory is, it is where the .exe is active. If you started the application in Visual Studio then this will probably be "../PresentationLayer/bin/Debug/", and for Unit Tests it's most likely "../UnitTest/bin/Debug/".

### Message types and requirements:
* __SMS__ - text messages sent from mobile phones over mobile services.
	* Properties:
		* Sender - who the text is from.
			* Will be a phone number.
			* In an international format (starts with ‘+’).
		* Message Text - simply the message of enquiry.
			* Limit of 140 characters.
			* May contain textspeak abbreviations (for example, lol, rofl).
				* Should be automatically expanded in the message.
* __Email__ - online messages.
	* Properties:
		* Sender - can be either the user or a manager.
			* Standard email address (example@napierbank.com).
		* Subject - a brief sentence highlighting the reason for the message.
			* Limit of 20 characters.
			* Can be automatically generated for SIRs (more on this below).
		* Message Text - regarding an enquiry or an incident.
			* Limit of 1028 characters.
			* URLs received must be quarantined for security inspection.
			* SIRs can also be (partially) generated (more below also).
	* Formats:
		* Standard Email Messages - standard emails from users.
		* Significant Incident Report - sent by branch managers (maybe implement certification to create one) reporting serious incidents.
			* Email Subject format - “SIR dd/mm/yy” (SIR + the incident date).
			* Message Text - standard again but must, by default, consist of:
				* Branch sort code in the format: “xx-xx-xx”.
				* Nature: “Nature of Incident: <incident>”. Can be any of:
				* Theft
				* Staff Attack
				* ATM Theft
				* Raid
				* Customer Attack
				* Staff Abuse
				* Bomb Threat
				* Terrorism
				* Suspicious Incident
				* Intelligence
				* Cash Loss
* __Tweet__ - text sent via Twitter (as Tweets).
	* Properties:
		* Sender - from the user’s Twitter account.
			* Their account ID - “@exampleID”.
			* Limit of 15 characters.
		* Message Text - enquiry text that the Tweet contains.
			* Limit of 140 characters.
			* May also contain textspeak abbreviations.
	* Formats:
			* Hashtags - used to group a Tweet with a topic.
			* Mentions - using another Twitter account’s ID (@) in a message.
* UI Requirements:
	* Use an input form, allowing text message headers to be entered in a heading text box and the message text in a body text box.
	* Display messages as mentioned above; headers as a heading, body text in the body.
	* Display certain messages in lists:
		* SIR list.
		* Hashtag (Trending) list.
		* Mentions list.
* Additional Requirements:
	* Input (a list of) messages from a file.
	* Automatically identify a message type.
	* Give each message an ID of 10 characters:
		* SMS character 1 - “S”.
		* Email character 1 - “E”.
		* Tweet character 1 - “T”.
		* Characters 2-10 are generated numeric characters.
