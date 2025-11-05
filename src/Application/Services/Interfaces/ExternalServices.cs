using System;
using Domain.Entities;

namespace Application.Services.Interfaces
{
    // Infrastructure interfaces that need to be implemented in Infrastructure layer

    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string passwordHash);
    }

    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
        string GenerateRefreshToken();
    }
}
