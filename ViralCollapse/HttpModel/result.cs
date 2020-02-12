using System;
using System.Collections.Generic;
using System.Text;

namespace ViralCollapse.Models
{
    public class result
    {
        public location location { get; set; }

        public int precise { get; set; }

        public int confidence { get; set; }

        public int comprehension { get; set; }

        public string level { get; set; }
    }
}
