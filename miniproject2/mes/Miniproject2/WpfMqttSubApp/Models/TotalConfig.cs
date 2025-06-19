using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace WpfMqttSubApp.Models
{
    public class TotalConfig
    {
        public DatabaseConfig dbConf { get; set; }
        public MqttConfig mqttConf {  get; set; }

        public TotalConfig()
        {
            string filePath = "config.json";
            if (!File.Exists(filePath))
            {
                Console.WriteLine("config.json 파일이 존재하지 않습니다.");
                return;
            }
            string json = File.ReadAllText(filePath);
            var fulljson = JsonNode.Parse(json);

            
            // 특정 블록만 추출
            JsonNode configNode = fulljson["Database"];
            if (configNode == null)
            {
                Console.WriteLine($"Key '{"Database"}' not found in config.");
                return;
            }
            dbConf = configNode.Deserialize<DatabaseConfig>();
            Debug.WriteLine($"db  {dbConf.Database}");

            configNode = fulljson["Mqtt"];
            if (configNode == null)
            {
                Console.WriteLine($"Key '{"Mqtt"}' not found in config.");
                return;
            }
            mqttConf = configNode.Deserialize<MqttConfig>();
            Debug.WriteLine($"mqtt broker {mqttConf.broker}");
        }
    }

    public class MqttConfig
    {
        public string broker {  get; set; }
        public string ClientId { get; set; }
        public short Port {  get; set; }
        public string Topic { get; set; }

    }

    public class DatabaseConfig
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string UserId { get; set; }  

        public string Password { get; set; }
    }
}
