using System;
using System.Collections.Generic;
using System.Text;

namespace ViralCollapse.Models
{
    public class AreaTree
    {
        public AreaTree()
        {
            this.Today = new Today();
            this.Total = new Overview();
            this.Children = new List<Children>();
        }
        public string Name { get; set; }

        public Today Today { get; set; }

        public Overview Total { get; set; }

        public List<Children> Children { get; set; }
    }

    public class Children
    {
        // 省份或者城市
        public string Name { get; set; }

        // 今日变化
        public Today Today { get; set; }
        // 统计
        public Overview Total { get; set; }

        public List<Children> children { get; set; }
    }
}
