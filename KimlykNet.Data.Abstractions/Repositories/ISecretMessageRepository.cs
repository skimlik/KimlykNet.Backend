using KimlykNet.Data.Abstractions.Entities;

namespace KimlykNet.Data.Abstractions.Repositories;

public interface ISecretMessageRepository
{
    Task<Guid> CreateSecretAsync(
        string message,
        string currentUser,
        CancellationToken cancellationToken = default);

    Task<SecretEntity> GetSecretAsync(
        Guid id,
        string currentUser,
        CancellationToken cancellationToken = default);
}