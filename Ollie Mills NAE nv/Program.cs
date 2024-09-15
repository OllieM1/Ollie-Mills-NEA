using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;

namespace Ollie_Mills_NAE_nv
{
    internal class Program
    {
        // Class that defines all the connected Arduinos
        public class CONNECTEDDEVICES
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public SerialPort Port { get; set; }

            public override string ToString()
            {
                return $"ID: {ID}, Name: {Name}, Port: {Port?.PortName ?? "Not connected"}";
            }
        }

        // Class that is for the Communication Logs, these are for Error Debugging
        public class DEVICELOG
        {
            public string ID { get; set; }
            public int count { get; set; }
            public bool Ingoing { get; set; }
            public string Communication { get; set; }

            // Override ToString to format the data properly
            public override string ToString()
            {
                return $"{ID}\t{(Ingoing ? "Yes" : "No")}\t{Communication}";
            }
        }


        // Public variables that need to be accessed program-wide
        public static List<CONNECTEDDEVICES> ConnectedDevices = new List<CONNECTEDDEVICES>();
        public static List<DEVICELOG> DeviceLog = new List<DEVICELOG>();
        public static bool print = false; // This is for Debugging
        public static int checkPortsList = 10; // How many ports It will check
        private static MySqlConnection connection; // To connect to the database
        private static readonly object portLock = new object(); // Lock for synchronizing port access

        public static async Task Main()
        {
            ConnectToDataBase(); // Connecting to the Database
            if (!Startup())
            {
                throw new Exception("Did not start content to beginning Program");
            }
            var monitorTask = Task.Run(() => MonitorComPorts()); // Using Async to Constantly check the ports if a device is connected or not.
            var checkMessagesTask = Task.Run(() => CheckForMessages()); // Using Async to Constantly check for a message from the Connected Devices            
            Console.WriteLine("Type Commands here");
            while (true)
            {
                CheckCommand(); // This is to make changes from a front end
                await Task.Delay(100);
            }
        }   //This Boots everything up
        static void CheckCommand()
        {
            switch (Console.ReadLine())
            {
                case ("SD"):
                    {
                        Console.WriteLine("Showing Devices");
                        foreach (var device in ConnectedDevices)
                        {
                            Console.WriteLine($"{device}"); // Changed to show proper device data
                        }
                        Console.WriteLine("Done Showing Devices");
                        break;
                    }

                case ("SC"):
                    {
                        Console.WriteLine("Showing past 20 logs");
                        int logCount = DeviceLog.Count;
                        int logsToShow = Math.Min(20, logCount); // Show up to 20 logs or less if there are fewer entries.

                        for (int i = logCount - logsToShow; i < logCount; i++)
                        {
                            var log = DeviceLog[i];
                            Console.WriteLine($"ID: {log.ID}, Count: {log.count}, Ingoing: {log.Ingoing}, Communication: {log.Communication}");
                        }

                        Console.WriteLine("Done showing logs");
                        break;
                    }

                case ("PRINT"):
                    {
                        if (print == true)
                        {
                            print = false;
                        }
                        else
                        {
                            print = true;
                        }
                        break;
                    }
                case ("CHANGENUM"):
                    {
                        Console.WriteLine("What do you want it changed to");
                        checkPortsList = int.Parse(Console.ReadLine());

                        break;
                    }
                case ("ADD"):
                    {
                        AddUserDataBase();
                        break;
                    }
                case ("CLOSE"):
                    {
                        Console.WriteLine("Are you sure y/n");
                        if (Console.ReadLine().ToLower() == "y")
                        {
                            OnProcessExit();
                        }
                        break;
                    }

                default:
                    {
                        Console.WriteLine("Command not recognized");
                        break;
                    }
            }
        }
        // This is the startup message
        static bool Startup()
        {
            Console.WriteLine();
            Console.WriteLine("******************************");
            Console.WriteLine("*        Welcome to          *");
            Console.WriteLine("*        Sign In Manager     *");
            Console.WriteLine("*                            *");
            Console.WriteLine("*        Press Y to begin    *");
            Console.WriteLine("*                            *");
            Console.WriteLine("******************************");

            if (Console.ReadLine().ToLower() == "y")
            {
                Console.WriteLine("******************************");
                Console.WriteLine("*                            *");
                Console.WriteLine("*        Thank you           *");
                Console.WriteLine("*                            *");
                Console.WriteLine("*        Have a nice day     *");
                Console.WriteLine("*                            *");
                Console.WriteLine("******************************");
                return true;
            }
            else
            {
                return false;
            }
        }
        // This is called upon exiting to safely close all connections and export logs
        static void OnProcessExit()
        {
            for (int i = ConnectedDevices.Count - 1; i >= 0; i--)
            {
                var device = ConnectedDevices[i];
                if (device.Port.IsOpen)
                {
                    device.Port.Close();
                    Print($"Device {device.ID} disconnected from {device.Port.PortName}");
                    ConnectedDevices.RemoveAt(i);
                }
            }

            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
                Print("Database connection closed.");
            }
            cmdExport();
            Environment.Exit(0);
        }

        //Next two Functions are Asynchronous 

        // This monitors the COM ports to check if devices are connected or disconnected
        static async Task MonitorComPorts()
        {
            while (true)
            {
                string[] availablePorts = SerialPort.GetPortNames();

                for (int i = 1; i <= checkPortsList; i++)
                {
                    string portName = $"COM{i}";
                    int deviceId = i;

                    if (!availablePorts.Contains(portName))
                    {
                        Print($"Skipping {portName}, not available.");
                    }
                    else if (ConnectedDevices.Exists(d => d.Port.PortName == portName))
                    {
                        Print($"Skipping {portName}, already connected.");
                    }
                    else
                    {
                        try
                        {
                            SerialPort port = new SerialPort(portName, 9600);
                            port.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                            port.Open();

                            CONNECTEDDEVICES newDevice = new CONNECTEDDEVICES
                            {
                                ID = deviceId,
                                Name = $"Device {deviceId}",
                                Port = port
                            };

                            ConnectedDevices.Add(newDevice);
                            Print($"Successfully connected to {portName}");
                        }
                        catch (Exception ex)
                        {
                            Print($"Unsuccessful in connecting to {portName}: {ex.Message}");
                        }
                    }

                }

                for (int i = ConnectedDevices.Count - 1; i >= 0; i--)
                {
                    var device = ConnectedDevices[i];
                    if (!device.Port.IsOpen)
                    {
                        Console.WriteLine($"Device {device.ID} disconnected from {device.Port.PortName}");
                        DeviceLog.Add(new DEVICELOG
                        {
                            ID = device.ID.ToString(),
                            count = DeviceLog.Count + 1,
                            Ingoing = false,
                            Communication = "Disconnected"
                        });
                        ConnectedDevices.RemoveAt(i);
                    }
                }

                await Task.Delay(5000);
            }
        }

        // This checks messages received from connected devices
        static async Task CheckForMessages()
        {
            while (true)
            {
                Print("Checking for Messages");
                foreach (var device in ConnectedDevices)
                {
                    if (print)
                    {
                        Console.WriteLine(device);
                    }
                    if (device.Port != null && device.Port.IsOpen)
                    {
                        Print("Reading...");
                        try
                        {
                            if (print)
                            {
                                Console.WriteLine(device.Port.BytesToRead);
                            }
                            if (device.Port.BytesToRead > 0)
                            {
                                string data = device.Port.ReadLine();
                                Print($"Message received from {device.Port.PortName}: {data}");
                                HandleReceivedData(device.Port.PortName, data);
                            }
                        }
                        catch (Exception ex)
                        {

                            Print($"Failed to read from COM{device.ID}: {ex.Message}");

                        }
                    }
                }

                await Task.Delay(1000);
            }
        }

        // This function handles sending messages to a specific device
        static void SendMessageToDevice(int DeviceID, string message)
        {
            var device = ConnectedDevices.Find(d => d.ID == DeviceID); //FROM CHATGPT
            if (device != null && device.Port != null && device.Port.IsOpen)
            {
                try
                {
                    lock (portLock) // Synchronize port access with a lock //FROM CHATGPT
                    {
                        device.Port.WriteLine(message);
                    }
                    DEVICELOG newLog = new DEVICELOG
                    {
                        ID = DeviceID.ToString(),
                        count = DeviceLog.Count + 1,
                        Ingoing = false,
                        Communication = message
                    };
                    DeviceLog.Add(newLog); // Log sent message

                    Print($"Message sent to COM{DeviceID}: {message}");


                }
                catch (Exception ex)
                {
                    Print($"Failed to send message to COM{DeviceID}: {ex.Message}");
                }
            }
            else
            {
                Print($"Cannot send message. Device {DeviceID} is not connected.");
            }
            Print(message);
        }

        // Handles received data, processes it, and logs it
        private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string inData = sp.ReadLine();

            Print($"Data Received from {sp.PortName}: {inData}");

            DEVICELOG newLog = new DEVICELOG
            {
                ID = sp.PortName,
                count = DeviceLog.Count + 1,
                Ingoing = true,
                Communication = inData
            };
            DeviceLog.Add(newLog);
            HandleReceivedData(sp.PortName, inData);
        }

        // Connect to the database
        static void ConnectToDataBase()
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=school_sign_in_data;";
            connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                Console.WriteLine("Successfully connected to the database.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        // Export the logs to a text file
        static void cmdExport()
        {
            string date = DateTime.Now.ToString("ddMMyyyy");
            string fileName = $"EXPORTDATA_{date}.txt";
            using (StreamWriter writer = new StreamWriter(fileName))
            {                
                writer.WriteLine("ID\tIngoing\tCommunication");                
                foreach (var DATA in DeviceLog) 
                {
                    writer.WriteLine(DATA.ToString());
                }
            }
        }

        // Adds a User to the DataBase
        static void AddUserDataBase()
        {
            Console.WriteLine("Scan the ID (Make sure the scan works the RED LED will flash) => ");
            Console.WriteLine("Enter Users First Name =>");
            string FirstName = Console.ReadLine();

            Console.WriteLine("Enter Users Second Name =>");
            string SecondName = Console.ReadLine();

            string CardNum = FindCardNum();
            MySqlCommand cmdImput = connection.CreateCommand();
            cmdImput.CommandText = $"INSERT INTO `user`( `firstName`, `secondName`, `CardNum`) VALUES " +
                                                    $"('{FirstName}','{SecondName}','{CardNum}')";
            cmdImput.ExecuteNonQuery();
        }
        // This Finds the CardNumber of the user
        static string FindCardNum()
        {
            string output = GetLastIngoingCommunication();
            output = output.Substring(6).Trim();
            return output;
        }
        public static string GetLastIngoingCommunication()
        {
            // From GitHub but with iterations
            var lastIngoingLog = DeviceLog.LastOrDefault(log => log.Ingoing == true);
            return lastIngoingLog != null ? lastIngoingLog.Communication : "No Ingoing logs found";
        }

        // This handles the received data and checks it against the database
        static string GetCurrentDate()
        {
            DateTime today = DateTime.Now;
            return today.ToString("yyyy-MM-dd");
        }
        static string GetCurrentTime()
        {
            DateTime now = DateTime.Now;
            return now.ToString("HH:mm:ss");
        }
        // Handles and Processes Data that has been Recived
        private static void HandleReceivedData(string portName, string data)
        {
            bool TorF = false;
            if (true)
            {
                DEVICELOG newLog = new DEVICELOG
                {
                    ID = portName,
                    count = DeviceLog.Count + 1,
                    Ingoing = true
                };
            }

            string id = "";

            Print($"Processing data from {portName}: {data}");
            Print(data.Substring(4, 1));


            try
            {
                switch (data.Substring(4, 1))
                {
                    case "1":
                        Print("found alive");
                        TorF = true;
                        break;

                    case "2":
                        if (print)
                        {
                            Console.WriteLine();
                            Print("Testing Against Database");
                        }

                        MySqlCommand cmdCheck = connection.CreateCommand();
                        string cardNumQuery = $"'{data.Substring(6).Trim()}' ";
                        string query = $"SELECT * FROM `user` WHERE CardNum = {cardNumQuery}";
                        Print(query);
                        cmdCheck.CommandText = query;
                        MySqlDataReader mySqlDataReader = cmdCheck.ExecuteReader();
                        Print("Reading DATA");

                        if (mySqlDataReader.Read())
                        {                          

                            id = mySqlDataReader["ID"].ToString();
                            string firstName = mySqlDataReader["firstName"].ToString();
                            string secondName = mySqlDataReader["secondName"].ToString();
                            string cardNum = mySqlDataReader["CardNum"].ToString();
                            Print($"{id}, {firstName}, {secondName}, {cardNum}");

                            while (mySqlDataReader.Read())
                            {
                                Print("In While");
                                id = mySqlDataReader["ID"].ToString();
                                firstName = mySqlDataReader["firstName"].ToString();
                                secondName = mySqlDataReader["secondName"].ToString();
                                cardNum = mySqlDataReader["CardNum"].ToString();
                                Print($"{id}, {firstName}, {secondName}, {cardNum}");
                            }
                            TorF = true;
                        }
                        else
                        {
                            Print("No data found");
                        }
                        mySqlDataReader.Close();
                        if (!string.IsNullOrEmpty(id))
                        {
                            if (print)
                            {

                                Print("Updating database");
                                Console.WriteLine(GetCurrentDate());
                                Print(GetCurrentTime());
                                Console.WriteLine();
                            }

                            MySqlCommand cmdImput = connection.CreateCommand();
                            cmdImput.CommandText = $"INSERT INTO timeofscan (DayOfScan, TimeOfScan, UserID) " +
                                                   $"VALUES ('{GetCurrentDate()}', '{GetCurrentTime()}', '{id}');";
                            cmdImput.ExecuteNonQuery();

                            DEVICELOG logEntry = new DEVICELOG
                            {
                                ID = portName,
                                count = DeviceLog.Count + 1,
                                Ingoing = false,
                                Communication = $"INSERT INTO timeofscan (Count, DayOfScan, TimeOfScan, UserID) VALUES (NULL, '{GetCurrentDate()}', '{GetCurrentTime()}', '{id}');"
                            };
                            DeviceLog.Add(logEntry);
                            TorF = true;
                        }
                        else
                        {
                            // If no user was found, send a negative response
                            TorF = false;
                            Print("No user found in Data base");
                        }
                        break;

                    default:
                        if (print)
                        {
                            // Log the unknown data for debugging
                            Console.WriteLine();
                            Console.WriteLine("Unknown Data");
                            Console.WriteLine();
                            TorF = false;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Print($"There was an error: {ex.Message}");
            }



            SendMessageToDevice(int.Parse(portName.Substring(3, 1)), TorF.ToString());
        }

        // If print is ennabled then it will print the message
        static void Print(string message)
        {
            if (print)
            {
                Console.WriteLine(message);
            }
        }
    }
}