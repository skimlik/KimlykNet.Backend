using KimlykNet.Contracts.Auth;
using KimlykNet.Data.Abstractions.Entities;
using KimlykNet.Data.Abstractions.Repositories;

using Microsoft.EntityFrameworkCore;

namespace KimlykNet.Data.Repositories;

internal sealed class SecretMessageRepository(DataContext context) : ISecretMessageRepository
{
    public async Task<Guid> CreateSecretAsync(
        string message,
        string currentUser,
        CancellationToken cancellationToken = default)
    {
        var entity = new SecretEntity
        {
            Id = Guid.NewGuid(),
            Secret = message,
            CreatedBy = currentUser,
            CreatedOn = DateTimeOffset.UtcNow,
        };

        context.Entry(entity).State = EntityState.Added;
        await context.SecretMessages.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task<SecretEntity> GetSecretAsync(
        Guid id,
        string currentUser,
        CancellationToken cancellationToken = default)
    {
        var secret = await context.SecretMessages.SingleOrDefaultAsync(s => s.Id == id && s.ReceivedOn == null, cancellationToken);
        if (secret is null)
        {
            return null;
        }

        if (secret.SentTo is not null)
        {
            if (currentUser != secret.SentTo)
            {
                return null;
            }
        }

        context.SecretMessages.Remove(secret);
        context.Entry(secret).State = EntityState.Deleted;
        await context.SaveChangesAsync(cancellationToken);
        return secret;
    }
}