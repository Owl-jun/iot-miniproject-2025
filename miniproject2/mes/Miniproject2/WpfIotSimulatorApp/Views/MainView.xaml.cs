using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfIotSimulatorApp.Views
{
    /// <summary>
    /// MainView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainView : MetroWindow
    {
        public MainView()
        {
            InitializeComponent();
        }
        //Timer timer = new Timer();
        Stopwatch sw = new Stopwatch();

        private void BtnTest_Click(object sender, RoutedEventArgs e)
        {
            StartHmiAni(); // Hmi 애니메이션 동작

        }

        private void StartSensorCheck()
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                DoubleAnimation sa = new DoubleAnimation()
                {
                    From = 1,
                    To = 0,
                    Duration = TimeSpan.FromSeconds(1),
                    AutoReverse = true
                };
                sa.Completed += (s, e) =>
                {
                    Random rand = new Random();
                    int result = rand.Next(3);
                    List<SolidColorBrush> colors = new List<SolidColorBrush>();
                    colors.Add(new SolidColorBrush(Colors.Green));
                    colors.Add(new SolidColorBrush(Colors.Crimson));
                    colors.Add(new SolidColorBrush(Colors.Black));
                    Product.Fill = colors.ToArray()[result];
                };
                SortingSensor.BeginAnimation(OpacityProperty, sa);

            }));
        }

        // WPF상의 객체 애니메이션 추가
        private void StartHmiAni()
        {
            Product.Fill = new SolidColorBrush(Colors.Gray);

            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 360;
            da.Duration = TimeSpan.FromSeconds(3);  // Schedules 의 LoadTime 값이 들어와야함
            RotateTransform rt = new RotateTransform();
            GearStart.RenderTransform = rt;
            GearStart.RenderTransformOrigin = new Point(0.5, 0.5);
            GearEnd.RenderTransform = rt;
            GearEnd.RenderTransformOrigin = new Point(0.5, 0.5);

            // Window IOCP 콜백기반의 비동기 작업.
            da.Completed += (s, e) =>
            {
                StartSensorCheck(); // 애니메이션 끝난 후 실행
            };

            rt.BeginAnimation(RotateTransform.AngleProperty, da);

            DoubleAnimation pa = new DoubleAnimation
            {
                From = 142,
                To = 398,
                Duration = TimeSpan.FromSeconds(3)
            };
            Product.BeginAnimation(Canvas.LeftProperty,pa);

        }
    }
}
