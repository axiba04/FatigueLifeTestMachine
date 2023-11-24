using Caliburn.Micro;
using FatigueLifeTestMachine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FatigueLifeTestMachine.ViewModels
{
    public class MonitorViewModel : Screen
    {
        public override string DisplayName { get { return "监控界面"; } }
        private readonly IEventAggregator EventAggregator;
        public readonly IWindowManager WindowManager;

        /// <summary>
        /// 初始化所有需要监控的参数
        /// </summary>
        public MonitorModel tempreture0 = new MonitorModel("1#温度(℃)");
        public MonitorModel tempreture1 = new MonitorModel("2#温度(℃)");
        public MonitorModel tempreture2 = new MonitorModel("3#温度(℃)");
        public MonitorModel tempreture3 = new MonitorModel("4#温度(℃)");
        public MonitorModel vibration = new MonitorModel("振动(g)");
        public MonitorModel axisLoad = new MonitorModel("轴载(KN)");
        public MonitorModel radiusLoad = new MonitorModel("径载(KN)");
        public MonitorModel flow0 = new MonitorModel("流量1(L/min)");
        public MonitorModel flow1 = new MonitorModel("流量2(L/min)");
        public MonitorModel flow2 = new MonitorModel("流量3(L/min)");
        public MonitorModel rotatingSpeed = new MonitorModel("转速(RPM)");
        public MonitorModel electricCurrent = new MonitorModel("电流(A)");
        public MonitorModel SpindleTemperature = new MonitorModel("主轴温度(℃)");
        public MonitorModel EnvironmentTemperature = new MonitorModel("环境温度(℃)");

        private List<MonitorModel> _monitorModelList;
        //用于储存监控参数的集合
        public List<MonitorModel> MonitorModelList
        {
            get => _monitorModelList;
        }

        //初始化监控参数的显示
        public void InitializeMonitorParameters()
        {
            _monitorModelList = new List<MonitorModel>
                {
                    tempreture0,
                    tempreture1,
                    tempreture2,
                    tempreture3,
                    vibration,
                    axisLoad,
                    radiusLoad,
                    flow0,
                    flow1,
                    flow2,
                    rotatingSpeed,
                    electricCurrent,
                    SpindleTemperature,
                    EnvironmentTemperature
                };
        }

        public MonitorViewModel(IEventAggregator eventAggregator,IWindowManager windowManager)
        {
            EventAggregator = eventAggregator;
            WindowManager = windowManager;
            InitializeMonitorParameters();
            this.EventAggregator.PublishOnUIThreadAsync(MonitorModelList);

        }
    }
}
