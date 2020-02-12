using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViralCollapse.Models
{
    public class EchartsMapModel
    {
        public string name { get; set; }
        public List<EchartsModel> value { get; set; }

        public object clone()
        {
            return this.MemberwiseClone();
        }
    }
}
