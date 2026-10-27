// ==== DELIVERABLE 2 ====

using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace StaffOps.Services;

using Models;

public sealed class StaffService
{
    private readonly ConcurrentDictionary<Guid, StaffMember> _staffMembers = new(
        SeedStaffMembers().Select(s => new KeyValuePair<Guid, StaffMember>(s.StaffId, s))
    );

    private static IEnumerable<StaffMember> SeedStaffMembers() =>
        [
            new(Guid.CreateVersion7(), "John Doe", "john@example.com", "Doctor", "Radiology"),
            new(Guid.CreateVersion7(), "Jane Doe", "jane@example.com", "Pharmacist", "Pharmacy"),
        ];

    private ReadOnlyCollection<StaffMember> GetStaffMembers() =>
        _staffMembers.Values.ToList().AsReadOnly();

    public StaffMember GetStaffMemberById(Guid staffId) =>
        _staffMembers.TryGetValue(staffId, out StaffMember? staffMember)
            ? staffMember
            : throw new KeyNotFoundException($"No staff member found with ID: {staffId}");

    public void AddStaffMember(StaffMember newStaffMember)
    {
        if (!_staffMembers.TryAdd(newStaffMember.StaffId, newStaffMember))
        {
            throw new InvalidOperationException(
                $"Staff member with ID '{newStaffMember.StaffId}' already exists."
            );
        }
    }

    public void UpdateStaffMember(StaffMember updatedStaffMember) =>
        _staffMembers.AddOrUpdate(
            updatedStaffMember.StaffId,
            _ =>
                throw new KeyNotFoundException(
                    $"No staff member found with ID: {updatedStaffMember.StaffId}"
                ),
            (_, _) => updatedStaffMember
        );

    public void RemoveStaffMember(Guid staffId)
    {
        if (!_staffMembers.TryRemove(staffId, out _))
        {
            throw new KeyNotFoundException($"No staff member found with ID: {staffId}");
        }
    }

    public IReadOnlyCollection<StaffMember> SearchStaffMembers(string query) =>
        string.IsNullOrWhiteSpace(query)
            ? GetStaffMembers()
            : _staffMembers
                .Values.Where(s =>
                    s.FullName.Contains(query, StringComparison.OrdinalIgnoreCase)
                    || s.Email.Contains(query, StringComparison.OrdinalIgnoreCase)
                    || s.Position.Contains(query, StringComparison.OrdinalIgnoreCase)
                    || s.Unit.Contains(query, StringComparison.OrdinalIgnoreCase)
                )
                .ToList()
                .AsReadOnly();
}
