using Android.Views;
using Google.Android.Material.BottomSheet;


namespace GPT.Components;

public delegate View OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState);

public sealed class BottomSheet : BottomSheetDialogFragment
{
    public OnCreateView CreateView { get; }

    public BottomSheet(OnCreateView onCreateView)
    {
        CreateView = onCreateView;
    }

    public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
    {
        return CreateView(inflater, container, savedInstanceState);
    }
}
