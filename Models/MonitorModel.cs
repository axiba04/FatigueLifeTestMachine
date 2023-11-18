using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatigueLifeTestMachine.Models
{
    public class MonitorModel
    {
        private string _monitorName;
        public string MonitorName { get => _monitorName; set => _monitorName = value; }

        private double? _monitorValue = null;
        public double? MonitorValue { get => _monitorValue; set => _monitorValue = value; }

        private double? _maxAlarmValue = null;
        public double? MaxAlarmValue { get => _maxAlarmValue; set => _maxAlarmValue = value; }

        private double? _minAlarmValue = null;
        public double? MinAlarmValue { get => _minAlarmValue; set => _minAlarmValue = value; }

        public MonitorModel(string monitorName) { MonitorName = monitorName; }
    }
}
