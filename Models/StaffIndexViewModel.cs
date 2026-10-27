// ==== DELIVERABLE 3 ====

namespace StaffOps.Models;

public sealed record StaffIndexViewModel(
    IReadOnlyCollection<StaffMember> StaffMembers,
    Guid? EditingStaffId,
    StaffMember? FailedSubmission,
    string? SearchQuery,
    bool ShowForm
);
