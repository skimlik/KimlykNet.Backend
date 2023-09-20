using Microsoft.AspNetCore.Identity;

namespace KimlykNet.Backend.Infrastructure.Auth;

public enum UserGender
{
    Unknown,
    Male,
    Female
}

public class ApplicationUser : IdentityUser
{
    public bool FamilyMember { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string MiddleName { get; set; }

    public UserGender Gender { get; set;}
}