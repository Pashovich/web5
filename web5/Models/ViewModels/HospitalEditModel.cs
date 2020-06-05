using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace web5.Models.ViewModels
{
    public class HospitalEditModel
    {
        [Required]
        [MaxLength(200)]
        public String Name { get; set; }

        public String Address { get; set; }

        public String Phones { get; set; }
    }
}
