using Xamarin.Forms.Xaml;
using XamarinBackgroundKit.Controls;
using XFExpandableListViewSample.Views;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace XFExpandableListViewSample
{
	public partial class App
	{
		public App ()
		{
			InitializeComponent();

			var x = typeof(MaterialShapeView);

			MainPage = new MainTabbedPage();
		}
	}
}
