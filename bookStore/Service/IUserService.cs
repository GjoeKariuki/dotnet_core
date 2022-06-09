namespace bookStore.Service
{
    public interface IUserService
    {
        string GetUserId();
        bool isAuthenticated();
    }
}