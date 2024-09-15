First wanted to say thank you for having a look at what I have created. 

**I have built up the core feature, This includes: **
1. Having a live database that logs and stores user data. (I will be adding more info and tables into it)
2. Being able to have the devices be unpluged and pluged back in without having to re set them up.
3. Being able to add users (This will be heavily built up in future)
4. Being able to give the user a visual indicater of weather the update has happened.
5. The code is able to talk and listen to multipul devices at the same time.
6. It constantly sereches for new devices as well as if a device has been unpluged.

**How I envision the code being used.**
1. The SQL data base is set up as a local host, once nearing completion it should be connected up to one of schools virtual servers as a host.
2. There will be 2 card readers per door. an out going and an ingoing. (This will be refind better in future)
3. A lower powered windows based device will run the c# code this has the ability to host 5 doors.
4. The User will be able to scan there id card (The students allready have them) and this will send a signal to the door locks to unlock (Currently flashes an led).
5. Once a user has been scanned in it will update the log on the SQL data base. It will change there status to either in or out.

**What still needs doing**
1. I want to build up a web interface for the faculty staff to be able to see the data.
2. I want to build up the adding student code. Currently it is clunky and can cause issues.
3. I want to build up a short bit of server code that will reset all the useres back to outside at midnight. This is to make sure there is no errors from people not signing out
4. I want to have a better way of adding student.
5. I will also refine the Arduino code.

6. 
