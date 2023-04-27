//using Android.Views;
//using System.Collections.ObjectModel;

//namespace AndroidX.RecyclerView.Widget;

///// <summary>
///// An adapter that can be set fluently to build recyclerviews fast.
///// </summary>
///// <typeparam name="TItem"></typeparam>
//public class FluentRecyclerViewAdapter<TItem> : RecyclerViewAdapter<TItem>
//    where TItem : class
//{
//    private int ViewType { get; set; }

//    private Action<View, TItem, int> BindViewHolderAction { get; set; }

//    /// <summary>
//    /// Initialize a new instance of <see cref="FluentRecyclerViewAdapter{TItem}"/>
//    /// </summary>
//    /// <param name="backingList"></param>
//    public FluentRecyclerViewAdapter(IEnumerable<TItem> backingList) : base(backingList)
//    {
//    }

//    /// <summary>
//    /// Initializes a new instance of the <see cref="RecyclerViewAdapter{TItem}"/> class.
//    /// </summary>
//    /// <param name="backingList">The backing list.</param>
//    public FluentRecyclerViewAdapter(ObservableCollection<TItem> backingList) : base(backingList)
//    {
//    }

//    /// <summary>
//    /// Called to create the view.
//    /// </summary>
//    /// <param name="parent"></param>
//    /// <param name="viewType"></param>
//    /// <returns></returns>
//    public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
//    {
//        var view = LayoutInflater.From(parent.Context)!.Inflate(ViewType, parent, false);
//        return new FlRecyclerViewViewHolder<TItem>(view!);
//    }

//    /// <summary>
//    /// Called when data need to bind to the view.
//    /// </summary>
//    /// <param name="holder"></param>
//    /// <param name="position"></param>
//    public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
//    {
//        base.OnBindViewHolder(holder, position);
//        if (BindViewHolderAction == null) throw new ArgumentNullException(nameof(OnBindViewHolder), "Please call SetOnBindViewHolder");

//        var h = holder as FlRecyclerViewViewHolder<TItem>;
//        BindViewHolderAction.Invoke(h.View, h.Item, h.AdapterPosition);
//    }

//    /// <summary>
//    /// Set the view that will be used to create the viewholder.
//    /// </summary>
//    /// <param name="viewType"></param>
//    /// <returns></returns>
//    public FluentRecyclerViewAdapter<TItem> SetView(int viewType)
//    {
//        ViewType = viewType == 0
//                ? throw new ArgumentException("Please provide a valid view", nameof(viewType))
//                : viewType;
//        return this;
//    }

//    /// <summary>
//    /// Set the method that will handle how the data bind to the view.
//    /// </summary>
//    /// <param name="bindViewHolder"></param>
//    /// <returns></returns>
//    public FluentRecyclerViewAdapter<TItem> SetOnBindViewHolder(Action<View, TItem, int> bindViewHolder)
//    {
//        BindViewHolderAction = bindViewHolder
//            ?? throw new ArgumentNullException(nameof(bindViewHolder), "Action can't be null");
//        return this;
//    }

//    private class FlRecyclerViewViewHolder<THolderItem> : RecyclerViewViewHolder<THolderItem>
//        where THolderItem : class
//    {
//        public FlRecyclerViewViewHolder(View view) : base(view)
//        {
//        }
//    }
//}
