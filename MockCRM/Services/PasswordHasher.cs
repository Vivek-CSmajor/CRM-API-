namespace MockCRM.Services;
using BCrypt.Net;
public static class PasswordHasher
{
    public static string HashPassword(string rawPassword) 
    {
        return BCrypt.HashPassword(rawPassword);
    }

    public static bool VerifyHashedPassword(string rawPassword, string hashedPassword)
    {
        return BCrypt.Verify(rawPassword, hashedPassword);
    }
}