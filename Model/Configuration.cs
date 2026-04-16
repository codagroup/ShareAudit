using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ShareAudit.Model;
public class Configuration : INotifyPropertyChanged
{
    private Credentials _credentials = new();
    private bool _disablePortScan = false;
    private bool _disableReverseDnsLookup = false;
    private bool _isReadOnly = false;
    private string _scope = string.Empty;
    private bool _useAlternateAuthenticationMethod = true;
    private bool _useVerbatimScope = false;

    public event PropertyChangedEventHandler? PropertyChanged;

    public Credentials Credentials
    {
        get => _credentials;
        set
        {
            if (ReferenceEquals(value, _credentials))
            {
                return;
            }

            _credentials = value;
            OnPropertyChanged();
        }
    }

    public bool DisablePortScan
    {
        get => _disablePortScan;
        set
        {
            if (value == _disablePortScan)
            {
                return;
            }

            _disablePortScan = value;
            OnPropertyChanged();
        }
    }

    public bool DisableReverseDnsLookup
    {
        get => _disableReverseDnsLookup;
        set
        {
            if (value == _disableReverseDnsLookup)
            {
                return;
            }

            _disableReverseDnsLookup = value;
            OnPropertyChanged();
        }
    }

    public bool IsReadOnly
    {
        get => _isReadOnly;
        set
        {
            if (value == _isReadOnly)
            {
                return;
            }

            _isReadOnly = value;
            OnPropertyChanged();
        }
    }

    public string Scope
    {
        get => _scope;
        set
        {
            if (value == _scope)
            {
                return;
            }

            _scope = value;
            OnPropertyChanged();
        }
    }

    public bool UseAlternateAuthenticationMethod
    {
        get => _useAlternateAuthenticationMethod;
        set
        {
            if (value == _useAlternateAuthenticationMethod)
            {
                return;
            }

            _useAlternateAuthenticationMethod = value;
            OnPropertyChanged();
        }
    }

    public bool UseVerbatimScope
    {
        get => _useVerbatimScope;
        set
        {
            if (value == _useVerbatimScope)
            {
                return;
            }

            _useVerbatimScope = value;
            OnPropertyChanged();
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
