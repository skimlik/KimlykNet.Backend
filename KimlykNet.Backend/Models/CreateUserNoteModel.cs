namespace KimlykNet.Backend.Models;

public class CreateUserNoteModel
{
    public string Title { get; set; }

    public string Text { get; set; }

    public bool IsPublic { get; set; }
}