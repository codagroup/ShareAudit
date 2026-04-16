using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ShareAudit.Model;

public class Host : INotifyPropertyChanged
{
    private bool _accessible;
    private string _fqdn = string.Empty;
    private string _ipAddress = string.Empty;
    private ObservableCollection<Share> _shares = [];
    private HostState _state = HostState.New;

    public event PropertyChangedEventHandler? PropertyChanged;

    public bool Accessible
    {
        get => _accessible;
        set
        {
            if (value == _accessible)
            {
                return;
            }

            _accessible = value;
            OnPropertyChanged();
        }
    }

    public string Fqdn
    {
        get => _fqdn;
        set
        {
            if (value == _fqdn)
            {
                return;
            }

            _fqdn = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Name));
        }
    }

    public string IPAddress
    {
        get => _ipAddress;
        set
        {
            if (ReferenceEquals(value, _ipAddress))
            {
                return;
            }

            _ipAddress = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Name));
        }
    }

    public string Name => string.IsNullOrEmpty(Fqdn) ? IPAddress : Fqdn;

    public ObservableCollection<Share> Shares
    {
        get => _shares;
        set
        {
            if (ReferenceEquals(value, _shares))
            {
                return;
            }

            _shares = value;
            _shares.CollectionChanged += (s, e) =>
            {
                if (e.NewItems is not null)
                {
                    foreach (Share share in e.NewItems)
                    {
                        share.PropertyChanged += (ss, se) =>
                        {
                            if (se.PropertyName is not null && se.PropertyName.Equals(nameof(Share.Accessible)))
                            {
                                Accessible = Shares.Any(x => x.Accessible);
                            }
                        };
                    }
                }
            };
            OnPropertyChanged();
        }
    }

    public HostState State
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
