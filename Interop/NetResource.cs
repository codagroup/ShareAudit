using System.Runtime.InteropServices;

namespace ShareAudit.Interop;

[StructLayout(LayoutKind.Sequential)]
public struct NetResource
{
    public ResourceScope Scope;
    public ResourceType ResourceType;
    public ResourceDisplayType DisplayType;
    public int Usage;
    public string LocalName;
    public string RemoteName;
    public string Comment;
    public string Provider;
}
