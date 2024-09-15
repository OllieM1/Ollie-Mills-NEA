First, Thank you for having a look at what I have created. 

**Overview of Completed Features:**
Implemented a live database that logs and stores user data. (More information and tables will be added as development progresses.)
Enabled devices to be unplugged and reconnected without needing to reconfigure them.
Basic user management has been implemented. (This will be significantly expanded in the future.)
Provided a visual indicator to show whether updates have been successfully applied. (This is done by a set of 3 LEDs)
The system can communicate with and manage multiple devices simultaneously.
The system continuously searches for new devices and detects if a device has been disconnected.

**Intended Use of the Code:**
The SQL database is currently set up on a local host. Once the project nears completion, it will be hosted on one of the school's virtual servers.
Each door will have two card readers: one for entry and one for exit. (This setup will be refined later.)
A low-powered Windows-based device will run the C# code and will be able to manage up to five doors per Windows device. (It is possible to do more but it may be unnecessary)
Users will scan their ID cards (which the students already possess), and the system will send a signal to unlock the door locks. (Currently, it triggers an LED indicator.)
After a user scans their card, the system updates the log in the SQL database and changes their status to either "in" or "out."

**Next Steps and Features to Implement:**
Develop a web interface for faculty staff to view the data.
Improve the process for adding students; the current implementation is clunky and can cause issues.
Implement server-side code to reset all users' statuses to "out" at midnight to avoid errors from people not signing out.
Refine the process for adding students to the system.
Further enhance the Arduino code.

**Explanation of Files:**
NEA NV: This is the C# code that connects to and manages all devices, as well as communicates with the database.
ArdCode: This is the Arduino code that operates the RFID scanner and communicates with the C# code.
SQL file: Although slightly outdated, it contains all the necessary components for the current system.
