using System.Runtime.InteropServices;

namespace ShareAudit.Interop;

[StructLayout(LayoutKind.Sequential)]
public struct ShareInfo
{
    [MarshalAs(UnmanagedType.LPWStr)]
    public string Shi1Netname;

    public uint Shi1Type;

    [MarshalAs(UnmanagedType.LPWStr)]
    public string Shi1Remark;
}
