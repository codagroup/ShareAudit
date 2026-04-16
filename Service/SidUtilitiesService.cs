using ShareAudit.Interop;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Text;

namespace ShareAudit.Service;

public class SidUtilitiesService : ISidUtilitiesService
{
    private readonly ConcurrentDictionary<string, string> _sidCache = [];

    public string SidStringToAccountName(string host, string sid)
    {
        const int ERROR_SUCCESS = 0;
        const int ERROR_INSUFFICIENT_BUFFER = 122;

        if (_sidCache.TryGetValue(sid, out string? value))
        {
            return value;
        }
        else
        {
            var name = new StringBuilder();
            uint cchName = (uint)name.Capacity;
            var referencedDomainName = new StringBuilder();
            uint cchReferencedDomainName = (uint)referencedDomainName.Capacity;

            byte[]? sidByteArray;
            NativeMethods.ConvertStringSidToSid(sid, out nint sid_ptr);
            var size = (int)NativeMethods.GetLengthSid(sid_ptr);
            sidByteArray = new byte[size];
            Marshal.Copy(sid_ptr, sidByteArray, 0, size);
            Marshal.FreeHGlobal(sid_ptr);

            var result = ERROR_SUCCESS;
            if (!NativeMethods.LookupAccountSid(host, sidByteArray, name, ref cchName, referencedDomainName, ref cchReferencedDomainName, out _))
            {
                result = Marshal.GetLastWin32Error();
                if (result == ERROR_INSUFFICIENT_BUFFER)
                {
                    name.EnsureCapacity((int)cchName);
                    referencedDomainName.EnsureCapacity((int)cchReferencedDomainName);
                    result = ERROR_SUCCESS;
                    if (!NativeMethods.LookupAccountSid(host, sidByteArray, name, ref cchName, referencedDomainName, ref cchReferencedDomainName, out _))
                    {
                        result = Marshal.GetLastWin32Error();
                    }
                }
            }

            if (result == ERROR_SUCCESS)
            {
                if (!string.IsNullOrEmpty(referencedDomainName.ToString()))
                {
                    _sidCache.TryAdd(sid, referencedDomainName.ToString() + @"\" + name.ToString());
                }
                else
                {
                    _sidCache.TryAdd(sid, name.ToString());
                }
            }
            else
            {
                _sidCache.TryAdd(sid, sid);
            }
        }

        return _sidCache[sid];
    }
}
