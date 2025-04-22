using Autotech.Desktop.Core.DTO;
using Autotech.Desktop.BusinessLayer.Helpers;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Autotech.Desktop.BusinessLayer.DTO;

namespace Autotech.Desktop.BusinessLayer.Services
{
    public class SalesService
    {
        private readonly string _apiUrl = "https://localhost:7106/api/v1/Sales";

        public async Task<(Guid saleId, string invoiceNumber)> CreateInvoiceAsync(InvoiceDTO invoice)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", SessionManager.Token);

            var response = await httpClient.PostAsJsonAsync(_apiUrl, invoice);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<InvoiceResponseDTO>();
                return (result.saleId, result.invoiceNumber);
            }

            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new Exception($"Invoice creation failed: {errorMessage}");
        }
    }

    public class InvoiceResponseDTO
    {
        public Guid saleId { get; set; }
        public string invoiceNumber { get; set; }
    }
}
