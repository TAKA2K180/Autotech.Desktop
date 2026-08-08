using Autotech.Desktop.BusinessLayer.Helpers;
using Autotech.Desktop.Core.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Autotech.Desktop.BusinessLayer.Services;

public abstract class ApiServiceBase
{
    protected static HttpClient CreateClient()
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionManager.Token);
        return client;
    }
}

public sealed class SupplierService : ApiServiceBase
{
    private static readonly string ApiUrl = $"{ApiSettings.BaseUrl}/Suppliers";

    public async Task<List<Supplier>> GetAllAsync(string? search = null)
    {
        using var client = CreateClient();
        var url = string.IsNullOrWhiteSpace(search) ? ApiUrl : $"{ApiUrl}?search={Uri.EscapeDataString(search)}";
        var response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<Supplier>>() ?? new List<Supplier>();
        }

        throw new Exception($"Failed to fetch suppliers: {await response.Content.ReadAsStringAsync()}");
    }

    public async Task SaveAsync(Supplier supplier)
    {
        using var client = CreateClient();
        var response = supplier.Id == Guid.Empty
            ? await client.PostAsJsonAsync(ApiUrl, supplier)
            : await client.PutAsJsonAsync($"{ApiUrl}/{supplier.Id}", supplier);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to save supplier: {await response.Content.ReadAsStringAsync()}");
        }
    }
}

public sealed class StockMovementService : ApiServiceBase
{
    private static readonly string ApiUrl = $"{ApiSettings.BaseUrl}/StockMovements";

    public async Task<List<StockMovement>> GetAllAsync()
    {
        using var client = CreateClient();
        var response = await client.GetAsync(ApiUrl);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<StockMovement>>() ?? new List<StockMovement>();
        }

        throw new Exception($"Failed to fetch stock movements: {await response.Content.ReadAsStringAsync()}");
    }

    public async Task AddAsync(StockMovement movement)
    {
        using var client = CreateClient();
        var response = await client.PostAsJsonAsync(ApiUrl, movement);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to save stock movement: {await response.Content.ReadAsStringAsync()}");
        }
    }
}

public sealed class ExpenseService : ApiServiceBase
{
    private static readonly string ApiUrl = $"{ApiSettings.BaseUrl}/Expenses";

    public async Task<List<Expense>> GetAllAsync()
    {
        using var client = CreateClient();
        var response = await client.GetAsync(ApiUrl);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<Expense>>() ?? new List<Expense>();
        }

        throw new Exception($"Failed to fetch expenses: {await response.Content.ReadAsStringAsync()}");
    }

    public async Task SaveAsync(Expense expense)
    {
        using var client = CreateClient();
        var response = expense.Id == Guid.Empty
            ? await client.PostAsJsonAsync(ApiUrl, expense)
            : await client.PutAsJsonAsync($"{ApiUrl}/{expense.Id}", expense);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to save expense: {await response.Content.ReadAsStringAsync()}");
        }
    }
}
