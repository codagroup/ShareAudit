namespace ShareAudit.Interop;

/// <summary>
/// A representation of <see href="https://learn.microsoft.com/en-us/windows/win32/api/winnt/ne-winnt-sid_name_use" />
/// </summary>
public enum SidType
{
    SidTypeNone,
    SidTypeUser = 1,
    SidTypeGroup,
    SidTypeDomain,
    SidTypeAlias,
    SidTypeWellKnownGroup,
    SidTypeDeletedAccount,
    SidTypeInvalid,
    SidTypeUnknown,
    SidTypeComputer,
    SidTypeLabel,
    SidTypeLogonSession,
}
