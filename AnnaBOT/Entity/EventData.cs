namespace AnnaBOT.Entity
{
    /// <summary>
    /// 活动信息
    /// </summary>
    public class EventData
    {
        /// <summary>
        /// 活动ID，用于获取活动图片
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 活动名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 活动日程
        /// </summary>
        public Schedule schedule { get; set; }
    }
}