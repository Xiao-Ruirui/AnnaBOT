namespace AnnaBOT;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
		

	}

	protected override Window CreateWindow(IActivationState activationState)
      {
        var window = base.CreateWindow(activationState);
		window.Height = 650;// 设置窗口高度
		window.Width = 500;// 设置窗口宽度
		window.MaximumHeight = 650; // 设置窗口最大高度
		window.MaximumWidth = 500; // 设置窗口最大宽度
		window.Title = "Anna Bot"; //设置窗口标题
		return window;
      }
}
