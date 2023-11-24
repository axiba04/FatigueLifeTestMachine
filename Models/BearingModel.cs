using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatigueLifeTestMachine.Models
{
    public class BearingModel
    {
        private string _station;
        [DisplayName("工位")]
        public string Station
        {
            get
            {
                return _station;
            }
            set
            {
                _station = value;
            }
        }

        private string _modelNumber;
        [DisplayName("型号")]
        public string ModelNumber
        {
            get
            {
                return _modelNumber;
            }
            set
            {
                _modelNumber = value;
            }
        }

        private string _brand;
        [DisplayName("品牌")]
        public string Brand
        {
            get
            {
                return _brand;
            }
            set
            {
                _brand = value;
            }
        }

        private string _id;
        [DisplayName("编号")]
        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public BearingModel(string station)
        {
            Station = station;
        }
    }
}
