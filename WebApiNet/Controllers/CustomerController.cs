#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiNet;
using WebApiNet.Filter;
using WebApiNet.Models;
using WebApiNet.Models.CustomerModels;
using WebApiNet.Models.Entities;

namespace WebApiNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class CustomerController : ControllerBase
    {
        private readonly SqlContext _context;

        public CustomerController(SqlContext context)
        {
            _context = context;
        }

        // GET: api/Customer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerOutputModel>>> GetCustomers()
        {
            var items = new List<CustomerOutputModel>();
            foreach (var i in await _context.Customers.Include(x => x.Address).ToListAsync())
                items.Add(new CustomerOutputModel(i.Id, i.FirstName, i.LastName, i.Email, new AddressModel(i.Address.Street, i.Address.ZipCode, i.Address.City, i.Address.Country)));
            return items;
        }

        // GET: api/Customer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerOutputModel>> GetCustomer(int id)
        {
            var customerEntity = await _context.Customers.Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == id);
            if (customerEntity == null)
            { return NotFound(); }
            return new CustomerOutputModel(customerEntity.Id, customerEntity.FirstName, customerEntity.LastName, customerEntity.Email, new AddressModel(customerEntity.Address.Street, customerEntity.Address.ZipCode, customerEntity.Address.City, customerEntity.Address.Country));
        }

        // PUT: api/Customer/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, CustomerUpdate model)
        {
            if (id != model.Id)
            { return BadRequest(); }
            var customer = await _context.Customers.FindAsync(model.Id);
            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.Email = model.Email;
            customer.Password = model.Password;
            _context.Entry(customer).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerEntityExists(id)) { return NotFound(); }
                else { throw; }
            }
            return NoContent();
        }

        // POST: api/Customer
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CustomerOutputModel>> PostCustomer(CustomerInputModel model)
        {
            var customer = new CustomerEntity(model.FirstName, model.LastName, model.Email, model.Password);
            var address = await _context.Addresses.FirstOrDefaultAsync(x => x.Street == model.Street && x.ZipCode == model.ZipCode && x.City == model.City && x.Country == model.Country);
            if (address != null)
                customer.AddressId = address.Id;
            else
                customer.Address = new AddressEntity(model.Street, model.ZipCode, model.City, model.Country);

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetCustomer", new { id = customer.Id }, new CustomerOutputModel(customer.Id, customer.FirstName, customer.LastName, customer.Email, new AddressModel(customer.Address.Street, customer.Address.ZipCode, customer.Address.City, customer.Address.Country)));
        }

        // DELETE: api/Customer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerEntity(int id)
        {
            var customerEntity = await _context.Customers.FindAsync(id);
            if (customerEntity == null)
            { return NotFound(); }
            _context.Customers.Remove(customerEntity);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool CustomerEntityExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
