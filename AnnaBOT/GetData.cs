using AnnaBOT.Entity;
using Newtonsoft.Json;

namespace AnnaBOT
{
    /// <summary>
    /// 获取数据的类，包含了程序主要使用的方法
    /// </summary>
    internal class GetData
    {
        public async Task<string[]> GetEventDataAsync()
        {
            TimeZoneInfo JapanZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");//设置日本时区
            DateTime JapanTime = TimeZoneInfo.ConvertTime(DateTime.Now, JapanZone);//将本地时间转换为日本时区的当地时间
            Uri getEventDataUri = new Uri(new string("https://api.matsurihi.me/api/mltd/v2/events?at=" + JapanTime)); //拼接获取json信息的所需的uri
            HttpClient client = new HttpClient(); //创建HttpClient实例才能发送HTTP请求
            string eventName = "暂无";
            int eventID = 0;
            string imageUri = "please_click_refresh_button.jpg";
            string eventBiginAt = "暂无";
            string eventEndAt = "暂无";
            string eventBoostAt = "暂无";
            try
            {
                var EvemtJson = await client.GetStringAsync(getEventDataUri); //发送Get请求，从URI标识的资源接收json并将其转化为字符串格式
                var eventData = JsonConvert.DeserializeObject<List<EventData>>(EvemtJson);//通过反序列化Json将数据传递给新实例
                //取出活动数据
                //如果此时没有活动，也有可能导致取值失败，则发送控制台消息
                try
                {
                    foreach (var eventDataA in eventData)
                    {
                        eventName = eventDataA.name;//从新实例类（以下不再重复）获取活动名称
                        eventID = eventDataA.id;
                        imageUri = "https://storage.matsurihi.me/mltd/event_bg/" + "0" + eventDataA.id + ".png";//拼接图片Uri字符串
                        eventBiginAt = eventDataA.schedule.beginAt.ToLocalTime().ToString();//获取活动开始时间，并转换为本地时间，最后转化为字符串格式
                        eventEndAt = eventDataA.schedule.endAt.ToLocalTime().ToString();//获取活动结束时间，并转换为本地时间，最后转化为字符串格式
                        //并非所有活动都含有后半战，有后半战的活动也就意味着有折返开始时间
                        //若折返时间的属性含有有效值，能成功拆箱，则将其转换为本地时间，最后转化为字符串格式
                        //否则将其字符串改为"本次活动无折返"
                        try
                        {
                            DateTime dateTime = (DateTime)eventDataA.schedule.boostBeginAt;
                            eventBoostAt = dateTime.ToLocalTime().ToString();
                        }
                        catch (InvalidCastException e)
                        {
                            Console.WriteLine("{0} Error: 拆箱错误.", e.Message);
                            eventBoostAt = "本次活动无折返";
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("活动信息获取失败" + e.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "可能是由404引起的问题");
            }
            string theTime = DateTime.Now.ToString();//执行本次获取的当地时间

            //
            //      以上是获取前活动名称等信息的代码，接下来是获取分数
            //

            int rank2500Score = 0;
            int rank100Score = 0;
            string rank2500ScoreString = "";
            string rank100ScoreString = "";
            DateTime UriTime = DateTime.Now;//本地时间
            Uri get2500ScoreUri = new Uri(new string("https://api.matsurihi.me/api/mltd/v2/events/" + eventID + "/rankings/eventPoint/logs/2500?since=" + UriTime)); //拼接获取json信息的所需的uri
            Uri get100ScoreUri = new Uri(new string("https://api.matsurihi.me/api/mltd/v2/events/" + eventID + "/rankings/eventPoint/logs/100?since=" + UriTime)); //拼接获取json信息的所需的uri
            try
            {
                string rank2500Json = await client.GetStringAsync(get2500ScoreUri); //发送Get请求，从URI标识的资源接收json并将其转化为字符串格式
                string rank100Json = await client.GetStringAsync(get100ScoreUri); //发送Get请求，从URI标识的资源接收json并将其转化为字符串格式
                var rank2500Data = JsonConvert.DeserializeObject<List<RankData>>(rank2500Json);//反序列化
                var rank100Data = JsonConvert.DeserializeObject<List<RankData>>(rank100Json);//反序列化
                //取出25000位和1000位的分数
                //如果有多条数据，则最后一条数据会覆盖之前的，这样保证数据是最近一次的
                //如果此时没有活动或没分数，也有可能导致取值失败，则发送控制台消息
                try
                {
                    foreach (var scoreDataA in rank2500Data)
                    {
                        foreach (var dataA in scoreDataA.data)
                        {
                            rank2500Score = dataA.score;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + "获取2500位分数时发生未知错误");
                }

                try
                {
                    foreach (var scoreDataA in rank100Data)
                    {
                        foreach (var dataA in scoreDataA.data)
                        {
                            rank100Score = dataA.score;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + "获取100位分数时发生未知错误");
                }

                rank100ScoreString = rank2500Score.ToString();
                rank100ScoreString = rank100Score.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "可能是由404引起的问题");
            }
            string[] eventDatas = { eventName, imageUri, eventBiginAt, eventEndAt, eventBoostAt, theTime, rank100ScoreString, rank2500ScoreString };//把所需数据添加到数组中，方便返回
            return eventDatas;//返回数组
        }
    }
}