using Xamarin.Forms.Xaml;
using XFExpandableListViewSample.Views;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace XFExpandableListViewSample
{
	public partial class App
	{
		public App ()
		{
			InitializeComponent();
            MainPage = new ExpandableListViewPage();
		}
	}
}
