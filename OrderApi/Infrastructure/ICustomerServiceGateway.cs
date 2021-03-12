namespace OrderApi.Infrastructure
{
    public interface ICustomerServiceGateway<T>
    {
        T Get(int id);
    }
}