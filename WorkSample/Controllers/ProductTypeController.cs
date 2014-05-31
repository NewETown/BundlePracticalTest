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
    public class ProductTypeController : ApiController
    {
        // private ProductTypesContext db = new ProductTypesContext();
        private Entities ent = new Entities();

        // GET api/ProductType
        public IQueryable<JsonProductType> GetProductTypes()
        {
            IQueryable<ProductType> productTypes = ent.ProductTypes.AsQueryable();
            List<JsonProductType> jsonProductTypes = new List<JsonProductType>();
            foreach (var productType in productTypes)
            {
                JsonProductType jsonProductType = new JsonProductType();
                jsonProductType.type = ent.ProductTypes.FirstOrDefault(p => p.Id == productType.Id).Type;
                jsonProductType.id = productType.Id;
                jsonProductTypes.Add(jsonProductType);
            }
            return jsonProductTypes.AsQueryable();
        }

        // GET api/ProductType/5
        [ResponseType(typeof(ProductType))]
        public IHttpActionResult GetProductType(int id)
        {
            ProductType producttype = ent.ProductTypes.FirstOrDefault(pt => pt.Id == id);
            if (producttype == null)
            {
                return NotFound();
            }

            return Ok(producttype);
        }

        // PUT api/ProductType/5
        public IHttpActionResult PutProductType(int id, ProductType producttype)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != producttype.Id)
            {
                return BadRequest();
            }

            ent.Entry(producttype).State = System.Data.Entity.EntityState.Modified;

            try
            {
                ent.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductTypeExists(id))
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

        // POST api/ProductType
        [ResponseType(typeof(ProductType))]
        public IHttpActionResult PostProductType(ProductType producttype)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ent.ProductTypes.Add(producttype);
            ent.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = producttype.Id }, producttype);
        }

        // DELETE api/ProductType/5
        [ResponseType(typeof(ProductType))]
        public IHttpActionResult DeleteProductType(int id)
        {
            ProductType producttype = ent.ProductTypes.FirstOrDefault(pt => pt.Id == id);
            if (producttype == null)
            {
                return NotFound();
            }

            ent.ProductTypes.Remove(producttype);
            ent.SaveChanges();

            return Ok(producttype);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ent.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductTypeExists(int id)
        {
            return ent.ProductTypes.Count(e => e.Id == id) > 0;
        }

        public class JsonProductType
        {
            public int id { get; set; }
            public string type { get; set; }
        }
    }
}