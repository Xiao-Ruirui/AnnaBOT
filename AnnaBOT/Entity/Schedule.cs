namespace AnnaBOT.Entity
{
    /// <summary>
    /// 活动日程
    /// </summary>
    public class Schedule
    {
        /// <summary>
        /// 活动开始时间
        /// </summary>
        public DateTime beginAt { get; set; }

        /// <summary>
        /// 活动结束时间
        /// </summary>
        public DateTime endAt { get; set; }

        /// <summary>
        /// 折返开始时间
        /// </summary>
        public object boostBeginAt { get; set; }
    }
}