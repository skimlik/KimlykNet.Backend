namespace KimlykNet.Data.Abstractions;

public interface IIdEncoder
{
    string Encode(string plainText);
    string Decode(string secretValue);
    string? SafeDecode(string secretValue);
}