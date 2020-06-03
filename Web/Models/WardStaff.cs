using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class WardStaff
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public String Position { get; set; }

        public Int32 WardId { get; set; }
        public Ward Ward { get; set; }
    }
}
