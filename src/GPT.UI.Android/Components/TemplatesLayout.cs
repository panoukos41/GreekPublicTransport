using Android.Content;
using Android.Util;
using Android.Views;

namespace GPT.Components;

public sealed class TemplatesLayout : FrameLayout
{
    private int DefaultId { get; }

    public TemplatesLayout(Context context, IAttributeSet? attrs) : base(context, attrs)
    {
        DefaultId = context.ObtainStyledAttributes(attrs, R.Styleable.TemplatesLayout).GetResourceId(0, 0);
    }

    public override void OnViewAdded(View? child)
    {
        base.OnViewAdded(child);

        if (child is null) return;

        child.Visibility = child.Id == DefaultId ? ViewStates.Visible : ViewStates.Gone;

        System.Diagnostics.Debug.WriteLine("Child added");
    }

    public void Show(int id)
    {
        foreach (var child in Children)
        {
            child.Visibility = child.Id == id ? ViewStates.Visible : ViewStates.Gone;
        }
    }

    private IEnumerable<View> Children
    {
        get
        {
            var count = ChildCount;
            for (int i = 0; i < count; i++)
            {
                yield return GetChildAt(i)!;
            }
        }
    }
}
