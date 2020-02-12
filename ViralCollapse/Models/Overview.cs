using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViralCollapse.Models
{
    public class Overview
    {
        public int Confirm { get; set; }

        public int Suspect { get; set; }

        public int Dead { get; set; }

        public int Heal { get; set; }
    }
}
