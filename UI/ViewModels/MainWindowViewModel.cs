namespace ShareAudit.UI.ViewModels;

public class MainWindowViewModel() : BindableBase
{
    private string _title = $"Share Audit - {typeof(MainWindowViewModel).Assembly.GetName().Version}";
    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }
}
