using Android.Views;
using AndroidX.RecyclerView.Widget;

namespace GPT.UI.Android.Common;

using View = global::Android.Views.View;

/// <summary>
/// An adapter for the Android <see cref="RecyclerView"/>.
/// </summary>
/// <typeparam name="T">The type of ViewModel that this adapter holds.</typeparam>
public sealed class RecyclerAdapter<T> : RecyclerView.Adapter
{
    private readonly ISourceList<T> _list;
    private readonly IDisposable _inner;
    private readonly int resource;

    /// <summary>
    /// Initializes a new instance of the <see cref="RecyclerAdapter{TViewModel}"/> class.
    /// </summary>
    /// <param name="backingList">The backing list.</param>
    public RecyclerAdapter(int resource, IObservable<IChangeSet<T>> backingList)
    {
        _list = new SourceList<T>(backingList);
        _inner = _list.Connect().ForEachChange(UpdateBindings).Subscribe();
        this.resource = resource;
    }

    public T this[int position] => _list.Items.ElementAt(position);

    private void UpdateBindings(Change<T> change)
    {
        switch (change.Reason)
        {
            case ListChangeReason.Add:
                NotifyItemInserted(change.Item.CurrentIndex);
                break;
            case ListChangeReason.Remove:
                NotifyItemRemoved(change.Item.CurrentIndex);
                break;
            case ListChangeReason.Moved:
                NotifyItemMoved(change.Item.PreviousIndex, change.Item.CurrentIndex);
                break;
            case ListChangeReason.Replace:
            case ListChangeReason.Refresh:
                NotifyItemChanged(change.Item.CurrentIndex);
                break;
            case ListChangeReason.AddRange:
                NotifyItemRangeInserted(change.Range.Index, change.Range.Count);
                break;
            case ListChangeReason.RemoveRange:
            case ListChangeReason.Clear:
                NotifyItemRangeRemoved(change.Range.Index, change.Range.Count);
                break;
        }
    }

    /// <inheritdoc/>
    public override int ItemCount => _list.Count;

    /// <summary>
    /// Set the method that is executed when your <see cref="View"/> is Bound to a ViewHolder.
    /// </summary>
    public Action<View, T, CompositeDisposable>? OnBind { get; set; }

    /// <summary>
    /// Gets an observable that signals when a ViewHolder has been clicked.
    /// The <see cref="int"/> is the position of the ViewHolder in the <see cref="RecyclerView"/>
    /// and corresponds to the <see cref="RecyclerView.ViewHolder.AbsoluteAdapterPosition"/> property.
    /// </summary>
    public Action<int>? ClickedIndex { get; set; }

    /// <summary>
    /// Gets an observable that signals that a ViewHolder has been clicked.
    /// The <typeparamref name="T"/> is the item of the ViewHolder in the <see cref="RecyclerView"/>.
    /// </summary>
    public Action<T>? ClickedItem { get; set; }

    /// <summary>
    /// Gets an observable that signals that a ViewHolder has been long-clicked.
    /// The <see cref="int"/> is the position of the ViewHolder in the <see cref="RecyclerView"/>
    /// and corresponds to the <see cref="RecyclerView.ViewHolder.AbsoluteAdapterPosition"/> property.
    /// </summary>
    public Action<int>? LongClickedIndex { get; set; }

    /// <summary>
    /// Gets an observable that signals that a ViewHolder has been long-clicked.
    /// The <typeparamref name="T"/> is the item of the ViewModel in the <see cref="RecyclerView"/>
    /// </summary>
    public Action<T>? LongClickedItem { get; set; }

    /// <inheritdoc/>
    public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
    {
        var item = _list.Items.ElementAt(position);
        var viewHolder = (ViewHolder)holder;

        viewHolder.Item = item;
        viewHolder.disposables.Clear();

        OnBind?.Invoke(viewHolder.ItemView, item, viewHolder.disposables);
    }

    /// <inheritdoc/>
    public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
    {
        var view = LayoutInflater.From(parent.Context)!.Inflate(resource, parent, false);
        var vh = new ViewHolder(view!);

        if (ClickedIndex is not null)
        {
            vh.ClickedIndex.Subscribe(ClickedIndex);
        }
        if (ClickedItem is not null)
        {
            vh.ClickedItem.Subscribe(ClickedItem);
        }
        if (LongClickedIndex is not null)
        {
            vh.LongClickedIndex.Subscribe(LongClickedIndex);
        }
        if (LongClickedItem is not null)
        {
            vh.LongClickedItem.Subscribe(LongClickedItem);
        }
        return vh;
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _inner.Dispose();
            _list.Dispose();
        }
        base.Dispose(disposing);
    }

    private sealed class ViewHolder : RecyclerView.ViewHolder
    {
        public T? Item;
        public readonly IObservable<int> ClickedIndex;
        public readonly IObservable<T> ClickedItem;
        public readonly IObservable<int> LongClickedIndex;
        public readonly IObservable<T> LongClickedItem;

        public CompositeDisposable disposables = new();

        public ViewHolder(View view) : base(view)
        {
            ClickedIndex = view.Events().Click.Select(e => AbsoluteAdapterPosition);
            ClickedItem = view.Events().Click.Select(e => Item);
            LongClickedIndex = view.Events().LongClick.Select(e => AbsoluteAdapterPosition);
            LongClickedItem = view.Events().LongClick.Select(e => Item);
        }
    }
}

public static class RecyclerAdapterMixins
{
    public static RecyclerAdapter<T> ToRecyclerAdapter<T>(
        this IObservable<IChangeSet<T>> backingList,
        int resource,
        Action<View, T, CompositeDisposable>? onBind,
        Action<int>? clickedIndex = null,
        Action<int>? longClickedIndex = null,
        Action<T>? clickedItem = null,
        Action<T>? longClickedItem = null)
        => new(resource, backingList)
        {
            OnBind = onBind,
            ClickedIndex = clickedIndex,
            ClickedItem = clickedItem,
            LongClickedIndex = longClickedIndex,
            LongClickedItem = longClickedItem
        };
}
