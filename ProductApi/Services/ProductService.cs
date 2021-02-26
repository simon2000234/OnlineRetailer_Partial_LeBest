using ProductApi.Data;
using ProductApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Services
{
    public class ProductService : IRepository<Product>
    {
        private readonly IRepository<Product> repository;
        public ProductService(IRepository<Product> repos)
        {
            repository = repos;

        }

        public Product Add(Product entity)
        {
            return repository.Add(entity);
        }

        public void Edit(Product entity)
        {
            repository.Edit(entity);
        }

        public Product Get(int id)
        {
            return repository.Get(id);
         
        }

        public IEnumerable<Product> GetAll()
        {
            return repository.GetAll();
        }

        public void Remove(int id)
        {
            repository.Remove(id);
        }
    }
}
