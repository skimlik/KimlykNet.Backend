namespace KimlykNet.Backend.Infrastructure;

public interface IInitializer
{
    Task InitializeAsync(CancellationToken token);
}