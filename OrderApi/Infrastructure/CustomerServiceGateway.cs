using System;
using RestSharp;
using SharedModels;

namespace OrderApi.Infrastructure
{
    public class CustomerServiceGateway: IServiceGateway<PublicCustomer>
    {
        private Uri customerServiceBaseUrl;
        public CustomerServiceGateway(Uri customerServiceBaseUrl)
        {
            this.customerServiceBaseUrl = customerServiceBaseUrl;
        }


        public PublicCustomer Get(int id)
        {
            RestClient c = new RestClient();
            c.BaseUrl = customerServiceBaseUrl;

            var request = new RestRequest(id.ToString(), Method.GET);
            var response = c.Execute<PublicCustomer>(request);
            var customer = response.Data;
            return customer;
        }
    }
}