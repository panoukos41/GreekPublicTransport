using Android.Views;
using AndroidX.RecyclerView.Widget;

namespace GPT.UI.Android.Common;

using View = global::Android.Views.View;

public sealed class ListAdapter<T> : BaseAdapter<T>
{
    private readonly RecyclerAdapter<T> mAdapter;

    public ListAdapter(int resource, IObservable<IChangeSet<T>> backingList)
    {
        mAdapter = new RecyclerAdapter<T>(resource, backingList);
    }

    /// <inheritdoc/>
    public override T this[int position] => mAdapter[position];

    /// <inheritdoc/>
    public override int Count => mAdapter.ItemCount;

    /// <inheritdoc/>
    public Filter? Filter { get; }

    /// <summary>
    /// Set the method that is executed when your <see cref="View"/> is Bound to a ViewHolder.
    /// </summary>
    public Action<View, T, CompositeDisposable>? OnBind
    {
        get => mAdapter.OnBind;
        set => mAdapter.OnBind = value;
    }

    /// <summary>
    /// Gets an observable that signals when a ViewHolder has been clicked.
    /// The <see cref="int"/> is the position of the ViewHolder in the <see cref="RecyclerView"/>
    /// and corresponds to the <see cref="RecyclerView.ViewHolder.AbsoluteAdapterPosition"/> property.
    /// </summary>
    public Action<int>? ClickedIndex
    {
        get => mAdapter.ClickedIndex;
        set => mAdapter.ClickedIndex = value;
    }

    /// <summary>
    /// Gets an observable that signals that a ViewHolder has been clicked.
    /// The <typeparamref name="T"/> is the item of the ViewHolder in the <see cref="RecyclerView"/>.
    /// </summary>
    public Action<T?>? ClickedItem
    {
        get => mAdapter.ClickedItem;
        set => mAdapter.ClickedItem = value;
    }

    /// <summary>
    /// Gets an observable that signals that a ViewHolder has been long-clicked.
    /// The <see cref="int"/> is the position of the ViewHolder in the <see cref="RecyclerView"/>
    /// and corresponds to the <see cref="RecyclerView.ViewHolder.AbsoluteAdapterPosition"/> property.
    /// </summary>
    public Action<int>? LongClickedIndex
    {
        get => mAdapter.LongClickedIndex;
        set => mAdapter.LongClickedIndex = value;
    }

    /// <summary>
    /// Gets an observable that signals that a ViewHolder has been long-clicked.
    /// The <typeparamref name="T"/> is the item of the ViewModel in the <see cref="RecyclerView"/>
    /// </summary>
    public Action<T?>? LongClickedItem
    {
        get => mAdapter.LongClickedItem;
        set => mAdapter.LongClickedItem = value;
    }

    /// <inheritdoc/>
    public override long GetItemId(int position) => mAdapter.GetItemId(position);

    /// <inheritdoc/>
    public override View? GetView(int position, View? view, ViewGroup? parent)
    {
        RecyclerView.ViewHolder holder;
        if (view is null)
        {
            //holder = mAdapter.OnCreateViewHolder(parent!, mAdapter.GetItemViewType(position));
            holder = (RecyclerView.ViewHolder)mAdapter.CreateViewHolder(parent!, mAdapter.GetItemViewType(position));
            view = holder.ItemView;
            view.Tag = holder;
        }
        else
        {
            holder = (RecyclerView.ViewHolder)view.Tag!;
        }
        //mAdapter.OnBindViewHolder(holder, position);
        mAdapter.BindViewHolder(holder, position);
        return holder.ItemView;
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            mAdapter.Dispose();
        }
        base.Dispose(disposing);
    }
}

public static class ListAdapterMixins
{
    public static ListAdapter<T> ToListAdapter<T>(
        this IObservable<IChangeSet<T>> backingList,
        int resource,
        Action<View, T, CompositeDisposable>? onBind,
        Action<int>? clickedIndex = null,
        Action<int>? longClickedIndex = null,
        Action<T?>? clickedItem = null,
        Action<T?>? longClickedItem = null)
        => new(resource, backingList)
        {
            OnBind = onBind,
            ClickedIndex = clickedIndex,
            ClickedItem = clickedItem,
            LongClickedIndex = longClickedIndex,
            LongClickedItem = longClickedItem
        };
}
