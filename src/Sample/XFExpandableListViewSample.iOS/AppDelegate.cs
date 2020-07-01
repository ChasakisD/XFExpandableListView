using Foundation;
using UIKit;
using Xamarin.Forms;
using XamarinBackgroundKit.iOS;
using XEPlatform = Xamarin.Essentials.Platform;

namespace XFExpandableListViewSample.iOS
{
    [Register("AppDelegate")]
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


