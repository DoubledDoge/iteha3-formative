// ==== DELIVERABLE 3 ====

namespace StaffOps.Services;

using Models;

public sealed class AdminService
{
    private readonly SystemAdmin _admin = new("admin", "password");

    public bool ValidateCredentials(string username, string password) =>
        string.Equals(username, _admin.Username, StringComparison.OrdinalIgnoreCase)
        && password == _admin.Password;
}
