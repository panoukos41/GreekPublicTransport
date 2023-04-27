using AndroidX.AppCompat.App;

namespace GPT.Abstractions;

public class ActivityBase : AppCompatActivity
{
    private Dictionary<int, Android.Views.View> cachedViews = new();

    internal TView FindCachedView<TView>(int id)
        where TView : Android.Views.View
    {
        if (cachedViews.ContainsKey(id))
            return (TView)cachedViews[id];

        var view = FindViewById<TView>(id)!;
        cachedViews[id] = view;
        return view;
    }
}
