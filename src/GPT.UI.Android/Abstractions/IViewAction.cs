using ReactiveUI;
using System.Reactive;

namespace GPT.Abstractions;

/// <summary>
/// View main action.
/// </summary>
public interface IViewAction
{
    ViewAction Action { get; }

    public void Deconstruct(out ViewAction action)
    {
        action = Action;
    }
}

/// <summary>
/// View secondary actions.
/// </summary>
public interface IViewActions
{
    ViewAction[] Actions { get; }
}

public sealed class ViewAction
{
    public required string Name { get; set; }

    public required int Icon { get; set; }

    public ReactiveCommand<Unit, Unit>? Command { get; set; }

    public void Deconstruct(out string name, out int icon, out ReactiveCommand<Unit, Unit>? command)
    {
        name = Name;
        icon = Icon;
        command = Command;
    }
}
