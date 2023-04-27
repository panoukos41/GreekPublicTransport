using P41.Navigation;

namespace GPT.Bus;

/// <summary>
/// A ViewModel that represents a collection of lines.
/// </summary>
public class LineCollectionViewModel : ViewModelBase, IViewModelTitle
{
    private readonly SourceList<Line> _lines = new();

    public LineCollectionViewModel(IBusClient client, IStorage storage)
    {
        // todo: In the future implement storage search so lines can be queried and there is no need for the key.
        GetLines = ReactiveCommand.CreateFromObservable<None, Line[]>(_ =>
            client.GetLines().Store(storage));

        GetLines.Subscribe(lines =>
        {
            _lines.Clear();
            _lines.AddRange(lines);
        });

        var filter = this
            .WhenAnyValue(vm => vm.Search)
            .Throttle(TimeSpan.FromMilliseconds(300))
            .Select<string?, Func<Line, bool>>(
                static search => string.IsNullOrWhiteSpace(search)
                ? _ => true
                : line => line.Description.ToString().Contains(search));

        Lines = _lines
            .Connect()
            .Filter(filter)
            .RefCount();

        this.WhenNavigatedTo((r, d) =>
        {
            GetLines.Invoke();
        });
    }

    public string? Title { get; } = "Home";

    [Reactive]
    public string? Search { get; set; }

    public ReactiveCommand<None, Line[]> GetLines { get; }

    public IObservable<IChangeSet<Line>> Lines { get; }
}
