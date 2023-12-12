using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatigueLifeTestMachine.ViewModels
{
    internal class MainViewModel : Conductor<IScreen>.Collection.OneActive
    {
        public MainViewModel(MonitorViewModel monitorViewModel,ParametersViewModel parametersViewModel,AlarmMessageViewModel alarmMessageViewModel) { 
            Items.Add(monitorViewModel);
            Items.Add(parametersViewModel);
            Items.Add(alarmMessageViewModel);
            ActiveItem = monitorViewModel;
        }
    }
}
