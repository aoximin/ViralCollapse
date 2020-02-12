using System;
using System.Collections.Generic;
using System.Text;

namespace ViralCollapse.Models
{
    public class Today
    {
        public int Confirm { get; set; }

        public int Suspect { get; set; }

        public int Dead { get; set; }

        public int Heal { get; set; }

        public bool IsUpdated { get; set; }
    }
}
