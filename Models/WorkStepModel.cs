using Caliburn.Micro;
using FatigueLifeTestMachine.Base;
using OfficeOpenXml.Attributes;
using OfficeOpenXml.Drawing.Chart;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatigueLifeTestMachine.Models
{
    [EpplusTable(PrintHeaders = true, AutofitColumns = true)]
    public class WorkStepModel : MyPropertyChangedBase
    {
        private double _serialNumber;
        [DisplayName("序号")]
        public double SerialNumber { get => _serialNumber; set { _serialNumber = value;  NotifyOfPropertyChange(() => SerialNumber); } }

        private double? _axialLoad = null;
        [DisplayName("轴向载荷")]
        public double? AxialLoad { get => _axialLoad; set => _axialLoad = value; }

        private double? _radialLoad = null;
        [DisplayName("径向载荷")]
        public double? RadialLoad { get => _radialLoad; set => _radialLoad = value; }

        private double? _rotatingSpeed = null;
        [DisplayName("转速")]
        public double? RotatingSpeed { get => _rotatingSpeed; set => _rotatingSpeed = value; }

        private double? _runningTime = null;
        [DisplayName("运行时间")]
        public double? RunningTime { get => _runningTime; set => _runningTime = value; }

        private bool _isRotationDirectionClockwise = true;
        [DisplayName("是否顺时针旋转")]
        public bool IsRotationDirectionClockwise { get => _isRotationDirectionClockwise; set => _isRotationDirectionClockwise = value; }

        public WorkStepModel(double serialNumber) { 
            SerialNumber = serialNumber;
        }
    }
}
