using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Forms
{
    public class WardCreateForm
    {
        [Required]
        [MaxLength(200)]
        public String Name { get; set; }
    }
}
