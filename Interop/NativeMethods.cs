using System.Runtime.InteropServices;

namespace ShareAudit.Interop;

public class NativeMethods
{
    [DllImport("Netapi32.dll")]
    public static extern int NetApiBufferFree(IntPtr Buffer);

    [DllImport("Netapi32.dll", CharSet = CharSet.Unicode)]
    public static extern int NetShareEnum(
        string ServerName,
        int level,
        ref IntPtr bufPtr,
        uint prefmaxlen,
        ref int entriesread,
        ref int totalentries,
        ref int resume_handle);

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, ThrowOnUnmappableChar = true, SetLastError = true)]
    public static extern bool ConvertStringSidToSid(string StringSid, out IntPtr ptrSid);

    [DllImport("advapi32.dll", EntryPoint = "GetLengthSid", CharSet = CharSet.Auto)]
    public static extern int GetLengthSid(IntPtr pSID);

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, ThrowOnUnmappableChar = true, SetLastError = true)]
    public static extern bool LookupAccountSid(
      string lpSystemName,
      [MarshalAs(UnmanagedType.LPArray)] byte[] Sid,
      System.Text.StringBuilder lpName,
      ref uint cchName,
      System.Text.StringBuilder ReferencedDomainName,
      ref uint cchReferencedDomainName,
      out SidType peUse);

    [DllImport("mpr.dll")]
    public static extern int WNetAddConnection2(ref NetResource netResource, string password, string username, uint flags);

    [DllImport("mpr.dll")]
    public static extern int WNetCancelConnection2(string name, int flags, bool force);
}