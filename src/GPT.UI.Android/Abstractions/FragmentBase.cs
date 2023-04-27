using Android.Views;
using ReactiveUI;
using System.Reactive;

namespace GPT.Abstractions;

public abstract class FragmentBase : FragmentBase<object>
{
    protected FragmentBase(int layoutId) : base(layoutId, false)
    {
    }
}

public abstract class FragmentBase<TViewModel> : ReactiveUI.AndroidX.ReactiveFragment<TViewModel>
    where TViewModel : class
{
    private readonly int layoutId;
    private readonly CompositeDisposable disposables = new();

    private FragmentBase()
    {
    }

    protected FragmentBase(int layoutId, bool initViewModel = true)
    {
        this.layoutId = layoutId;
        Activated.Subscribe(ActivatedAction);
        Deactivated.Subscribe(DeactivatedAction);

        if (initViewModel)
        {
            ViewModel = Services.Resolve<TViewModel>();
        }
    }

    /// <inheritdoc/>
    public override sealed View OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
    {
        var view = inflater.Inflate(layoutId, container, false)!;
        OnCreateView(view, savedInstanceState, disposables);
        return view!;
    }

    public virtual void OnCreateView(View view, Bundle? savedInstanceState, CompositeDisposable disposables)
    {
    }

    /// <inheritdoc/>
    public override sealed void OnViewCreated(View view, Bundle? savedInstanceState)
    {
        OnViewCreated(view, savedInstanceState, disposables);
        base.OnViewCreated(view, savedInstanceState);
    }

    public virtual void OnViewCreated(View view, Bundle? savedInstanceState, CompositeDisposable disposables)
    {
    }


    public override void OnDestroyView()
    {
        disposables.Clear();
        base.OnDestroyView();
    }

    private void ActivatedAction(Unit obj)
    {
        if (ViewModel is IActivatableViewModel vm)
        {
            vm.Activator.Activate();
        }
    }

    private void DeactivatedAction(Unit obj)
    {
        if (ViewModel is IActivatableViewModel vm)
        {
            vm.Activator.Deactivate();
        }
    }
}
