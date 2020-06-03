using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class Placement
    {
        public Int32 Id { get; set; }
        public Int32 Bed { get; set; }
        public Int32 WardId { get; set; }
        public Ward Ward { get; set; }
    }
}
