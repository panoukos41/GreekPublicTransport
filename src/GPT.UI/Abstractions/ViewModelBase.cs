using P41.Navigation;

namespace GPT.Abstractions;

public abstract class ViewModelBase : ReactiveObject,
    IActivatableViewModel,
    INavigatableViewModel
{
    /// <inheritdoc/>
    public ViewModelNavigator Navigator { get; } = new();

    /// <inheritdoc/>
    public ViewModelActivator Activator { get; } = new();
}
