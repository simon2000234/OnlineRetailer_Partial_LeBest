using ProductApi.Models;
using System.Collections.Generic;

namespace ProductApi.Services
{
    public interface IProductService
    {
        Product Add(Product entity);
        void Edit(Product entity);
        Product Get(int id);
        IEnumerable<Product> GetAll();
        void Remove(int id);
    }
}