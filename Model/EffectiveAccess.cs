using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ShareAudit.Model;
public class EffectiveAccess : INotifyPropertyChanged
{
    private bool _read = false;
    private bool _write = false;

    public event PropertyChangedEventHandler? PropertyChanged;

    public bool Read
    {
        get => _read;
        set
        {
            if (value == _read)
            {
                return;
            }

            _read = value;
            OnPropertyChanged();
        }
    }

    public bool Write
    {
        get => _write;
        set
        {
            if (value == _write)
            {
                return;
            }

            _write = value;
            OnPropertyChanged();
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
