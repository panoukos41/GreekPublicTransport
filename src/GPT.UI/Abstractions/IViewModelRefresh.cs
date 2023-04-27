namespace GPT.Abstractions;

public interface IViewModelRefresh
{
    ReactiveCommand<Unit, Unit> Refresh { get; }
}
