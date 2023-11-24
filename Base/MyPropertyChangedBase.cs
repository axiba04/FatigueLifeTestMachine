using Caliburn.Micro;
using OfficeOpenXml.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatigueLifeTestMachine.Base
{
    public class MyPropertyChangedBase : PropertyChangedBase
    {
        [EpplusIgnore]
        public override bool IsNotifying { get => base.IsNotifying; set => base.IsNotifying = value; }
    }
}
