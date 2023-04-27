using Android.Runtime;
using GPT.Bus;

namespace GPT.UI.Android;

[Application]
public class AndroidApp : Application
{
    public AndroidApp(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
    {
    }

    public override void OnCreate()
    {
        Services.Initialize(new AndroidServiceProvider());
        //Services.Initialize(new AndroidServices());
        //Xamarin.Essentials.Platform.Init(this);
        //Maui.Essentials.Platform.Init(this);
        base.OnCreate();
    }
}

public class AndroidServiceProvider : ServiceProvider
{
    protected override string DatabaseDirectory { get; } =
        Application.Context.GetExternalFilesDir(null)!.AbsolutePath;

    protected override INavigationHost GetNavigationHost()
    {
        return new NavigationHost()
            .Map("oasa/", () => new LineCollectionView())
            .Map("oasa/line/{}", () => new LineDetailsView())
            .Map("oasa/stop/{}", () => new StopDetailsView());
    }
}
