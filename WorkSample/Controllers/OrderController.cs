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
    public class OrderController : ApiController
    {
        public class JsonOrder
        {
            public int orderId { get; set; }
            public string product { get; set; }
            public string productType { get; set; }
            public string color { get; set; }
            public string size { get; set; }
            public decimal cost { get; set; }
        }
        // private OrdersContext db = new OrdersContext();
        private Entities ent = new Entities();

        // GET api/Order
        public IQueryable<JsonOrder> GetOrders()
        {
            IQueryable<Order> orders = ent.Orders.AsQueryable();
            List<JsonOrder> jsonOrders = new List<JsonOrder>();
            foreach(var order in orders)
            {
                JsonOrder jsonOrder = new JsonOrder();
                jsonOrder.color = ent.Colors.FirstOrDefault(c => c.Id == order.Color_Id).Name;
                jsonOrder.product = ent.Products.FirstOrDefault(p => p.Id == order.Product_Id).Name;
                jsonOrder.cost = ent.Products.FirstOrDefault(p => p.Id == order.Product_Id).Cost;
                jsonOrder.productType = ent.ProductTypes.FirstOrDefault(t => t.Id == order.ProductType_Id).Type;
                jsonOrder.size = ent.Sizes.FirstOrDefault(s => s.Id == order.Size_Id).Category;
                jsonOrder.orderId = order.Id;
                jsonOrders.Add(jsonOrder);
            }
            return jsonOrders.AsQueryable();
        }

        // GET api/Order/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult GetOrder(int id)
        {
            Order order = ent.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // PUT api/Order/5
        public IHttpActionResult PutOrder(int id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.Id)
            {
                return BadRequest();
            }

            ent.Entry(order).State = System.Data.Entity.EntityState.Modified;

            try
            {
                ent.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST api/Order
        [ResponseType(typeof(Order))]
        public IHttpActionResult PostOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ent.Orders.Add(order);
            ent.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = order.Id }, order);
        }

        // DELETE api/Order/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult DeleteOrder(int id)
        {
            //Order order = db.Orders.Find(id);
            Order order = ent.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            ent.Orders.Remove(order);
            ent.SaveChanges();

            return Ok(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ent.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return ent.Orders.Count(e => e.Id == id) > 0;
        }
    }
}