using KimlykNet.Data.Abstractions.Entities;

namespace KimlykNet.Data.Abstractions.Repositories;

public interface IUserNotesRepository
{
    Task<UserNote> CreateAsync(
        string user,
        string title,
        string text,
        bool isPublic = false,
        CancellationToken cancellationToken = default);

    Task<UserNote> UpdateAsync(
        int id,
        string title,
        string text,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<UserNote> GetAsync(int id, CancellationToken cancellationToken = default);
    
    Task<UserNote> ShareAsync(int id, bool isPublic = false, CancellationToken cancellationToken = default);

    Task<ICollection<UserNote>> GetAllAsync(
        string user,
        int pageSize = 100,
        int pageIndex = 0,
        CancellationToken cancellationToken = default);
}