using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTasks.Models
{
    public class Doctor
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public String Speciality { get; set; }

        public ICollection<HospitalDoctor> Hospitals { get; set; }
    }
}
