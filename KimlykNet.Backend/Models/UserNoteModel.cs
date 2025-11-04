namespace KimlykNet.Backend.Models;

public class UserNoteModel
{
    public string NoteId { get; set; }

    public string Title { get; set; }

    public string Text { get; set; }

    public bool IsPublic { get; set; }

    public DateTimeOffset CreatedDate { get; set; }

    public DateTimeOffset? ModifiedDate { get; set; }
}