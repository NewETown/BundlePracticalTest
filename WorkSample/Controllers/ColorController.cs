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
    public class ColorController : ApiController
    {
        //private ColorsContext db = new ColorsContext();
        private Entities ent = new Entities();

        // GET api/Color
        public IQueryable<JsonColor> GetColors()
        {
            IQueryable<Color> colors = ent.Colors.AsQueryable();
            List<JsonColor> jsonColors = new List<JsonColor>();
            foreach (var color in colors)
            {
                JsonColor jsonColor = new JsonColor();
                jsonColor.color = ent.Colors.FirstOrDefault(p => p.Id == color.Id).Name;
                jsonColor.id = color.Id;
                jsonColors.Add(jsonColor);
            }
            return jsonColors.AsQueryable();
        }

        // GET api/Color/5
        [ResponseType(typeof(Color))]
        public IHttpActionResult GetColor(int id)
        {
            Color color = ent.Colors.FirstOrDefault(c => c.Id == id);
            if (color == null)
            {
                return NotFound();
            }

            return Ok(color);
        }

        // PUT api/Color/5
        public IHttpActionResult PutColor(int id, Color color)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != color.Id)
            {
                return BadRequest();
            }

            ent.Entry(color).State = System.Data.Entity.EntityState.Modified;

            try
            {
                ent.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ColorExists(id))
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

        // POST api/Color
        [ResponseType(typeof(Color))]
        public IHttpActionResult PostColor(Color color)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ent.Colors.Add(color);
            ent.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = color.Id }, color);
        }

        // DELETE api/Color/5
        [ResponseType(typeof(Color))]
        public IHttpActionResult DeleteColor(int id)
        {
            Color color = ent.Colors.FirstOrDefault(c => c.Id == id);
            if (color == null)
            {
                return NotFound();
            }

            ent.Colors.Remove(color);
            ent.SaveChanges();

            return Ok(color);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ent.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ColorExists(int id)
        {
            return ent.Colors.Count(e => e.Id == id) > 0;
        }

        public class JsonColor
        {
            public string color { get; set; }
            public int id { get; set; }
        }
    }
}