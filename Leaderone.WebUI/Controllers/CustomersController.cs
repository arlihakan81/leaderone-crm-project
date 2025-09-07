using Leaderone.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Leaderone.WebUI.Controllers
{
    public class CustomersController(IHttpClientFactory apiClient) : Controller
    {
        private readonly IHttpClientFactory apiClient = apiClient;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var httpClient = apiClient.CreateClient("MyApiClient");

            var tenantId = User.FindFirst("TenantId")?.Value;
            var customers = await httpClient.GetFromJsonAsync<List<CustomerDTO>>($"{httpClient.BaseAddress}/customers/tenants/{tenantId}");
            return View(customers);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CustomerDTO customerDTO)
        {
            var httpClient = apiClient.CreateClient("MyApiClient");
            var response = await httpClient.PostAsJsonAsync<CustomerDTO>($"{httpClient.BaseAddress}/customers", customerDTO);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return View(customerDTO);
        }

        [HttpGet("Edit/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var httpClient = apiClient.CreateClient("MyApiClient");
            var customer = await httpClient.GetFromJsonAsync<CustomerDTO>($"{httpClient.BaseAddress}/customers/{id}");
            return View(customer);
        }

        [HttpPost("Edit/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, CustomerDTO customerDTO)
        {
            var httpClient = apiClient.CreateClient("MyApiClient");
            var response = await httpClient.PutAsJsonAsync<CustomerDTO>($"{httpClient.BaseAddress}/customers/{id}", customerDTO);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return View(customerDTO);

        }

        [HttpGet("Customers/{id:guid}/Details")]
        public async Task<IActionResult> Detail(Guid id)
        {
            var httpClient = apiClient.CreateClient("MyApiClient");
            var customer = await httpClient.GetFromJsonAsync<CustomerDTO>($"{httpClient.BaseAddress}/customers/{id}");

            return View(customer);
        }



    }
}
