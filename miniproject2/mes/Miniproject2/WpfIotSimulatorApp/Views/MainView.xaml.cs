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
using WpfIotSimulatorApp.ViewModels;

namespace WpfIotSimulatorApp.Views
{
    public partial class MainView : MetroWindow
    {
        public bool isEnd;

        public MainView()
        {
            InitializeComponent();
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
                isEnd = false;
                sa.Completed += (async (s, e) =>
                {
                    isEnd = true;
                    if (DataContext is MainViewModel vm)
                    {
                        vm.AnimationCompleted?.Invoke(); // ✅ 여기서 신호를 줘야 WaitFor가 끝남
                    }
                });
                SortingSensor.BeginAnimation(OpacityProperty, sa);

            }));
        }

        // WPF상의 객체 애니메이션 추가
        public void StartHmiAni()
        {
            Product.Fill = new SolidColorBrush(Colors.Gray);

            DoubleAnimation da = new DoubleAnimation
            { 
                From = 0,
                To = 360,
                Duration = TimeSpan.FromSeconds(3)  // Schedules 의 LoadTime 값이 들어와야함
            };

            RotateTransform rt = new RotateTransform();
            GearStart.RenderTransform = rt;
            GearStart.RenderTransformOrigin = new Point(0.5, 0.5);
            GearEnd.RenderTransform = rt;
            GearEnd.RenderTransformOrigin = new Point(0.5, 0.5);

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
