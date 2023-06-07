using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Maui.Controls.Internals.Profile;

namespace AnnaBOT.Entity
{
    /// <summary>
    /// 活动排位数据
    /// </summary>
    internal class RankData
    {
        /// <summary>
        /// 活动排位数据
        /// </summary>
        public List<RankDatum> data { get; set; }
    }
}
