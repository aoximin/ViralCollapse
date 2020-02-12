using System;
using System.Collections.Generic;
using System.Text;

namespace ViralCollapse.Models
{
    public class nCoVDataDetail
    {
        public nCoVDataDetail()
        {
            this.ChinaTotal = new Overview();
            this.ChinaAdd = new Overview();
            this.ChinaDayList = new List<ChinaDay>();
            this.ChinaDayAddList = new List<ChinaDay>();
            this.AreaTree = new List<AreaTree>();
        }

        public DateTime LastUpdateTime { get; set; }

        public Overview ChinaTotal { get; set; }

        public Overview ChinaAdd { get; set; }

        public bool IsShowAdd { get; set; }

        public List<ChinaDay> ChinaDayList { get; set; }

        public List<ChinaDay> ChinaDayAddList { get; set; }

        public List<AreaTree> AreaTree { get; set; }
    }
}
