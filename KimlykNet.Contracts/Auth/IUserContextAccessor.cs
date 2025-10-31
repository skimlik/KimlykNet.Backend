namespace KimlykNet.Contracts.Auth;

public interface IUserContextAccessor
{
    bool IsAuthenticated { get; }

    UserInfo GetUserInfo();
}