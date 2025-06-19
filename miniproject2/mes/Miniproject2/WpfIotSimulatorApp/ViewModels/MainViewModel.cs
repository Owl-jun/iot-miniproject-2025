using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MQTTnet;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WpfIotSimulatorApp.Models;
using WpfIotSimulatorApp.Views;

namespace WpfIotSimulatorApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private static readonly Brush[] BrushesPool = new Brush[]
        {
            Brushes.Green,
            Brushes.Crimson,
            Brushes.Black
        };
        private static readonly Random rand = new Random();

        [ObservableProperty] private Brush _productBrush;
        [ObservableProperty] private string _logText;

        public MainViewModel()
        {
            brokerHost = "210.119.12.82";
            mqttTopic = "pknu/황달쌤/data";
            clientId = "IoT77";
            LogText = "init";
            logNum = 1;
            InitMqttClient();
        }

        private async Task InitMqttClient()
        {
            var mqttFactory = new MqttClientFactory();
            mqttClient = mqttFactory.CreateMqttClient();

            var mqttClientOptions = new MqttClientOptionsBuilder()
                                        .WithTcpServer(brokerHost, 1883)
                                        .WithClientId(clientId)
                                        .WithCleanSession(true)
                                        .Build();

            mqttClient.ConnectedAsync += async e =>
            {
                LogText = "Connected !";
            };

            await mqttClient.ConnectAsync(mqttClientOptions);
        }

        private IMqttClient mqttClient;
        private string brokerHost;
        private string mqttTopic;
        private string clientId;

        private int logNum;


        public event Action? StartHmiRequested;
        public Action? AnimationCompleted { get; set; }

        //public event Action? StartSensorCheckRequested;

        [RelayCommand]
        private async void Execute()
        {
            ProductBrush = Brushes.Gray;
            int idx = rand.Next(BrushesPool.Length);
            
            StartHmiRequested?.Invoke();

            await WaitForAnimationComplete(); 

            ProductBrush = BrushesPool[idx];

            var resultText = idx == 1 ? "OK" : "FAIL";
            var payload = new CheckResult
            {
                ClientId = clientId,
                Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                //Result = "훠훠훠훠"
                Result = resultText
            };
            var message = new MqttApplicationMessageBuilder()
                  .WithTopic(mqttTopic)
                  .WithPayload(JsonConvert.SerializeObject(payload, Formatting.Indented))
                  .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce)
                  .Build();
            
            await mqttClient.PublishAsync(message);
            LogText = $"Message sended ! {logNum++}";
        }
        private Task WaitForAnimationComplete()
        {
            var tcs = new TaskCompletionSource();
            void Handler()
            {
                AnimationCompleted -= Handler;
                tcs.SetResult();
            }
            AnimationCompleted += Handler;
            return tcs.Task;
        }
    }
}
