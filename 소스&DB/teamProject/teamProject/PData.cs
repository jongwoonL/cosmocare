using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teamProject
{
    public class PData
    {
        public DateTime datetime { get; set; } // PK
        public double ReactA_Temp { get; set; }
        public double ReactB_Temp { get; set; }
        public double ReactC_Temp { get; set; }
        public double ReactD_Temp { get; set; }
        public double ReactE_Temp { get; set; }
        public double ReactF_Temp { get; set; }
        public double ReactF_PH { get; set; }
        public double Power { get; set; }
        public double CurrentA { get; set; }
        public double CurrentB { get; set; }
        public double CurrentC { get; set; }
    }
}
