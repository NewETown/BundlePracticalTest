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
    public class SizeController : ApiController
    {
        // private SizesContext db = new SizesContext();
        private Entities ent = new Entities();

        // GET api/Size
        public IQueryable<JsonSize> GetSizes()
        {
            IQueryable<Size> sizes = ent.Sizes.AsQueryable();
            List<JsonSize> jsonColors = new List<JsonSize>();
            foreach (var size in sizes)
            {
                JsonSize jsonColor = new JsonSize();
                jsonColor.size = ent.Sizes.FirstOrDefault(p => p.Id == size.Id).Category;
                jsonColor.id = size.Id;
                jsonColors.Add(jsonColor);
            }
            return jsonColors.AsQueryable();
        }

        // GET api/Size/5
        [ResponseType(typeof(Size))]
        public IHttpActionResult GetSize(int id)
        {
            Size size = ent.Sizes.FirstOrDefault(s => s.Id == id);
            if (size == null)
            {
                return NotFound();
            }

            return Ok(size);
        }

        // PUT api/Size/5
        public IHttpActionResult PutSize(int id, Size size)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != size.Id)
            {
                return BadRequest();
            }

            ent.Entry(size).State = System.Data.Entity.EntityState.Modified;

            try
            {
                ent.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SizeExists(id))
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

        // POST api/Size
        [ResponseType(typeof(Size))]
        public IHttpActionResult PostSize(Size size)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ent.Sizes.Add(size);
            ent.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = size.Id }, size);
        }

        // DELETE api/Size/5
        [ResponseType(typeof(Size))]
        public IHttpActionResult DeleteSize(int id)
        {
            Size size = ent.Sizes.FirstOrDefault(s => s.Id == id);
            if (size == null)
            {
                return NotFound();
            }

            ent.Sizes.Remove(size);
            ent.SaveChanges();

            return Ok(size);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ent.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SizeExists(int id)
        {
            return ent.Sizes.Count(e => e.Id == id) > 0;
        }

        public class JsonSize
        {
            public string size { get; set; }
            public int id { get; set; }
        }
    }
}