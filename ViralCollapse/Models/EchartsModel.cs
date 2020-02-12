using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViralCollapse.Models
{
    public class EchartsModel
    {
        public string name { get; set; }

        public int value { get; set; }

        public object clone()
        {
            return this.MemberwiseClone();
        }
    }
}
