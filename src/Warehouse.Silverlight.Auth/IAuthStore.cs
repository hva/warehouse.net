namespace Warehouse.Silverlight.Auth
{
    public interface IAuthStore
    {
        void Save(AuthToken token);
        AuthToken Load();
        void Clear();
    }
}
