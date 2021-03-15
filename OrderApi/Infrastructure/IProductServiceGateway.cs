using SharedModels;

namespace OrderApi.Infrastructure
{
    public interface IProductServiceGateway<T>
    {
        T Get(int id);

        decimal CheckFunds(CheckPriceMessage cpm);
    }
}
