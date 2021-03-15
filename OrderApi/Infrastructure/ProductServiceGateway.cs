using System;
using RestSharp;
using SharedModels;

namespace OrderApi.Infrastructure
{
    public class ProductServiceGateway : IProductServiceGateway<ProductDto>
    {
        Uri productServiceBaseUrl;

        public ProductServiceGateway(Uri baseUrl)
        {
            productServiceBaseUrl = baseUrl;
        }

        public ProductDto Get(int id)
        {
            RestClient c = new RestClient();
            c.BaseUrl = productServiceBaseUrl;

            var request = new RestRequest(id.ToString(), Method.GET);
            var response = c.Execute<ProductDto>(request);
            var orderedProduct = response.Data;
            return orderedProduct;
        }

        public decimal CheckFunds(CheckPriceMessage cpm)
        {
            RestClient c = new RestClient();
            c.BaseUrl = productServiceBaseUrl;
            var request = new RestRequest("CheckPrice", Method.POST, DataFormat.Json);
            request.AddJsonBody(cpm);
            var response = c.Execute<decimal>(request);
            return response.Data;
        }
    }
}
