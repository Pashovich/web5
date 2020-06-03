using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class Doctor
    {
        public Int32 Id { get; set; }
        [MaxLength(50)]
        public String Name { get; set; }
        public String Speciality { get; set; }

        public ICollection<HospitalDoctor> Hospitals { get; set; }
    }
}
