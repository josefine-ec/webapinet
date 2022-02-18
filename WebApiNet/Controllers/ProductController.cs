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
using WebApiNet.Models.Entities;
using WebApiNet.Models.ProductModels;

namespace WebApiNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class ProductController : ControllerBase
    {
        private readonly SqlContext _context;

        public ProductController(SqlContext context)
        {
            _context = context;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductOutputModel>>> GetProducts()
        {
            var items = new List<ProductOutputModel>();
            foreach (var i in await _context.Products.Include(x => x.Category).ToListAsync())
                items.Add(new ProductOutputModel(i.Id, i.Product, i.Description, i.Price, i.EAN, new CategoryModel(i.Category.Category)));
            return items;
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductOutputModel>> GetProduct(int id)
        {
            var productEntity = await _context.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);

            if (productEntity == null)
            {
                return NotFound();
            }

            return new ProductOutputModel(productEntity.Id, productEntity.Product, productEntity.Description, productEntity.Price, productEntity.EAN, new CategoryModel(productEntity.Category.Category));
        }

        // PUT: api/Product/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductUpdate model)
        {
            if (id != model.Id) { return BadRequest(); }
            var product = await _context.Products.FindAsync(model.Id);
            product.Product = model.Product;
            product.Description = model.Description;
            product.Price = model.Price;
            _context.Entry(product).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductEntityExists(id)) { return NotFound(); }
                else { throw; }
            }
            return NoContent();
        }

        // POST: api/Product
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductOutputModel>> PostProduct(ProductInputModel model)
        {
            var product = new ProductEntity(model.EAN, model.Product, model.Description, model.Price);
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Category == model.Category);
            if (category != null)
                product.CategoryId = category.Id;
            else
                product.Category = new CategoryEntity(model.Category);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetProduct", new { id = product.Id }, new ProductOutputModel(product.Id, product.Product, product.Description, product.Price, product.EAN, new CategoryModel(product.Category.Category)));
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var productEntity = await _context.Products.FindAsync(id);
            if (productEntity == null)
            {
                return NotFound();
            }

            _context.Products.Remove(productEntity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductEntityExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
