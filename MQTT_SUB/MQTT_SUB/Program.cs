using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MQTT_SUB
{
    class Program
    {
        public class Content
        {
            public IList<string> format { get; set; }
            public IList<IList<string>> value { get; set; }
        }

        public class RootObject
        {
            public Content content { get; set; }
            public string type { get; set; }
            public string uuid { get; set; }
        }

        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                var factory = new MqttFactory();
                var mqttClient = factory.CreateMqttClient();
                var options = new MqttClientOptionsBuilder()
                .WithClientId("Bedir")
                .WithTcpServer("192.168.1.102", 1883)
                .WithCredentials(null, null)
                .WithCleanSession()
                .Build();


                mqttClient.Connected += async (s, e) =>
                {
                    Console.WriteLine("### CONNECTED WITH SERVER ###");

                    // Subscribe to a topic
                    await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("workstation/4845445957535d0002203500454e0237/up").Build());

                    Console.WriteLine("### SUBSCRIBED ###");
                };

                await mqttClient.ConnectAsync(options);


                mqttClient.ApplicationMessageReceived += (s, e) =>
                {
                    FileStream fStream = new FileStream("data.txt", FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter sWriter = new StreamWriter(fStream);
                    sWriter.AutoFlush = true;


                    Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
                    Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
                    Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                    Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                    Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
                    Console.WriteLine();

                    try
                    {
                        string jsonData = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                        var RootObject = JsonConvert.DeserializeObject<RootObject>(jsonData);
                        int value_length = RootObject.content.value.Count;
                        for (var i = 0; i < value_length; i++)
                        {
                            for (var j = 2; j < 6; j++)
                            {
                                Console.WriteLine((RootObject.content.format[j] + ":" + RootObject.content.value[i][j]));
                                sWriter.WriteLine(RootObject.content.format[j] + ":" + RootObject.content.value[i][j]);
                            }
                            Console.WriteLine();
                        }
                        sWriter.Close();
                    }
                    catch (Exception E)
                    {
                        Console.WriteLine("ERROR MESSAGE ==>{0}", E);
                    }
                };

                Console.ReadLine();
            }).GetAwaiter().GetResult();
        }
    }
}
