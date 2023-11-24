using Caliburn.Micro;
using FatigueLifeTestMachine.Base;
using OfficeOpenXml.Attributes;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatigueLifeTestMachine.Models
{
    [EpplusTable(PrintHeaders = true, AutofitColumns = true)]
    public class MonitorModel : MyPropertyChangedBase
    {
        private string _monitorName;
        [DisplayName("报警项")]
        public string MonitorName { get => _monitorName; set => _monitorName = value; }

        private double? _monitorValue = null;
        [EpplusIgnore]
        public double? MonitorValue { get => _monitorValue; set => _monitorValue = value; }

        private double? _minAlarmValue = null;
        [DisplayName("最小报警值")]
        public double? MinAlarmValue { get => _minAlarmValue; set => _minAlarmValue = value; }

        private double? _maxAlarmValue = null;
        [DisplayName("最大报警值")]
        public double? MaxAlarmValue { get => _maxAlarmValue; set => _maxAlarmValue = value; }

        private bool _isAlarm = true;
        [DisplayName("是否报警")]
        public bool IsAlarm { get => _isAlarm; set => _isAlarm = value; }

        public MonitorModel(string monitorName) { MonitorName = monitorName; }
    }
}
