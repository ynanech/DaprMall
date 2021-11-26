using Dapr.Client;
using DaprMall.Cart.Models;

namespace DaprMall.Cart.Services
{
    public interface IProductService
    {
        Task<AjaxResult<IEnumerable<Product>>?> GetProductsAsync();
        Task<AjaxResult<Product>?> GetProductAsync(int id);
        Task<AjaxResult?> DeductionAsync(StockDto input);
    }

    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService()
        {
            _httpClient = DaprClient.CreateInvokeHttpClient("productservice");
        }


        public async Task<AjaxResult<IEnumerable<Product>>?> GetProductsAsync()
        {
            return await _httpClient.GetFromJsonAsync<AjaxResult<IEnumerable<Product>>>("api/v1/Product");
        }

        public async Task<AjaxResult<Product>?> GetProductAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<AjaxResult<Product>>($"api/v1/Product/{id}");
        }

        public async Task<AjaxResult?> DeductionAsync(StockDto input)
        {
            var response = await _httpClient.PostAsJsonAsync("api/v1/Product", input);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AjaxResult>();
        }
    }


    public class StockDto
    {
        public StockDto(int productId, int num)
        {
            ProductId = productId;
            Num = num;
        }

        public int ProductId { get; set; }
        public int Num { get; set; }
    }
}
