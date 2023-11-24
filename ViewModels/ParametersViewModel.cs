using Caliburn.Micro;
using FatigueLifeTestMachine.Models;
using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FatigueLifeTestMachine.ViewModels
{
    public class ParametersViewModel : Screen, IHandle<List<MonitorModel>>
    {
        public override string DisplayName { get { return "试验参数"; } }
        private TestInformationModel _testInformation = new TestInformationModel();
        public TestInformationModel TestInformation { get {  return _testInformation; } }
        private readonly IEventAggregator EventAggregator;
        public readonly IWindowManager WindowManager;
        public List<MonitorModel> MonitorModels { get; set; }

        private ObservableCollection<WorkStepModel> _preWorkSteps = new ObservableCollection<WorkStepModel>();
        public ObservableCollection<WorkStepModel> PreWorkSteps { get => _preWorkSteps; set { _preWorkSteps = value; NotifyOfPropertyChange(() => PreWorkSteps); } }

        private ObservableCollection<WorkStepModel> _formalWorkSteps = new ObservableCollection<WorkStepModel>();
        public ObservableCollection<WorkStepModel> FormalWorkSteps { get => _formalWorkSteps; set { _formalWorkSteps = value; NotifyOfPropertyChange(() => FormalWorkSteps); } }

        public ParametersViewModel(IEventAggregator eventAggregator, IWindowManager windowManager)
        {
            EventAggregator = eventAggregator;
            WindowManager = windowManager;
            EventAggregator.SubscribeOnPublishedThread(this);
            InitializeWorkSteps();
        }

        /// <summary>
        /// 获取监控参数的所有信息，设置报警值
        /// </summary>
        /// <param name="message">监控参数的集合</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task HandleAsync(List<MonitorModel> message, CancellationToken cancellationToken)
        {
            MonitorModels = message;
            return Task.CompletedTask;
        }

        public void TxtNumberOnly_PreviewTextInput(TextCompositionEventArgs e)
        {
            short val;
            if (!Int16.TryParse(e.Text, out val))
                e.Handled = true;
        }

        public void TextBox_PreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        /// <summary>
        /// 添加工步
        /// </summary>
        /// <param name="workSteps"></param>
        public void AddWorkStep(ObservableCollection<WorkStepModel> workSteps)
        {
            if(workSteps != null)
            {
                workSteps.Add(new WorkStepModel(workSteps.Count + 1));
            }
        }

        /// <summary>
        /// 删除选中工步
        /// </summary>
        /// <param name="workSteps"></param>
        /// <param name="workStep"></param>
        public void DeleteSelectedWorkStep(ObservableCollection<WorkStepModel> workSteps,WorkStepModel workStep)
        {
            if(workSteps!= null && workStep != null && workSteps.Contains(workStep))
            {
                int selectedIndex = workSteps.IndexOf(workStep);
                workSteps.Remove(workStep);
                for (int i = selectedIndex; i < workSteps.Count; i++)
                {
                    workSteps[i].SerialNumber-=1;
                }
            }
        }

        /// <summary>
        /// 初始化预实验工步及正式试验公布
        /// </summary>
        private void InitializeWorkSteps() 
        {
            for (int i = 1; i <= 8; i++)
            {
                this._preWorkSteps.Add(new WorkStepModel(i));
            }

            for (int i = 1; i <= 15; i++)
            {
                this._formalWorkSteps.Add(new WorkStepModel(i));
            }
        }

        /// <summary>
        /// 保存信息到表格文件
        /// </summary>
        public void SaveTestInformation()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "保存试验参数";
            saveFileDialog.AddExtension = true;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.DefaultExt = "xlsx";
            saveFileDialog.FileName = this.TestInformation.TestNumber + "-" + this.TestInformation.Tester;
            saveFileDialog.Filter = "Excel 工作簿(*.xlsx)|*.xlsx";
            bool? result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                string filePath = saveFileDialog.FileName;
                if(File.Exists(filePath) == true)
                {
                    File.Delete(filePath);
                }
                using (var testInformationExcel = new ExcelPackage(@filePath))
                {
                    //生成试验类型及轴承信息表格
                    var testBasicInformationWorkSheet = testInformationExcel.Workbook.Worksheets.Add("试验基本信息");
                    testBasicInformationWorkSheet.Cells["A1"].LoadFromCollection(this.TestInformation.BearingList, true);
                    testBasicInformationWorkSheet.Cells["E1"].LoadFromText("试验编号");
                    testBasicInformationWorkSheet.Cells["E2"].LoadFromText(this.TestInformation.TestNumber);
                    testBasicInformationWorkSheet.Cells["F1"].LoadFromText("试验员");
                    testBasicInformationWorkSheet.Cells["F2"].LoadFromText(this.TestInformation.Tester);
                    testBasicInformationWorkSheet.Cells["G1"].LoadFromText("润滑方式");
                    testBasicInformationWorkSheet.Cells["G2"].LoadFromText(this.TestInformation.TestType);

                    //生成报警信息表格
                    var alarmInformationWorkSheet = testInformationExcel.Workbook.Worksheets.Add("报警参数");
                    alarmInformationWorkSheet.Cells["A1"].LoadFromCollection(this.MonitorModels);
                    testInformationExcel.Save();

                    //生成预实验工步信息表格
                    var preTestWorkStepWorkSheet = testInformationExcel.Workbook.Worksheets.Add("预实验工步参数");
                    preTestWorkStepWorkSheet.Cells["A1"].LoadFromCollection(this.PreWorkSteps);
                    testInformationExcel.Save();

                    //生成正式实验工步信息表格
                    var formalTestWorkStepWorkSheet = testInformationExcel.Workbook.Worksheets.Add("正式试验工步参数");
                    formalTestWorkStepWorkSheet.Cells["A1"].LoadFromCollection(this.FormalWorkSteps);
                    formalTestWorkStepWorkSheet.Cells["G1"].LoadFromText("循环周期");
                    formalTestWorkStepWorkSheet.Cells["G2"].LoadFromText(this.TestInformation.CyclePeriod.ToString());
                    formalTestWorkStepWorkSheet.Cells["H1"].LoadFromText("保存间隔");
                    formalTestWorkStepWorkSheet.Cells["H2"].LoadFromText(this.TestInformation.SaveInterval.ToString());
                    testInformationExcel.Save();
                }
            }
        }
    }
}
