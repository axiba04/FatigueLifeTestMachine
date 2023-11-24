using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatigueLifeTestMachine.Models
{
    public class TestInformationModel
    {
        private string _testNumber;
        public string TestNumber
        {
            get
            {
                return _testNumber;
            }
            set
            {
                _testNumber = value;
            }
        }

        private string _tester;
        public string Tester
        {
            get
            {
                return _tester;
            }
            set
            {
                _tester = value;
            }
        }

        private List<string> _testTypeList = new List<string>() { "油润滑", "脂润滑" };
        public List<string> TestTypeList { get { return _testTypeList; } }

        private string _testType;
        public string TestType
        {
            get
            {
                return _testType;
            }
            set
            {
                _testType = value;
            }
        }

        private int? _cyclePeriod = null;
        public int? CyclePeriod { get => _cyclePeriod; set => _cyclePeriod = value; }

        private int? _saveInterval = null;
        public int? SaveInterval { get => _saveInterval; set => _saveInterval = value; }

        private List<BearingModel> _bearingList = new List<BearingModel>();

        public List<BearingModel> BearingList
        {
            get
            {
                return _bearingList;
            }
            set
            {
                _bearingList = value;
            }
        }

        public TestInformationModel()
        {
            _bearingList.Add(new BearingModel("1#"));
            _bearingList.Add(new BearingModel("2#"));
            _bearingList.Add(new BearingModel("3#"));
            _bearingList.Add(new BearingModel("4#"));
        }
    }
}
