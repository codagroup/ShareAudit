namespace ShareAudit.Model;
public class FileEntry : FileSystemEntry
{
    private string _head = string.Empty;
    private FileEntryState _state;

    public string Head
    {
        get => _head;
        set
        {
            if (value == _head)
            {
                return;
            }

            _head = value;
            OnPropertyChanged();
        }
    }

    public FileEntryState State
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
}
