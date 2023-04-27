//// The MIT License(MIT)
//// 
//// Copyright(c) .NET Foundation and Contributors
//// 
//// All rights reserved.
//// 
//// Permission is hereby granted, free of charge, to any person obtaining a copy
//// of this software and associated documentation files (the "Software"), to deal
//// in the Software without restriction, including without limitation the rights
//// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//// copies of the Software, and to permit persons to whom the Software is
//// furnished to do so, subject to the following conditions:
//// 
//// The above copyright notice and this permission notice shall be included in all
//// copies or substantial portions of the Software.
//// 
//// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//// SOFTWARE.
////
//// https://github.com/reactiveui/ReactiveUI/blob/main/src/ReactiveUI.AndroidX/ReactiveRecyclerViewAdapter.cs
////
//// Modified by Panagiotis Athanasiou, Removed IReactiveObject and the need for the viewmodel to implement IReactiveObject.

//using System.Collections.ObjectModel;
//using System.Collections.Specialized;

//namespace AndroidX.RecyclerView.Widget;

//// todo
///// <summary>
///// An adapter for the Android <see cref="RecyclerView"/>.
///// </summary>
///// <typeparam name="TItem">The type of Item that this adapter holds.</typeparam>
//public abstract class RecyclerViewAdapter<TItem> : RecyclerView.Adapter
//    where TItem : class
//{
//    private readonly IEnumerable<TItem> _list;

//    /// <summary>
//    /// Initializes a new instance of the <see cref="RecyclerViewAdapter{TItem}"/> class.
//    /// </summary>
//    /// <param name="backingList">The backing list.</param>
//    protected RecyclerViewAdapter(IEnumerable<TItem> backingList)
//    {
//        _list = backingList;

//        if (backingList is INotifyCollectionChanged notifier)
//            notifier.CollectionChanged += HandleCollectionChanged;
//    }

//    /// <summary>
//    /// Initializes a new instance of the <see cref="RecyclerViewAdapter{TItem}"/> class.
//    /// </summary>
//    /// <param name="backingList">The backing list.</param>
//    protected RecyclerViewAdapter(ObservableCollection<TItem> backingList)
//    {
//        _list = backingList;

//        if (backingList is INotifyCollectionChanged notifier)
//            notifier.CollectionChanged += HandleCollectionChanged;
//    }

//    private void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
//    {
//        switch (e.Action)
//        {
//            case NotifyCollectionChangedAction.Add:
//                {
//                    var count = e.NewItems.Count;
//                    for (var i = 0; i < count; i++)
//                    {
//                        NotifyItemInserted(e.NewStartingIndex + i);
//                    }
//                }
//                break;

//            case NotifyCollectionChangedAction.Remove:
//                {
//                    int i = 0;
//                    foreach (var item in e.OldItems)
//                    {
//                        NotifyItemRemoved(e.OldStartingIndex + i++);
//                    }
//                }
//                break;

//            default:
//                NotifyDataSetChanged();
//                break;
//        };
//    }

//    /// <inheritdoc/>
//    public override int ItemCount => _list.Count();

//    /// <inheritdoc/>
//    public override int GetItemViewType(int position) =>
//        GetItemViewType(position, GetItemByPosition(position));

//    /// <summary>
//    /// Determine the View that will be used/re-used in lists where
//    /// the list contains different cell designs.
//    /// </summary>
//    /// <param name="position">The position of the current view in the list.</param>
//    /// <param name="viewModel">The Item associated with the current View.</param>
//    /// <returns>An ID to be used in OnCreateViewHolder.</returns>
//    public virtual int GetItemViewType(int position, TItem viewModel) =>
//        base.GetItemViewType(position);

//    public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
//    {
//        if (holder == null)
//        {
//            throw new ArgumentNullException(nameof(holder));
//        }
//        if (holder is RecyclerViewViewHolder<TItem> viewForHolder)
//        {
//            viewForHolder.Item = GetItemByPosition(position);
//        }
//    }

//    protected override void Dispose(bool disposing)
//    {
//        base.Dispose(disposing);
//    }

//    protected TItem GetItemByPosition(int position)
//    {
//        return position >= _list.Count() ? null : _list.ElementAt(position);
//    }
//}
