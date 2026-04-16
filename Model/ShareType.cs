namespace ShareAudit.Model;

[Flags]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1069:Enums values should not be duplicated", Justification = "Win32API doesn't play by the rules")]
public enum ShareTypes : uint
{
    Disktree = 0x00000000,
    PrintQueue = 0x00000001,
    Device = 0x00000002,
    Ipc = 0x00000003,
    ClusterFS = 0x02000000,
    ClusterSofs = 0x04000000,
    ClusterDfs = 0x08000000,
    Temporary = 0x40000000,
    Special = 0x80000000,
    SpecialDisktree = Special | Disktree,
    SpecialPrintQueue = Special | PrintQueue,
    SpecialDevice = Special | Device,
    SpecialIpc = Special | Ipc,
    SpecialClusterFS = Special | ClusterFS,
    SpecialClusterSofs = Special | ClusterSofs,
    SpecialClusterDfs = Special | ClusterDfs,
    TemporaryDisktree = Temporary | Disktree,
    TemporaryPrintQueue = Temporary | PrintQueue,
    TemporaryDevice = Temporary | Device,
    TemporaryIpc = Temporary | Ipc,
    TemporaryClusterFS = Temporary | ClusterFS,
    TemporaryClusterSofs = Temporary | ClusterSofs,
    TemporaryClusterDfs = Temporary | ClusterDfs,
    SpecialTemporaryDisktree = Special | Temporary | Disktree,
    SpecialTemporaryPrintQueue = Special | Temporary | PrintQueue,
    SpecialTemporaryDevice = Special | Temporary | Device,
    SpecialTemporaryIpc = Special | Temporary | Ipc,
    SpecialTemporaryClusterFS = Special | Temporary | ClusterFS,
    SpecialTemporaryClusterSofs = Special | Temporary | ClusterSofs,
    SpecialTemporaryClusterDfs = Special | Temporary | ClusterDfs,
    SpecialTemporary = Special | Temporary,
}