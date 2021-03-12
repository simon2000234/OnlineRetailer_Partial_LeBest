using SharedModels;

namespace OrderApi.Infrastructure
{
    public interface IProductServiceGateway<T>
    {
        T Get(int id);

        bool CheckFunds(CheckPriceMessage cpm);
    }
}
