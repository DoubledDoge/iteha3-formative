// ==== DELIVERABLE 1 ====

using System.ComponentModel.DataAnnotations;

namespace StaffOps.Models;

public sealed record StaffMember(
    Guid StaffId,
    [Required, StringLength(100)] string FullName,
    [Required, EmailAddress] string Email,
    [Required, StringLength(100)] string Position,
    [Required, StringLength(100)] string Unit
);
