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
//// https://github.com/reactiveui/ReactiveUI/blob/main/src/ReactiveUI.AndroidX/ReactiveRecyclerViewViewHolder.cs
////
//// Modified by Panagiotis Athanasiou, Removed IReactiveObject and the need for the viewmodel to implement IReactiveObject.

//using Android.Views;
//using System.ComponentModel;
//using System.Runtime.CompilerServices;

//namespace AndroidX.RecyclerView.Widget;

//// todo
///// <summary>
///// A <see cref="RecyclerView.ViewHolder"/> implementation that binds to a reactive view model.
///// </summary>
///// <typeparam name="TItem">The type of the item.</typeparam>
//public abstract class RecyclerViewViewHolder<TItem> : RecyclerView.ViewHolder, INotifyPropertyChanged
//        where TItem : class
//{
//    private TItem _item;

//    /// <summary>
//    /// Initializes a new instance of the <see cref="RecyclerViewViewHolder{TItem}"/> class.
//    /// </summary>
//    /// <param name="view">The view.</param>
//    protected RecyclerViewViewHolder(View view) : base(view)
//    {
//        view.ViewAttachedToWindow += (s, e) =>
//        {
//            view.Click += OnClick;
//            view.LongClick += OnLongClick;
//        };

//        view.ViewDetachedFromWindow += (s, e) =>
//        {
//            view.Click -= OnClick;
//            view.LongClick -= OnLongClick;
//        };
//    }

//    private void OnClick(object s, EventArgs e)
//    {
//        Selected?.Invoke(this, AdapterPosition);
//        SelectedWithItem?.Invoke(this, Item);
//    }

//    private void OnLongClick(object s, View.LongClickEventArgs e)
//    {
//        LongClicked?.Invoke(this, AdapterPosition);
//        LongClickedWithItem?.Invoke(this, Item);
//    }

//    /// <summary>
//    /// Raised when a property has changed value.
//    /// </summary>
//    public event PropertyChangedEventHandler PropertyChanged;

//    /// <summary>
//    /// Raised when the view-holder is Selected.
//    /// The <see cref="int"/> is the position of this ViewHolder in the <see cref="RecyclerView"/>
//    /// and corresponds to the <see cref="RecyclerView.ViewHolder.AdapterPosition"/> property.
//    /// </summary>
//    public EventHandler<int> Selected { get; set; }

//    /// <summary>
//    /// Raised when the view-holder is Selected.
//    /// The TItem is the Item of this ViewHolder in the <see cref="RecyclerView"/>.
//    /// </summary>
//    public EventHandler<TItem> SelectedWithItem { get; set; }

//    /// <summary>
//    /// Raised when the view-holder is LongClicked.
//    /// The <see cref="int"/> is the position of this ViewHolder in the <see cref="RecyclerView"/>
//    /// and corresponds to the <see cref="RecyclerView.ViewHolder.AdapterPosition"/> property.
//    /// </summary>
//    public EventHandler<int> LongClicked { get; set; }

//    /// <summary>
//    /// Raised when the view-holder is LongClicked.
//    /// The TItem is the Item of this ViewHolder in the <see cref="RecyclerView"/>.
//    /// </summary>
//    public EventHandler<TItem> LongClickedWithItem { get; set; }

//    /// <summary>
//    /// Gets the current view being shown.
//    /// </summary>
//    public View View => ItemView;

//    /// <summary>
//    /// The ViewHolder's item.
//    /// </summary>
//    public TItem Item
//    {
//        get => _item;
//        set => Set(ref _item, value);
//    }

//    #region PropertyChanged

//    protected bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
//    {
//        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
//        field = value;
//        OnPropertyChanged(propertyName);
//        return true;
//    }

//    protected virtual void OnPropertyChanged(string propertyName) =>
//        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

//    #endregion
//}
