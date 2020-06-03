using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Forms
{
    public class WardEditForm
    {
        [Required]
        [MaxLength(200)]
        public String Name { get; set; }
    }
}
