namespace Warehouse.Wpf.Auth.Interfaces
{
    public interface IAuthStore
    {
        void SaveToken(AuthToken token);
        AuthToken LoadToken();
        void ClearToken();
    }
}
