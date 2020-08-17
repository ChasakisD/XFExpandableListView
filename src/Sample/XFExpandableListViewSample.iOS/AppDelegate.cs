using Foundation;
using UIKit;
using Xamarin.Forms;
using XamarinBackgroundKit.iOS;

namespace XFExpandableListViewSample.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register(nameof(AppDelegate))]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Init();

            FormsMaterial.Init();
            BackgroundKit.Init();

            LoadApplication(new App());
            return base.FinishedLaunching(app, options);
        }
    }
}


