using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTasks.Models
{
    public class HospitalLab
    {
        public Int32 HospitalId { get; set; }

        public Hospital Hospital { get; set; }

        public Int32 LabId { get; set; }

        public Lab Lab { get; set; }
    }
}
