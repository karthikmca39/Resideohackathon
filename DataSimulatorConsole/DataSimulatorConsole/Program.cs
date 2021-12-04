using System;
using Microsoft.Azure.Devices.Client;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace DataSimulatorConsole
{
    public class Program
    {
        private static DeviceClient s_deviceClient;
        public static int occupancy = 1;
        public static string armState = "disarm";
        //Provide your hub connection string
        private readonly static string s_connectionString01 = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
        static void Main(string[] args)
        {
            s_deviceClient = DeviceClient.CreateFromConnectionString(s_connectionString01, TransportType.Mqtt);
            SendDeviceToCloudMessagesAsync(s_deviceClient);
            Console.ReadLine();

        }

        private static async void SendDeviceToCloudMessagesAsync(DeviceClient s_deviceClient)
        {
            try
            {
                double minTemperature = 20;
                double minHumidity = 60;
                Random rand = new Random();

                while (true)
                {
                    double currentTemperature = minTemperature + rand.NextDouble() * 15;
                    double currentHumidity = minHumidity + rand.NextDouble() * 20;
                    // int occupancy = rand.Next(0, 2);
                    //int power = rand.Next(500, 1550);
                    // Create JSON message

                    Powerdata powerData = new Powerdata();
                    powerData.Temperature = currentTemperature;
                    powerData.Humidity = currentHumidity;
                    powerData.Occupancy = occupancy;
                    powerData.ArmState = armState;
                    powerData.Power = getPower();
                    powerData.reportedAt = DateTime.Now;
                    powerData.deviceId = "CT_SensorS";
                    powerData.locationId = "12345";

                    string messageString = "";

                    messageString = JsonConvert.SerializeObject(powerData);

                    var message = new Message(Encoding.ASCII.GetBytes(messageString));

                    // Add a custom application property to the message.
                    // An IoT hub can filter on these properties without access to the message body.
                    //message.Properties.Add("temperatureAlert", (currentTemperature > 30) ? "true" : "false");

                    // Send the telemetry message
                    await s_deviceClient.SendEventAsync(message);
                    Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);
                    await Task.Delay(1000);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static double getPower()
        {
            /*Current calculations: 
             * Total 1 average month current = 250Khwh 
             * 30 Watts bulb = 7 (30w) 1+18+5+5+1= 30 * 30 = 900w(1day)
             * 1.5 Ton AC = 1 (1700w) 1*1700 = 1700w
             * FAN = 3 (70W) 24 * 70w = 1680w
             * TV = 1 (150w) 18 * 150 = 2700w
             * Washing machine = 1 (2250w)            
            */
            Random rnd = new Random();

            double F = 0.020;
            double L = 0.008;
            double T = 0.041;
            double W = 0.625;
            double AC = 0.472;
            double power = rnd.Next(0, 10) * 0.001;
            occupancy = 1;
            var day = Convert.ToDateTime(DateTime.Now.ToString("yyyy/M/dd ")).ToString("dddd");
            var hour = Convert.ToInt32(Convert.ToDateTime(DateTime.Now.ToString("yyyy/M/dd hh:mm:ss")).ToString("HH"));
            switch (day)
            {
                case "Sunday":
                    if (hour >= 0 && hour <= 6) //F
                    { power += F; }
                    else if (hour >= 6 && hour <= 7)//F L
                    { power += F + L; }
                    else if (hour >= 7 && hour <= 8) // L
                    { power = L; occupancy = 0; }
                    else if (hour >= 8 && hour <= 9) //F T W
                    { power += F + T + W; }
                    else if (hour >= 9 && hour <= 10) //F T W AC 
                    { power += F + T + W + AC; }
                    else if (hour >= 10 && hour <= 12) // F T L
                    { power += F + T + L; }
                    else if (hour >= 12 && hour <= 13) // F T
                    { power += F + T; occupancy = 0; }
                    else if (hour >= 13 && hour <= 14) // F T AC
                    { power += F + T + AC; }
                    else if (hour >= 14 && hour <= 15) // F T
                    { power += F + T; }
                    else if (hour >= 15 && hour <= 16) // F T
                    { power += F + T; }
                    else if (hour >= 16 && hour <= 18) // T
                    { power = T; occupancy = 0; }
                    else if (hour >= 18 && hour <= 20) // F T 3L
                    { power += F + T + (3 * L); }
                    else if (hour >= 20 && hour <= 21) // F T AC 3L
                    { power += F + T + AC + (3 * L); }
                    else if (hour >= 21 && hour <= 22) // F T AC 2L
                    { power += F + T + AC + (2 * L); }
                    else if (hour >= 22 && hour <= 23) // F 
                    { power += F; }
                    break;
                case "Monday":
                    if (hour >= 0 && hour <= 6) //F
                    { power += F; }
                    else if (hour >= 6 && hour <= 7)//F L
                    { power += F + L; }
                    else if (hour >= 7 && hour <= 8) // L
                    { power = L; occupancy = 1; }
                    else if (hour >= 8 && hour <= 9) //F T W
                    { power += F + T + W; }
                    else if (hour >= 9 && hour <= 10) //F T W 
                    { power += F + T + W + AC; }
                    else if (hour >= 10 && hour <= 12) // F T L
                    { power += F + T + L; }
                    else if (hour >= 12 && hour <= 13) // F T
                    { power += F + T; occupancy = 0; }
                    else if (hour >= 13 && hour <= 14) // F T AC
                    { power += F + T + AC; }
                    else if (hour >= 14 && hour <= 15) // F T
                    { power += F + T; occupancy = 0; }
                    else if (hour >= 15 && hour <= 16) // F T
                    { power += F + T; }
                    else if (hour >= 16 && hour <= 18) // T
                    { power = T; occupancy = 0; }
                    else if (hour >= 18 && hour <= 20) // F T 3L
                    { power += F + T + (3 * L); }
                    else if (hour >= 20 && hour <= 21) // F T 3L
                    { power += F + T + AC + (3 * L); occupancy = 0; }
                    else if (hour >= 21 && hour <= 22) // F T AC 2L
                    { power += F + T + AC + (2 * L); }
                    else if (hour >= 22 && hour <= 23) // F 
                    { power += F; }
                    break;
                case "Tuesday":
                    if (hour >= 0 && hour <= 6) //F
                    { power += F; }
                    else if (hour >= 6 && hour <= 7)//F L
                    { power += F + L; }
                    else if (hour >= 7 && hour <= 8) // L
                    { power = L; occupancy = 1; }
                    else if (hour >= 8 && hour <= 9) //F T W
                    { power += F + T + W; }
                    else if (hour >= 9 && hour <= 10) //F T AC 
                    { power += F + T + AC; }
                    else if (hour >= 10 && hour <= 12) // F T L
                    { power += F + T + L; }
                    else if (hour >= 12 && hour <= 13) // F T
                    { power += F + T; occupancy = 0; }
                    else if (hour >= 13 && hour <= 14) // F T AC
                    { power += F + T + AC; }
                    else if (hour >= 14 && hour <= 15) // F T
                    { power += F + T; }
                    else if (hour >= 15 && hour <= 16) // F T
                    { power += F + T; }
                    else if (hour >= 16 && hour <= 18) // T
                    { power = T; occupancy = 0; }
                    else if (hour >= 18 && hour <= 20) // F T 3L
                    { power += F + T + (3 * L); }
                    else if (hour >= 20 && hour <= 21) // F T AC 3L
                    { power += F + T + AC + (3 * L); }
                    else if (hour >= 21 && hour <= 22) // F T AC 2L
                    { power += F + T + AC + (2 * L); }
                    else if (hour >= 22 && hour <= 23) // F 
                    { power += F; }
                    break;
                case "Wednesday":
                    if (hour >= 0 && hour <= 6) //F
                    { power += F; }
                    else if (hour >= 6 && hour <= 7)//F L
                    { power += F + L; }
                    else if (hour >= 7 && hour <= 8) // L
                    { power = L; occupancy = 0; }
                    else if (hour >= 8 && hour <= 9) //F T 
                    { power += F + T; }
                    else if (hour >= 9 && hour <= 10) //F T AC 
                    { power += F + T; }
                    else if (hour >= 10 && hour <= 12) // F T L
                    { power += F + T + L; }
                    else if (hour >= 12 && hour <= 13) // F T
                    { power += F + T; occupancy = 1; }
                    else if (hour >= 13 && hour <= 14) // F T AC
                    { power += F + T + AC; }
                    else if (hour >= 14 && hour <= 15) // F T
                    { power += F + T; }
                    else if (hour >= 15 && hour <= 16) // F T
                    { power += F + T; }
                    else if (hour >= 16 && hour <= 18) // T
                    { power = T; occupancy = 0; }
                    else if (hour >= 18 && hour <= 20) // F T 3L
                    { power += F + T + (3 * L); occupancy = 0; }
                    else if (hour >= 20 && hour <= 21) // F T AC 3L
                    { power += F + T + AC + (3 * L); }
                    else if (hour >= 21 && hour <= 22) // F T AC 2L
                    { power += F + T + AC + (2 * L); }
                    else if (hour >= 22 && hour <= 23) // F 
                    { power += F; }
                    break;
                case "Thursday":
                    if (hour >= 0 && hour <= 6) //F
                    { power += F; }
                    else if (hour >= 6 && hour <= 7)//F L
                    { power += F + L; }
                    else if (hour >= 7 && hour <= 8) // L
                    { power = L; occupancy = 0; }
                    else if (hour >= 8 && hour <= 9) //F T W
                    { power += F + T + W; }
                    else if (hour >= 9 && hour <= 10) //F T W AC 
                    { power += F + T + W + AC; }
                    else if (hour >= 10 && hour <= 12) // F T L
                    { power += F + T + L; }
                    else if (hour >= 12 && hour <= 13) // F T
                    { power += F + T; occupancy = 0; }
                    else if (hour >= 13 && hour <= 14) // F T AC
                    { power += F + T + AC; }
                    else if (hour >= 14 && hour <= 15) // F T
                    { power += F + T; }
                    else if (hour >= 15 && hour <= 16) // F T
                    { power += F + T; }
                    else if (hour >= 16 && hour <= 18) // T
                    { power = T; occupancy = 0; }
                    else if (hour >= 18 && hour <= 20) // F T 3L
                    { power += F + T + (3 * L); }
                    else if (hour >= 20 && hour <= 21) // F T AC 3L
                    { power += F + T + AC + (3 * L); }
                    else if (hour >= 21 && hour <= 22) // F T AC 2L
                    { power += F + T + AC + (2 * L); }
                    else if (hour >= 22 && hour <= 23) // F 
                    { power += F; }
                    break;
                case "Friday":
                    if (hour >= 0 && hour <= 6) //F
                    { power += F; }
                    else if (hour >= 6 && hour <= 7)//F L
                    { power += F + L; }
                    else if (hour >= 7 && hour <= 8) // L
                    { power = L; occupancy = 1; }
                    else if (hour >= 8 && hour <= 9) //F T W
                    { power += F + T + W; }
                    else if (hour >= 9 && hour <= 10) //F T W AC 
                    { power += F + T + W + AC; }
                    else if (hour >= 10 && hour <= 12) // F T L
                    { power += F + T + L; }
                    else if (hour >= 12 && hour <= 13) // F T
                    { power += F + T; occupancy = 0; }
                    else if (hour >= 13 && hour <= 14) // F T AC
                    { power += F + T + AC; }
                    else if (hour >= 14 && hour <= 15) // F T
                    { power += F + T; occupancy = 0; }
                    else if (hour >= 15 && hour <= 16) // F T
                    { power += F + T; }
                    else if (hour >= 16 && hour <= 18) // T
                    { power = T; occupancy = 0; }
                    else if (hour >= 18 && hour <= 20) // F T 3L
                    { power += F + T + (3 * L); }
                    else if (hour >= 20 && hour <= 21) // F T AC 3L
                    { power += F + T + AC + (3 * L); occupancy = 0; }
                    else if (hour >= 21 && hour <= 22) // F T AC 2L
                    { power += F + T + AC + (2 * L); }
                    else if (hour >= 22 && hour <= 23) // F 
                    { power += F; }
                    break;
                case "Saturday":
                    if (hour >= 0 && hour <= 6) //F
                    { power += F; }
                    else if (hour >= 6 && hour <= 7)//F L
                    { power += F + L; }
                    else if (hour >= 7 && hour <= 8) // L
                    { power = L; occupancy = 0; }
                    else if (hour >= 8 && hour <= 9) //F T
                    { power += F + T; }
                    else if (hour >= 9 && hour <= 10) //F T AC 
                    { power += F + T; }
                    else if (hour >= 10 && hour <= 12) // F T L
                    { power += F + T + L; }
                    else if (hour >= 12 && hour <= 13) // F T
                    { power += F + T; occupancy = 1; }
                    else if (hour >= 13 && hour <= 14) // F T AC
                    { power += F + T + AC; occupancy = 1; }
                    else if (hour >= 14 && hour <= 15) // F T
                    { power += F + T; }
                    else if (hour >= 15 && hour <= 16) // F T
                    { power += F + T; }
                    else if (hour >= 16 && hour <= 18) // T
                    { power = T; occupancy = 0; }
                    else if (hour >= 18 && hour <= 20) // F T 3L
                    { power += F + T + (3 * L); }
                    else if (hour >= 20 && hour <= 21) // F T 3L
                    { power += F + T + (3 * L); }
                    else if (hour >= 21 && hour <= 22) // F T AC 2L
                    { power += F + T + AC + (2 * L); }
                    else if (hour >= 22 && hour <= 23) // F 
                    { power += F; }
                    break;
            }

            return power;
        }
    }

    public class Powerdata
    {
        public string locationId { get; set; }
        public string deviceId { get; set; }
        public DateTime reportedAt { get; set; }
        public double Power { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public int Occupancy { get; set; }
        public string ArmState { get; set; }
    }
}