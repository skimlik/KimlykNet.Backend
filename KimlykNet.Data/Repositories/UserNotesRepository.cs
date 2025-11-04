using System.Data;

using KimlykNet.Data.Abstractions.Entities;
using KimlykNet.Data.Abstractions.Repositories;

using Microsoft.EntityFrameworkCore;

namespace KimlykNet.Data.Repositories;

internal sealed class UserNotesRepository(DataContext context) : IUserNotesRepository
{
    public async Task<UserNote> CreateAsync(
        string user,
        string title,
        string text,
        bool isPublic = false,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(title);

        var note = new UserNote
        {
            User = user,
            Title = title,
            Note = text,
            IsPublic = isPublic,
            DateCreated = DateTimeOffset.UtcNow
        };
        
        await context.UserNotes.AddAsync(note, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return note;
    }

    public async Task<UserNote> UpdateAsync(
        int id,
        string title,
        string text,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(title);
        var note = await context.UserNotes.SingleOrDefaultAsync(r => r.Id == id, cancellationToken);
        if (note is null)
        {
            throw new DataException("UserNote not found");
        }
        
        note.Title = title;
        note.Note = text;
        note.DateModified = DateTimeOffset.UtcNow;
        context.Entry(note).State = EntityState.Modified;
        await context.SaveChangesAsync(cancellationToken);
        return note;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var note = await context.UserNotes.SingleOrDefaultAsync(r => r.Id == id, cancellationToken);
        if (note is not null)
        {
            context.Entry(note).State = EntityState.Deleted;
            context.UserNotes.Remove(note);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<UserNote> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var note = await context.UserNotes.SingleOrDefaultAsync(r => r.Id == id, cancellationToken);
        return note;
    }

    public async Task<UserNote> ShareAsync(int id, bool isPublic = false, CancellationToken cancellationToken = default)
    {
        var note = await context.UserNotes.SingleOrDefaultAsync(r => r.Id == id, cancellationToken);
        if (note is null)
        {
            throw new DataException("UserNote not found");
        }
        
        note.IsPublic = isPublic;
        note.DateModified = DateTimeOffset.UtcNow;
        context.Entry(note).State = EntityState.Modified;
        await context.SaveChangesAsync(cancellationToken);
        return note;
    }

    public async Task<ICollection<UserNote>> GetAllAsync(
        string user,
        int pageSize = 100,
        int pageIndex = 0,
        CancellationToken cancellationToken = default)
    {
        var data = await context.UserNotes
            .Where(r => r.User == user)
            .OrderBy(r => r.DateCreated)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);

        return data;
    }
}