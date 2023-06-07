using FluentDateTime;

namespace AnnaBOT;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        RefreashData();
    }

    private async void OnRefreshClicked(object sender, EventArgs e)
    {
        await RefreashData();
    }

    private async void OnEventEndCountdownToggled(object sender, EventArgs e)
    {
        await EventEndCountdown();
    }

    private async void OnRefreshCountdownToggled(object sender, EventArgs e)
    {
        await RefreshCountdown();
    }

    private async Task RefreashData()
    {
        GetData getData = new GetData(); //创建实例以便使用获取数据的方法
        //获取数据数组,内容分别为（活动名称，活动图片地址，活动开始时间，活动结束时间，折返开始时间，上次数据获取时间）
        string[] datas = await getData.GetEventDataAsync();
        //更新数据到控件
        this.eventName.Text = datas[0];
        this.eventImage.Source = datas[1];
        this.eventAt.Text = "<b>活动开启时间：</b>" + datas[2] + "〜\n" + datas[3];
        this.eventBoostBeginAt.Text = "<b>折返开启时间：</b>" + datas[4] + "〜";
        this.getDatasTime.Text = "<b>上次数据获取时间：</b>" + datas[5];
        this.rank100Score.Text = "<b>100位pt：</b>" + datas[6];
        this.rank2500Score.Text = "<b>2500位pt：</b>" + datas[7];
        SemanticScreenReader.Announce("数据已刷新"); //发送信息告诉用户
    }

    private async Task EventEndCountdown()
    {
        GetData getData = new GetData(); //获取数据
        string[] datas = await getData.GetEventDataAsync(); //数据传入数组
        try
        {
            DateTime eventEndTime = Convert.ToDateTime(datas[3]); //数组内第四个字符串就是活动结束日期，需要手动转换格式
            while (this.eventEndCountdownS.IsToggled) //如果开关目前为开的状态，就每0.1秒刷新时间间隔
            {
                DateTime theTime = DateTime.Now;
                TimeSpan timeSpan = eventEndTime - theTime;
                this.endTime.Text = "<b>距离活动结束还有：</b>" + timeSpan;
                await Task.Delay(100);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString() + "也许是因为没有活动数据，程序出错啦");
        }
    }

    private async Task RefreshCountdown()
    {
        GetData getData = new GetData(); //获取数据
        string[] datas = await getData.GetEventDataAsync(); //数据传入数组
        try
        {
            AutoRefresh();//自动刷新
            //TODO:逻辑不对，优化成检查页面的生命周期状态
            String Test = datas[0]; //如果能取到数据，那窗口一定是创建好了
            //当开关是开启的状态时，通过循环获取当前时间
            //如果时间的分钟小于30，则计算距离下个半点还差多久，并把数据更新到页面
            //如果时间的分钟大于等于30，则计算距离下个整点还差多久，并把数据更新到页面
            while (this.refreshTimeCountdownS.IsToggled)
            {
                DateTime theTime = DateTime.Now;
                if (theTime.Minute < 30)
                {
                    DateTime nextTime = theTime.SetMinute(30).SetSecond(0);
                    TimeSpan timeSpan = nextTime - theTime;
                    this.refreshTime.Text = "<b>距离数据刷新还有：</b>" + timeSpan;
                    await Task.Delay(100);
                }
                else
                {
                    DateTime nextTime = theTime.SetHour(theTime.Hour + 1).SetMinute(0).SetSecond(0);
                    TimeSpan timeSpan = nextTime - theTime;
                    this.refreshTime.Text = "<b>距离数据刷新还有：</b>" + timeSpan;
                    await Task.Delay(100);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("获取数据的倒计时生成失败" + ex.Message);
        }
    }
    /// <summary>
    /// 自动刷新，调用时首先会刷新一次，然后如果自动刷新的倒计时开关为“开”，
    /// 则计算下次半点刷新或整点刷新还要等多久，需要等多久就让线程暂停多久再继续执行，继续执行时会直接再次调用自己
    /// </summary>
    private async Task AutoRefresh()
    {
        RefreashData();
        if (this.eventEndCountdownS.IsToggled)
        {
            DateTime theTime = DateTime.Now;
            if (theTime.Minute < 30)
            {
                DateTime nextTime = theTime.SetMinute(30).SetSecond(0);
                TimeSpan timeSpan = nextTime - theTime;
                await Task.Delay(timeSpan);
                AutoRefresh();
            }
            else
            {
                DateTime nextTime = theTime.SetHour(theTime.Hour + 1).SetMinute(0).SetSecond(0);
                TimeSpan timeSpan = nextTime - theTime;
                await Task.Delay(timeSpan);
                AutoRefresh();
            }
        }
    }
}