using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Data;
using ProductApi.Models;
using ProductApi.Services;
using PublicModels;

namespace ProductApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductsController( IProductService productService)
        {
           
            this.productService = productService;

        }

        // GET products
        [HttpGet]
        public IEnumerable<PublicProduct> Get()
        {
            var privateList = productService.GetAll();
            List<PublicProduct> publicList = new List<PublicProduct>();
            foreach (var prod in privateList)
            {
                PublicProduct pp = new PublicProduct
                {
                    Name = prod.Name, Category = prod.Category, Id = prod.Id, ItemsInStock = prod.ItemsInStock,
                    ItemsReserved = prod.ItemsReserved, Price = prod.Price
                };
                publicList.Add(pp);
            }

            return publicList;
        }

        // GET products/5
        [HttpGet("{id}", Name="GetProduct")]
        public IActionResult Get(int id)
        {
           var prod = productService.Get(id);

            if (prod == null)
            {
                return NotFound();
            }

            PublicProduct pp = new PublicProduct
            {
                Name = prod.Name, Category = prod.Category, Id = prod.Id, ItemsInStock = prod.ItemsInStock,
                ItemsReserved = prod.ItemsReserved, Price = prod.Price
            };
            return new ObjectResult(pp);
        }

        // POST products
        [HttpPost]
        public IActionResult Post([FromBody]PublicProduct pubProduct)
        {
            if (pubProduct == null)
            {
                return BadRequest();
            }

            Product product = new Product
            {
                Category = pubProduct.Category, Name = pubProduct.Name, Id = pubProduct.Id,
                ItemsInStock = pubProduct.ItemsInStock, ItemsReserved = pubProduct.ItemsReserved,
                Price = pubProduct.Price
            };

            var newProduct = productService.Add(product);

            return CreatedAtRoute("GetProduct", new { id = newProduct.Id }, newProduct);
        }

        // PUT products/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Product pubProduct)
        {

            if (pubProduct == null || pubProduct.Id != id)
            {
                return BadRequest();
            }

            var modifiedProduct = productService.Get(id);

            if (modifiedProduct == null)
            {
                return NotFound();
            }

            modifiedProduct.Name = pubProduct.Name;
            modifiedProduct.Price = pubProduct.Price;
            modifiedProduct.ItemsInStock = pubProduct.ItemsInStock;
            modifiedProduct.ItemsReserved = pubProduct.ItemsReserved;

            productService.Edit(modifiedProduct);
            return new NoContentResult();
        }

        // DELETE products/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (productService.Get(id) == null)
            {
                return NotFound();
            }

            productService.Remove(id);
            return new NoContentResult();
        }
    }
}
