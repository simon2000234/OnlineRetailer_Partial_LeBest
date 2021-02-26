namespace CustomerAPI.Data
{
    public interface IDbInitializer
    {
        void Initialize(CustomerApiContext context);
    }
}
