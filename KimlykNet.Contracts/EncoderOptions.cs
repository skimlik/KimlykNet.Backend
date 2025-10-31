namespace KimlykNet.Contracts;

public class EncoderOptions
{
    public static readonly string SectionName = "Encoder";

    public byte[] Key { get; set; }

    public byte[] IV { get; set; }
}