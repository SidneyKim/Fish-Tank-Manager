using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using UtilityLibrary.Network;

namespace UtilityLibrary.Misc
{
    public static class NisTime
    {
        // Constants
        const int ECHO_PORT = 7;  // The Echo protocol uses port 7 in this sample
        const int QOTD_PORT = 17; // The Quote of the Day (QOTD) protocol uses port 17 in this sample

        public static DateTime GetFastestNISTDate()
        {
            var result = DateTime.MinValue;
            // Initialize the list of NIST time servers
            // http://tf.nist.gov/tf-cgi/servers.cgi
            string[] servers = new string[] { "216.229.0.179", "24.56.178.140", "66.219.116.140", "64.113.32.5", "98.175.203.200",
                                                "129.6.15.30", "128.138.140.44", "128.138.141.172", "198.60.73.8",
                                                "nist.expertsmi.com", "nist.netservicesgroup.com", "nist1-macon.macon.ga.us", "wwv.nist.gov" };

            

            // Try 5 servers in random order to spread the load
            Random rnd = new Random();
            foreach (string server in servers.OrderBy(s => rnd.NextDouble()).Take(5))
            {
                try
                {
                    // Connect to the server (at port 13) and get the response
                    string serverResponse = string.Empty;
                    // Instantiate the SocketClient
                    TcpClient client = new TcpClient();

                    serverResponse = client.Connect(server, 13);

                    serverResponse = client.Receive();


                    // Close the socket connection explicitly
                    client.Close();


                    // If a response was received
                    if (!string.IsNullOrEmpty(serverResponse))
                    {
                        // Split the response string ("55596 11-02-14 13:54:11 00 0 0 478.1 UTC(NIST) *")
                        string[] tokens = serverResponse.Split(' ');

                        // Check the number of tokens
                        if (tokens.Length >= 6)
                        {
                            // Check the health status
                            string health = tokens[5];
                            if (health == "0")
                            {
                                // Get date and time parts from the server response
                                string[] dateParts = tokens[1].Split('-');
                                string[] timeParts = tokens[2].Split(':');

                                // Create a DateTime instance
                                DateTime utcDateTime = new DateTime(
                                    Convert.ToInt32(dateParts[0]) + 2000,
                                    Convert.ToInt32(dateParts[1]), Convert.ToInt32(dateParts[2]),
                                    Convert.ToInt32(timeParts[0]), Convert.ToInt32(timeParts[1]),
                                    Convert.ToInt32(timeParts[2]));

                                // Convert received (UTC) DateTime value to the local timezone
                                //result = utcDateTime.ToLocalTime();

                                result =  TimeZoneInfo.ConvertTime(utcDateTime, TimeZoneInfo.Utc, TimeZoneInfo.FindSystemTimeZoneById("Korea Standard Time"));

                                return result;
                                // Response successfully received; exit the loop

                            }
                        }

                    }

                }
                catch
                {
                    // Ignore exception and try the next server
                }
            }
            return result;
        }
    }
}
