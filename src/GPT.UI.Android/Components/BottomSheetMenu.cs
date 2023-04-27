using Android.Views;
using Google.Android.Material.BottomSheet;
using Google.Android.Material.Button;
using Google.Android.Material.TextView;
using System.Collections;

namespace GPT.UI.Android.Components;

public sealed record MenuItem<TKey>(TKey Key, string Text, MenuGroup? Group = null)
    where TKey : notnull
{
    public bool Enabled { get; set; } = true;

    public bool Equals(MenuItem<TKey>? other) => EqualityComparer<TKey>.Default.Equals(Key, other!.Key);

    public override int GetHashCode() => HashCode.Combine(Key);
}

public sealed record MenuGroup(int Key, string? Text = null)
{
    public bool Equals(MenuGroup? other) => EqualityComparer<int>.Default.Equals(Key, other!.Key);

    public override int GetHashCode() => HashCode.Combine(Key);
}

public class BottomSheetMenu<TKey> : BottomSheetDialogFragment, ICollection<MenuItem<TKey>>
     where TKey : notnull
{
    private readonly HashSet<MenuItem<TKey>> _inner = new();
    private readonly Subject<MenuItem<TKey>> _whenItemClicked = new();
    private readonly CompositeDisposable disposables = new();

    public IObservable<MenuItem<TKey>> WhenItemClick => _whenItemClicked.AsObservable();

    public BottomSheetMenu()
    {
        WhenItemClick.Subscribe(_ => Dismiss());
    }

    public void SetEnableToAll(bool enabled)
    {
        foreach (var item in this)
        {
            item.Enabled = enabled;
        }
    }

    public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
    {
        var view = inflater.Inflate(R.Layout.bottomsheetmenu, container, false)!;
        var parent = (LinearLayout)view;
        var defaultGroup = new MenuGroup(int.MaxValue, null);

        var groups = _inner
            .GroupBy(x => x.Group ?? defaultGroup)
            .OrderBy(x => x.Key.Key)
            .ToArray();

        for (int i = 0; i < groups.Length; i++)
        {
            AddGroupName(inflater, parent, groups[i].Key);

            if (i > 0)
            {
                AddDivider(inflater, parent);
            }

            foreach (var item in groups[i])
            {
                AddButton(inflater, parent, item);
            }
        }

        return view;
    }

    #region Render Methods

    private static void AddDivider(LayoutInflater inflater, LinearLayout parent)
    {
        //var divider = inflater.Inflate(R.Layout.c_bottom_menu_sheet_divider, parent, false)!;
        var divider = inflater.Inflate(R.Layout.bottomsheetmenu_divider, parent, false)!;
        parent.AddView(divider);
    }

    private static void AddGroupName(LayoutInflater inflater, LinearLayout parent, MenuGroup? group)
    {
        if (group is { Text.Length: > 0 })
        {
            //var textView = (MaterialTextView)inflater.Inflate(R.Layout.c_bottom_menu_sheet_group_name, parent, false)!;
            var textView = (MaterialTextView)inflater.Inflate(R.Layout.bottomsheetmenu_group_name, parent, false)!;
            textView.Text = group.Text;
            parent.AddView(textView);
        }
    }

    private void AddButton(LayoutInflater inflater, LinearLayout parent, MenuItem<TKey> item)
    {
        //var button = (MaterialButton)inflater.Inflate(R.Layout.c_bottom_menu_sheet_item, parent, false)!;
        var button = (MaterialButton)inflater.Inflate(R.Layout.bottomsheetmenu_item, parent, false)!;
        button.Text = item.Text;
        button.Enabled = item.Enabled;
        button.Events().Click
            .Select(_ => item)
            .Subscribe(_whenItemClicked)
            .DisposeWith(disposables);

        parent.AddView(button);
    }

    public override void OnDestroyView()
    {
        disposables.Clear();
        base.OnDestroyView();
    }

    #endregion

    #region ICollection

    public MenuItem<TKey> this[TKey key]
    {
        get => _inner.First(x => x.Key.Equals(key));
        set => _inner.Add(value);
    }

    public int Count => _inner.Count;

    public bool IsReadOnly => false;

    public bool Contains(MenuItem<TKey> item) => _inner.Contains(item);

    public void Add(MenuItem<TKey> item) => _inner.Add(item);

    public bool Remove(MenuItem<TKey> item) => _inner.Remove(item);

    public void Clear() => _inner.Clear();

    public void CopyTo(MenuItem<TKey>[] array, int arrayIndex) => _inner.CopyTo(array, arrayIndex);

    public IEnumerator<MenuItem<TKey>> GetEnumerator() => _inner.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _inner.GetEnumerator();

    #endregion
}
