using Microsoft.Win32;
using ShareAudit.Model;
using ShareAudit.Service;
using System.Diagnostics;
using System.Windows;

namespace ShareAudit.UI.ViewModels;

public class AuditViewModel : BindableBase, INavigationAware
{
    private readonly IFileSystemStoreService _fileSystemStoreService;
    private readonly IShareAuditService _shareAuditService;
    private readonly ISmbUtilitiesService _smbUtilitiesService;
    private bool _isBusy;
    private bool _isRunning;
    private Project? _project;
    private string _projectPath = string.Empty;
    private bool _runningInitialAutomaticAudit = false;
    private object _selectedItem = new();

    public AuditViewModel(
        IFileSystemStoreService fileSystemStoreService,
        IShareAuditService shareAuditService,
        ISmbUtilitiesService smbUtilitiesService)
    {
        _fileSystemStoreService = fileSystemStoreService ?? throw new ArgumentNullException(nameof(fileSystemStoreService));
        _shareAuditService = shareAuditService ?? throw new ArgumentNullException(nameof(shareAuditService));
        _smbUtilitiesService = smbUtilitiesService ?? throw new ArgumentNullException(nameof(smbUtilitiesService));

        _shareAuditService.Started += (sender, e) =>
        {
            IsRunning = true;
            IsBusy = false;
        };
        _shareAuditService.Stopped += async (sender, e) =>
        {
            IsRunning = false;
            if (_project is null || _projectPath == string.Empty)
            {
                throw new ArgumentException("Project or path not specified.");
            }
            else
            {
                await _fileSystemStoreService.SaveProjectAsync(_project, _projectPath);

                if (_runningInitialAutomaticAudit)
                {
                    MessageBox.Show("The initial audit is now complete, you may proceed to review the results", "Initial Audit Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                    _runningInitialAutomaticAudit = false;
                }

                IsBusy = false;
            }
        };

        Export = new DelegateCommand(OnExport, CanExport).ObservesProperty(() => IsBusy).ObservesProperty(() => IsRunning);
        StartAudit = new DelegateCommand(OnStartAudit, CanStartAudit).ObservesProperty(() => IsBusy).ObservesProperty(() => IsRunning);
        StopAudit = new DelegateCommand(OnStopAudit, CanStopAudit).ObservesProperty(() => IsBusy).ObservesProperty(() => IsRunning);
        AuditFolder = new DelegateCommand(OnAuditFolder, CanAuditFolder).ObservesProperty(() => IsBusy).ObservesProperty(() => IsRunning).ObservesProperty(() => SelectedItem);
        RevealInExplorer = new DelegateCommand(OnRevealInExplorer, CanRevealInExplorer).ObservesProperty(() => IsBusy).ObservesProperty(() => SelectedItem);
    }

    public DelegateCommand AuditFolder { get; }

    public DelegateCommand Export { get; }

    public bool IsBusy
    {
        get => _isBusy;
        private set => SetProperty(ref _isBusy, value);
    }

    public bool IsRunning
    {
        get => _isRunning;
        private set => SetProperty(ref _isRunning, value);
    }

    public Project? Project
    {
        get => _project;
        private set => SetProperty(ref _project, value);
    }

    public string ProjectPath
    {
        get => _projectPath;
        private set => SetProperty(ref _projectPath, value);
    }

    public DelegateCommand RevealInExplorer { get; }

    public object SelectedItem
    {
        get => _selectedItem;
        set => SetProperty(ref _selectedItem, value);
    }

    public DelegateCommand StartAudit { get; }

    public DelegateCommand StopAudit { get; }

    public bool IsNavigationTarget(NavigationContext navigationContext) => false;

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        IsBusy = true;

        ProjectPath = navigationContext.Parameters.GetValue<string>(nameof(ProjectPath));
        Project = navigationContext.Parameters.GetValue<Project>(nameof(Project));

        if (Project.State == ProjectState.Configured && !Project.Configuration.IsReadOnly)
        {
            _runningInitialAutomaticAudit = true;
            OnStartAudit();
        }

        IsBusy = false;
    }

    private bool CanAuditFolder()
    {
        return SelectedItem is IFolderEntry &&
            (SelectedItem as IFolderEntry)!.State == FolderEntryState.EnumerationSuspended &&
            !IsBusy &&
            !IsRunning &&
            !(Project?.Configuration?.IsReadOnly ?? true);
    }

    private bool CanExport() => !IsBusy && !IsRunning;

    private bool CanRevealInExplorer()
    {
        return (SelectedItem is IFolderEntry || SelectedItem is FileEntry) &&
            !IsBusy &&
            !(Project?.Configuration?.IsReadOnly ?? true);
    }

    private bool CanStartAudit() => !IsBusy && !IsRunning && !(Project?.Configuration?.IsReadOnly ?? true);

    private bool CanStopAudit() => !IsBusy && IsRunning;

    private void OnAuditFolder()
    {
        IsBusy = true;
        if (SelectedItem is IFolderEntry && _project is not null)
        {
            (SelectedItem as IFolderEntry)!.State = FolderEntryState.EnumeratingFilesystemEntries;

            _shareAuditService.StartAudit(_project);
        }
        else
        {
            throw new ArgumentException("Selected item is invalid");
        }
    }

    private async void OnExport()
    {
        IsBusy = true;

        var dialog = new SaveFileDialog
        {
            Filter = _fileSystemStoreService.ExportFilter,
            FileName = _fileSystemStoreService.ExportDefaultFilename
        };

        if (dialog.ShowDialog() == true && _project is not null)
        {
            await _fileSystemStoreService.ExportProjectAsync(_project, dialog.FileName);
        }

        IsBusy = false;
    }

    private void OnRevealInExplorer()
    {
        IsBusy = true;

        if (SelectedItem is FileEntry)
        {
            var host = (SelectedItem as FileEntry)!.FullName.Trim('\\').Split('\\')[0];
            if (_project!.Configuration.Credentials.UseCurrentCredentials)
            {
                using (Process.Start("explorer.exe", $"/select,\"{(SelectedItem as FileEntry)!.FullName}\""))
                {
                }
            }
            else
            {
                using (var netUseConnection = _smbUtilitiesService.CreateNetUseConnection(host, _project.Configuration.Credentials.Username, _project.Configuration.Credentials.Domain, _project.Configuration.Credentials.Password))
                {
                    using (Process.Start("explorer.exe", $"/select,\"{(SelectedItem as FileEntry)!.FullName}\""))
                    {
                    }
                }
            }
        }
        else
        {
            var host = (SelectedItem as IFolderEntry)!.FullName.Trim('\\').Split('\\')[0];
            if (_project!.Configuration.Credentials.UseCurrentCredentials)
            {
                using (Process.Start("explorer.exe", $"\"{(SelectedItem as IFolderEntry)!.FullName}\""))
                {
                }
            }
            else
            {
                using (var netUseConnection = _smbUtilitiesService.CreateNetUseConnection(host, _project.Configuration.Credentials.Username, _project.Configuration.Credentials.Domain, _project.Configuration.Credentials.Password))
                {
                    using (Process.Start("explorer.exe", $"\"{(SelectedItem as IFolderEntry)!.FullName}\""))
                    {
                    }
                }
            }
        }

        IsBusy = false;
    }

    private void OnStartAudit()
    {
        IsBusy = true;
        if (_project is not null)
        {
            _shareAuditService.StartAudit(_project);
        }
        else
        {
            throw new ArgumentException("No project created.");
        }
    }

    private void OnStopAudit()
    {
        IsBusy = true;

        _shareAuditService.StopAudit();
    }
}
