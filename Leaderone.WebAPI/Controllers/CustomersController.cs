using Leaderone.Application.DTOs;
using Leaderone.Application.Interfaces;
using Leaderone.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Leaderone.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomersController(ICustomerRepository repository) : ControllerBase
    {
        private readonly ICustomerRepository repository = repository;

        [HttpGet("tenants/{id:guid}")]
        public async Task<ActionResult<List<CustomerDTO>>> GetAllCustomersAsync(Guid id)
        {
            var customers = await repository.GetAllCustomersAsync(id);
            var result = customers?.Select(
                c => new CustomerDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    Address = c.Address,
                    City = c.City,
                    Country = c.Country,
                    Phone = c.Phone,
                    Status = c.Status,
                    TenantId = c.TenantId,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                }
            );
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CustomerDTO>> GetCustomerByIdAsync(Guid id)
        {
            var customer = await repository.GetCustomerByIdAsync(id);
            var result = new CustomerDTO
            {
                Id = customer!.Id,
                Name = customer.Name,
                Email = customer.Email,
                Address = customer.Address,
                Phone = customer.Phone,
                Status = customer.Status,
                TenantId = customer.TenantId,
                City = customer.City,
                CreatedAt = customer.CreatedAt,
                UpdatedAt = customer.UpdatedAt
            };
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync([FromBody] CustomerDTO customerDTO)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError(null!, "Customer informations are required");
                return BadRequest("Customer informations are required...");
            }

            if(await repository.IsExistsCustomerEmailAsync(customerDTO.Email))
            {
                ModelState.AddModelError(customerDTO.Email, "Customer email already exists");
                return BadRequest("Customer email already exists");
            }

            if (await repository.IsExistsCustomerPhoneAsync(customerDTO.Phone))
            {
                ModelState.AddModelError(customerDTO.Phone, "Customer phone number already exists");
                return BadRequest("Customer phone number already exists");
            }
            var customer = new Customer
            {
                Name = customerDTO.Name,
                Email = customerDTO.Email,
                Address = customerDTO.Address,
                Phone = customerDTO.Phone,
                Status = customerDTO.Status,
                City = customerDTO.City,
                Country = customerDTO.Country,
                TenantId = customerDTO.TenantId,
                CreatedAt = DateTime.Now
            };
            await repository.AddCustomerAsync(customer);
            return Ok("New customer has been added");
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCustomerAsync(Guid id, [FromBody] CustomerDTO customerDTO)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError(null!, "Customer informations are required");
                return BadRequest("Customer informations are required");
            }

            if (await repository.IsExistsCustomerEmailAsync(id, customerDTO.Email))
            {
                ModelState.AddModelError(customerDTO.Email, "Customer email already exists");
                return BadRequest("Customer email already exists");
            }

            if (await repository.IsExistsCustomerPhoneAsync(id, customerDTO.Phone))
            {
                ModelState.AddModelError(customerDTO.Phone, "Customer phone number already exists");
                return BadRequest("Customer phone number already exists");
            }

            var customer = await repository.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                ModelState.AddModelError(null!, "Customer not found");
                return NotFound("Customer not found");
            }

            customer.Name = customerDTO.Name;
            customer.Email = customerDTO.Email;
            customer.Address = customerDTO.Address;
            customer.Phone = customerDTO.Phone;
            customer.Status = customerDTO.Status;
            customer.City = customerDTO.City;
            customer.UpdatedAt = DateTime.Now;
            await repository.UpdateCustomerAsync(customer);
            return Ok("Customer has been updated");
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCustomerAsync(Guid id)
        {
            var customer = await repository.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                ModelState.AddModelError(null!, "Customer not found");
                return NotFound("Customer not found");
            }
            await repository.DeleteCustomerAsync(id);
            return Ok("Customer has been deleted");
        }

    }
}
