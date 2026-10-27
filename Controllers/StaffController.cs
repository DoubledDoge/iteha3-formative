// ==== DELIVERABLE 3 ====

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StaffOps.Controllers;

using Models;
using Services;

[Authorize]
public sealed class StaffController(StaffService staffService) : Controller
{
    // Listing
    [HttpGet]
    public IActionResult Index(Guid? editId = null, string? search = null, bool add = false) =>
        View("Index", BuildViewModel(editId, search: search, isAdding: add));

    [HttpPost]
    public IActionResult Create(StaffMember staffMember)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", BuildViewModel(null, staffMember));
        }

        staffService.AddStaffMember(staffMember with { StaffId = Guid.CreateVersion7() });
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Edit(StaffMember staffMember)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", BuildViewModel(staffMember.StaffId, staffMember));
        }

        staffService.UpdateStaffMember(staffMember);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Delete(Guid id)
    {
        staffService.RemoveStaffMember(id);
        return RedirectToAction(nameof(Index));
    }

    private StaffMember? FindEditingStaff(Guid? editId)
    {
        if (editId is not { } id)
            return null;

        try
        {
            return staffService.GetStaffMemberById(id);
        }
        catch (KeyNotFoundException)
        {
            return null;
        }
    }

    private StaffIndexViewModel BuildViewModel(
        Guid? editId,
        StaffMember? failedSubmission = null,
        string? search = null,
        bool isAdding = false
    )
    {
        Guid? effectiveEditId = isAdding ? null : editId;
        StaffMember? resolvedSubmission = failedSubmission ?? FindEditingStaff(effectiveEditId);
        bool showForm = isAdding || effectiveEditId.HasValue || failedSubmission is not null;

        return new(
            staffService.SearchStaffMembers(search ?? string.Empty),
            effectiveEditId,
            resolvedSubmission,
            search,
            showForm
        );
    }
}
