using ShareAudit.Model;
using ShareAudit.Interop;

namespace ShareAudit.Service;

public class ShareInfo1
{
    internal ShareInfo1(ShareInfo shi1)
    {
        NetName = shi1.Shi1Netname;
        Type = (ShareTypes)shi1.Shi1Type;
        Remark = shi1.Shi1Remark;
    }

    public string NetName { get; }

    public string Remark { get; }

    public ShareTypes Type { get; }
}
