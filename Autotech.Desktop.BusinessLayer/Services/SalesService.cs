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

        public async Task<List<SalesDTO>> GetAllInvoicesAsync()
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", SessionManager.Token);

            var response = await httpClient.GetAsync(_apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var invoices = await response.Content.ReadFromJsonAsync<List<SalesDTO>>();
                return invoices ?? new List<SalesDTO>();
            }

            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to retrieve invoices: {errorMessage}");
        }

        public async Task<InvoiceDetailsDTO> GetInvoiceByIdAsync(Guid invoiceId)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", SessionManager.Token);

            var response = await httpClient.GetAsync($"{_apiUrl}/{invoiceId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<InvoiceDetailsDTO>();
            }

            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to fetch invoice: {errorMessage}");
        }

        public async Task<List<PaymentHistoryDTO>> GetPaymentsBySaleIdAsync(Guid saleId)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", SessionManager.Token);

            var response = await httpClient.GetAsync($"{_apiUrl}/payments/sale/{saleId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<PaymentHistoryDTO>>();
            }

            throw new Exception($"Failed to load payment history: {await response.Content.ReadAsStringAsync()}");
        }

        public async Task AddPaymentAsync(PaymentHistoryDTO dto)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", SessionManager.Token);

            var response = await httpClient.PostAsJsonAsync($"{_apiUrl}/AddPayment", dto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception("Failed to add payment: " + error);
            }
        }
    }

    public class InvoiceResponseDTO
    {
        public Guid saleId { get; set; }
        public string invoiceNumber { get; set; }
    }
}
