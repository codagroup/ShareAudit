using ShareAudit.UI.Views;

namespace ShareAudit.UI;

public class UserInterfaceModule : IModule
{
    public void OnInitialized(IContainerProvider containerProvider)
    {
        var regionManager = containerProvider.Resolve<IRegionManager>();
        var region = regionManager.Regions["ContentRegion"];

        var welcomeView = containerProvider.Resolve<WelcomeView>();
        region.Add(welcomeView);
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<HelpView>();
        containerRegistry.RegisterForNavigation<ConfigurationView>();
        containerRegistry.RegisterForNavigation<ImportScopeView>();
        containerRegistry.RegisterForNavigation<AuditView>();
    }
}