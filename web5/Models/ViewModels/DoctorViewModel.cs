using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace web5.Models.ViewModels
{
    public class DoctorViewModel
    {
        [Required]
        public String Name { get; set; }
        public String Speciality { get; set; }
    }
}
