using ShareAudit.UI.Views;

namespace ShareAudit.UI.ViewModels;

public class HelpViewModel : BindableBase
{
    private readonly IRegionManager _regionManager;

    public HelpViewModel(IRegionManager regionManager)
    {
        _regionManager = regionManager ?? throw new ArgumentNullException(nameof(regionManager));

        Back = new DelegateCommand(OnBack, () => true);
    }

    public DelegateCommand Back { get; }

    private void OnBack() => _regionManager.RequestNavigate("ContentRegion", nameof(WelcomeView));
}
