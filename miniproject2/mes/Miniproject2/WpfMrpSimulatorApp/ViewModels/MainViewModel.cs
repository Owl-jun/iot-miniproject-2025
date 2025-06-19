using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMrpSimulatorApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty] private string _greeting;

        public MainViewModel()
        {
            Greeting = "우동킹갓제너럴";
        }
    }
}
