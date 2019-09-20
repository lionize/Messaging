namespace TIKSN.Lionize.Messaging.Providers
{
    public interface ICachedConnectionProvider
    {
        CachedConnection GetConnection();
    }
}