namespace KimlykNet.Backend.Infrastructure.Auth;

public interface IUserContextAccessor
{
    bool IsAuthenticated { get; }

    UserInfo GetUserInfo();
}