namespace KimlykNet.Data.Abstractions.Entities;

public class UserNote
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public Guid NoteId { get; set; }

    public string Note { get; set; }

    public bool IsPublic { get; set; }

    public DateTimeOffset DateCreated { get; set; }

    public DateTimeOffset? DateModified { get; set; }
}