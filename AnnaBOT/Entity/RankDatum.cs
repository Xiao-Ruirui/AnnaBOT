using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnaBOT.Entity
{
    /// <summary>
    /// 活动排位数据
    /// </summary>
    internal class RankDatum
    {
        /// <summary>
        /// 活动排位数据分数
        /// </summary>
        public int score { get; set; }
        /// <summary>
        /// 活动排位数据更新时间
        /// </summary>
        public DateTime aggregatedAt { get; set; }
    }
}
