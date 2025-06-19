using System.Configuration;
using System.Data;
using System.Windows;
using WpfMrpSimulatorApp.ViewModels;
using WpfMrpSimulatorApp.Views;

namespace WpfMrpSimulatorApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var vm = new MainViewModel();
            var v = new MainView
            {
                DataContext = vm,
            };

            v.ShowDialog();
        }
    }

}
