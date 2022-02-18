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
using WebApiNet.Models.Entities;
using WebApiNet.Models.OrderModels;

namespace WebApiNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class OrderController : ControllerBase
    {
        private readonly SqlContext _context;

        public OrderController(SqlContext context)
        {
            _context = context;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderOutputModel>>> GetOrders()
        {
            var items = new List<OrderOutputModel>();
            foreach (var i in await _context.Orders.Include(x => x.Customer).Include(x => x.Product).ToListAsync())
                items.Add(new OrderOutputModel(i.Id, i.CreatedTime, i.Amount, new CustomerEntity(i.Customer.Id), new ProductEntity(i.Product.Id)));
            return items;
        }

        // GET: api/Order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderOutputModel>> GetOrder(int id)
        {
            var orderEntity = await _context.Orders.Include(x => x.Customer).Include(x => x.Product).FirstOrDefaultAsync(x => x.Id == id);
            if (orderEntity == null) { return NotFound(); }
            return new OrderOutputModel(orderEntity.Id, orderEntity.CreatedTime, orderEntity.Amount, new CustomerEntity(orderEntity.Customer.Id), new ProductEntity(orderEntity.Product.Id));
        }

        // PUT: api/Order/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, OrderUpdate model)
        {
            if (id != model.Id)
            { return BadRequest(); }
            var order = await _context.Orders.FindAsync(model.Id);
            order.Amount = model.Amount;
            _context.Entry(order).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderEntityExists(id)) { return NotFound(); }
                else { throw; }
            }
            return NoContent();
        }

        // POST: api/Order
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderOutputModel>> PostOrder(OrderInputModel model)
        {
            var order = new OrderEntity(model.Amount);

            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == model.CustomerId);
            if (customer != null)
                order.CustomerId = customer.Id;
            else
                order.Customer = new CustomerEntity(model.CustomerId);

            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == model.ProductId);
            if (product != null)
                order.ProductId = product.Id;
            else
                order.Product = new ProductEntity(model.ProductId);

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetOrder", new { id = order.Id }, new OrderOutputModel(order.Id, order.CreatedTime, order.Amount, new CustomerEntity(order.Customer.Id), new ProductEntity(order.Product.Id)));
        }

        // DELETE: api/Order/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var orderEntity = await _context.Orders.FindAsync(id);
            if (orderEntity == null)
            { return NotFound(); }
            _context.Orders.Remove(orderEntity);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool OrderEntityExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
