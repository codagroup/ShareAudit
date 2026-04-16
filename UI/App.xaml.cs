using ShareAudit.Service;
using ShareAudit.UI.Views;
using System.Text;
using System.Windows;

namespace ShareAudit.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
        moduleCatalog.AddModule<ServicesModule>();
        moduleCatalog.AddModule<UserInterfaceModule>();
    }

    protected override Window CreateShell()
    {
        return Container.Resolve<MainWindow>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry) { }
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        SetupExceptionHandling();
    }
    private void SetupExceptionHandling()
    {
        AppDomain.CurrentDomain.UnhandledException += (s, e) =>
        {
            ShowUnhandledException((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");
        };

        DispatcherUnhandledException += (s, e) =>
        {
            e.Handled = true;
            ShowUnhandledException(e.Exception, "Application.Current.DispatcherUnhandledException");
        };

        TaskScheduler.UnobservedTaskException += (s, e) =>
        {
            ShowUnhandledException(e.Exception, "TaskScheduler.UnobservedTaskException");
        };
    }
    private static void ShowUnhandledException(Exception e, string source)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"An application error occurred in {source}");
        sb.AppendLine();
        sb.AppendLine($"Error: {e.GetType().Name} - {e.Message}");
        sb.AppendLine($"Stack Trace:");
        sb.AppendLine(e.StackTrace);
        while (e.InnerException != null)
        {
            e = e.InnerException;

            sb.AppendLine();
            sb.AppendLine($"Error: {e.GetType().Name} - {e.Message}");
            sb.AppendLine($"Stack Trace:");
            sb.AppendLine(e.StackTrace);
        }

        new ErrorWindow(sb.ToString()).ShowDialog();

        Application.Current.Shutdown();
    }
}