using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WorkSample.Models;

namespace WorkSample.Controllers
{
    public class ProductController : ApiController
    {
        private Entities ent = new Entities();

        // GET api/Product
        public IQueryable<JsonProduct> GetProducts()
        {
            IQueryable<Product> products = ent.Products.AsQueryable();
            List<JsonProduct> jsonProducts = new List<JsonProduct>();
            foreach (var product in products)
            {
                JsonProduct jsonProduct = new JsonProduct();
                var prod = ent.Products.FirstOrDefault(p => p.Id == product.Id);
                jsonProduct.name = prod.Name;
                jsonProduct.cost = prod.Cost;
                jsonProduct.id = product.Id;
                jsonProducts.Add(jsonProduct);
            }
            return jsonProducts.AsQueryable();
        }

        // GET api/Product/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(int id)
        {
            Product product = ent.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT api/Product/5
        public IHttpActionResult PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            ent.Entry(product).State = System.Data.Entity.EntityState.Modified;

            try
            {
                ent.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Product
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ent.Products.Add(product);
            ent.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);
        }

        // DELETE api/Product/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = ent.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            ent.Products.Remove(product);
            ent.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ent.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return ent.Products.Count(e => e.Id == id) > 0;
        }

        public class JsonProduct
        {
            public int id { get; set; }
            public string name { get; set; }
            public decimal cost { get; set; }
        }
    }
}