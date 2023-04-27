//using Android.Views;
//using AndroidX.RecyclerView.Widget;
//using DynamicData.Binding;
//using ReactiveUI;

//namespace GPT.UI.Android.Common;

//public class RVAdapter<T> : ReactiveUI.AndroidX.ReactiveRecyclerViewAdapter<T>
//    where T : class, IReactiveObject
//{
//    private readonly int resource;

//    public Action<global::Android.Views.View, T>? OnBind { get; init; }

//    public Action<int>? Selected { get; init; }

//    public Action<T?>? SelectedItem { get; init; }

//    public Action<int>? LongClicked { get; init; }

//    public Action<T?>? LongClickedItem { get; init; }

//    public RVAdapter(IObservable<IChangeSet<T>> backingList, int resource) : base(backingList)
//    {
//        this.resource = resource;
//    }

//    public RVAdapter(IObservableList<T> backingList, int resource)
//        : base(backingList.Connect())
//    {
//        this.resource = resource;
//    }

//    public RVAdapter(IObservableCollection<T> backingList, int resource)
//        : base(backingList.ToObservableChangeSet<IObservableCollection<T>, T>())
//    {
//        this.resource = resource;
//    }

//    public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
//    {
//        base.OnBindViewHolder(holder, position);
//        var vh = (ViewHolder)holder;

//        OnBind?.Invoke(vh.View, vh.ViewModel!);
//    }

//    public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
//    {
//        var view = LayoutInflater.From(parent.Context)!.Inflate(resource, parent, false);
//        var vh = new ViewHolder(view!);

//        if (Selected is not null)
//        {
//            vh.Selected.Subscribe(Selected);
//        }
//        if (SelectedItem is not null)
//        {
//            vh.SelectedWithViewModel.Subscribe(SelectedItem);
//        }
//        if (LongClicked is not null)
//        {
//            vh.LongClicked.Subscribe(LongClicked);
//        }
//        if (LongClickedItem is not null)
//        {
//            vh.LongClickedWithViewModel.Subscribe(LongClickedItem);
//        }
//        return vh;
//    }

//    public class ViewHolder : ReactiveUI.AndroidX.ReactiveRecyclerViewViewHolder<T>
//    {
//        public ViewHolder(global::Android.Views.View view) : base(view)
//        {
//        }
//    }
//}

//public static class RVAdapterExtensions
//{
//    public static RVAdapter<T> ToRvAdapter<T>(this IObservable<IChangeSet<T>> observable,
//        int resource,
//        Action<global::Android.Views.View, T>? onBind = null,
//        Action<int>? selected = null,
//        Action<T?>? selectedItem = null,
//        Action<int>? longClicked = null,
//        Action<T?>? longClickedItem = null)
//        where T : class, IReactiveObject
//        => new(observable, resource)
//        {
//            OnBind = onBind,
//            Selected = selected,
//            SelectedItem = selectedItem,
//            LongClicked = longClicked,
//            LongClickedItem = longClickedItem
//        };

//    public static RVAdapter<T> ToRvAdapter<T>(this IObservableList<T> observable,
//        int resource,
//        Action<global::Android.Views.View, T>? onBind = null,
//        Action<int>? selected = null,
//        Action<T?>? selectedItem = null,
//        Action<int>? longClicked = null,
//        Action<T?>? longClickedItem = null)
//        where T : class, IReactiveObject
//        => new(observable, resource)
//        {
//            OnBind = onBind,
//            Selected = selected,
//            SelectedItem = selectedItem,
//            LongClicked = longClicked,
//            LongClickedItem = longClickedItem
//        };

//    public static RVAdapter<T> ToRvAdapter<T>(this IObservableCollection<T> observable,
//        int resource,
//        Action<global::Android.Views.View, T>? onBind = null,
//        Action<int>? selected = null,
//        Action<T?>? selectedItem = null,
//        Action<int>? longClicked = null,
//        Action<T?>? longClickedItem = null)
//        where T : class, IReactiveObject
//        => new(observable, resource)
//        {
//            OnBind = onBind,
//            Selected = selected,
//            SelectedItem = selectedItem,
//            LongClicked = longClicked,
//            LongClickedItem = longClickedItem
//        };
//}
