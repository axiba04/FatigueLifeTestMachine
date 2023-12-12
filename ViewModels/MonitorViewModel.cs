using Caliburn.Micro;
using FatigueLifeTestMachine.Base;
using FatigueLifeTestMachine.Models;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FatigueLifeTestMachine.ViewModels
{
    public class MonitorViewModel : Screen
    {
        public override string DisplayName { get { return "监控界面"; } }
        private readonly IEventAggregator EventAggregator;
        public readonly IWindowManager WindowManager;

        private const int MaxSecondsToShow = 5;
        private const int MaxCountToShow = 100;
        private DispatcherTimer? _timer;
        private readonly Random _randomGenerator = new Random();

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

        //监控图
        public PlotModel SensorPlotModel { get; set; }
        public LineSeries Tempreture1_LineSeries { get; set; }

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

        /// <summary>
        /// 初始化监控曲线
        /// </summary>
        public void InitializePlotModel()
        {
            SensorPlotModel = new();


            //X轴
            SensorPlotModel.Axes.Add(new DateTimeAxis
            {
                Title = "时间/s",
                Position = AxisPosition.Bottom,
                StringFormat = "yyyy/MM/dd HH:mm:ss",
                IntervalLength = 60,
                Minimum = DateTimeAxis.ToDouble(DateTime.Now),
                Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddSeconds(MaxSecondsToShow)),
                IsPanEnabled = true,
                IsZoomEnabled = true,
                IntervalType = DateTimeIntervalType.Seconds,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid,
            });

            SensorPlotModel.Axes.Add(new LinearAxis
            {
                Title = "温度/℃",
                Position = AxisPosition.Left,
                IsPanEnabled = true,
                IsZoomEnabled = true,
                Minimum = 0,
                Maximum = 1,
                PositionTier = 0,
                Key = "axis_tempreture"
            });

            SensorPlotModel.Axes.Add(new LinearAxis
            {
                Title = "振动/g",
                Position = AxisPosition.Right,
                IsPanEnabled = true,
                IsZoomEnabled = true,
                Minimum = 0,
                Maximum = 100,
                PositionTier = 1,
                Key = "axis_vibration"
            });

            SensorPlotModel.Axes.Add(new LinearAxis
            {
                Title = "转速/rpm",
                Position = AxisPosition.Right,
                IsPanEnabled = true,
                IsZoomEnabled = true,
                Minimum = 0,
                Maximum = 7000,
                PositionTier = 2,
                Key = "axis_rotatingSpeed"
            });

            SensorPlotModel.Axes.Add(new LinearAxis
            {
                Title = "载荷/kN",
                Position = AxisPosition.Left,
                IsPanEnabled = true,
                IsZoomEnabled = true,
                Minimum = 0,
                Maximum = 100,
                PositionTier = 3,
                Key = "axis_load"
            });

            SensorPlotModel.Axes.Add(new LinearAxis
            {
                Title = "电流/A",
                Position = AxisPosition.Right,
                IsPanEnabled = true,
                IsZoomEnabled = true,
                Minimum = 0,
                Maximum = 60,
                PositionTier = 4,
                Key = "axis_electricCurrent"
            });

            Tempreture1_LineSeries = new LineSeries()
            {
                Title = "1#温度",
                YAxisKey = "axis_tempreture"
            };

            SensorPlotModel.Series.Add(Tempreture1_LineSeries);

            SensorPlotModel.Legends.Add(new Legend() { LegendPlacement = LegendPlacement.Inside, LegendPosition = LegendPosition.RightTop});
        }

        public bool CanStartAcquisition => _timer?.IsEnabled != true;
        public void StartAcquisition()
        {
            if (_timer is null)
            {
                _timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(50),
                };

                _timer.Tick += MockSensorRecievedData;
            }

            _timer.Start();
            NotifyOfPropertyChange(nameof(CanStartAcquisition));
            NotifyOfPropertyChange(nameof(CanStopAcquisition));

        }

        private void MockSensorRecievedData(object? sender, EventArgs e)
        {
            var dateTimeAxis = SensorPlotModel.Axes.OfType<DateTimeAxis>().First();

            if (!Tempreture1_LineSeries.Points.Any())
            {
                dateTimeAxis.Minimum = DateTimeAxis.ToDouble(DateTime.Now);
                dateTimeAxis.Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddSeconds(MaxSecondsToShow));
            }

            if (Tempreture1_LineSeries.Points.Count == MaxCountToShow)
            {
                Tempreture1_LineSeries.Points.RemoveAt(0);
            }

            var tmp = _randomGenerator.NextDouble();
            this.tempreture0.MonitorValue = tmp;

            Tempreture1_LineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now), tmp));

            // if (series.Points.Last().X > dateTimeAxis.Maximum)
            if (DateTimeAxis.ToDateTime(Tempreture1_LineSeries.Points.Last().X) > DateTimeAxis.ToDateTime(dateTimeAxis.Maximum))
            {
                dateTimeAxis.Minimum = DateTimeAxis.ToDouble(DateTime.Now.AddSeconds(-1 * MaxSecondsToShow));
                dateTimeAxis.Maximum = DateTimeAxis.ToDouble(DateTime.Now);
                dateTimeAxis.Reset();
            }

            SensorPlotModel.InvalidatePlot(true);

            NotifyOfPropertyChange(nameof(SensorPlotModel));
        }

        public bool CanStopAcquisition => _timer?.IsEnabled == true;
        public void StopAcquisition()
        {
            _timer?.Stop();
            NotifyOfPropertyChange(nameof(CanStartAcquisition));
            NotifyOfPropertyChange(nameof(CanStopAcquisition));
        }

        public MonitorViewModel(IEventAggregator eventAggregator,IWindowManager windowManager)
        {
            EventAggregator = eventAggregator;
            WindowManager = windowManager;
            InitializeMonitorParameters();
            InitializePlotModel();
            this.EventAggregator.PublishOnUIThreadAsync(MonitorModelList);
        }
    }
}
