namespace Warehouse.Wpf.Auth
{
    public interface IAuthStore
    {
        void SaveToken(AuthToken token);
        AuthToken LoadToken();
        void ClearToken();
    }
}
