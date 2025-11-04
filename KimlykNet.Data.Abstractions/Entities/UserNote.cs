namespace KimlykNet.Data.Abstractions.Entities;

public class UserNote
{
    public int Id { get; set; }

    public string User { get; set; }

    public string Note { get; set; }
    
    public string Title { get; set; }

    public bool IsPublic { get; set; }

    public DateTimeOffset DateCreated { get; set; }

    public DateTimeOffset? DateModified { get; set; }
}