using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ShareAudit.Model;

public class Project : INotifyPropertyChanged
{
    private Configuration _configuration = new();
    private ObservableCollection<Host> _hosts = [];
    private ProjectState _state = ProjectState.New;

    public event PropertyChangedEventHandler? PropertyChanged;

    public Configuration Configuration
    {
        get => _configuration;
        set
        {
            if (ReferenceEquals(value, _configuration))
            {
                return;
            }

            _configuration = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Host> Hosts
    {
        get => _hosts;
        set
        {
            if (ReferenceEquals(value, _hosts))
            {
                return;
            }

            _hosts = value;
            OnPropertyChanged();
        }
    }

    public ProjectState State
    {
        get => _state;
        set
        {
            if (value == _state)
            {
                return;
            }

            _state = value;
            OnPropertyChanged();
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
