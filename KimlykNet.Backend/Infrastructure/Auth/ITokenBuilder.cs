﻿namespace KimlykNet.Backend.Infrastructure.Auth;

public interface ITokenBuilder
{
    Task<SecurityToken> CreateAsync(
        string email,
        string password,
        string clientId,
        CancellationToken token = default);
}