using Leaderone.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Leaderone.WebUI.Controllers
{
    public class CustomersController(HttpClient apiClient) : Controller
    {
        private readonly HttpClient apiClient = apiClient;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tenantId = User.FindFirst("TenantId")?.Value;
            var customers = await apiClient.GetFromJsonAsync<List<CustomerDTO>>($"{apiClient.BaseAddress}/customers/tenants/{tenantId}");
            return View(customers);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CustomerDTO customerDTO)
        {
            var response = await apiClient.PostAsJsonAsync<CustomerDTO>($"{apiClient.BaseAddress}/customers", customerDTO);

            if(response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return View(customerDTO);
        }

        [HttpGet("Edit/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var customer = await apiClient.GetFromJsonAsync<CustomerDTO>($"{apiClient.BaseAddress}/customers/{id}");
            return View(customer);
        }

        [HttpPost("Edit/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, CustomerDTO customerDTO)
        {
            var response = await apiClient.PutAsJsonAsync<CustomerDTO>($"{apiClient.BaseAddress}/customers/{id}", customerDTO);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return View(customerDTO);

        }






    }
}
