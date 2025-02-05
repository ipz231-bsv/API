using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private static List<Item> items = new List<Item>
        {
            new Item { Id = 1, Name = "Item1", Description = "Description1" },
            new Item { Id = 2, Name = "Item2", Description = "Description2" }
        };

        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<Item>> Get()
        {
            return items;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<Item> Post([FromBody] Item newItem)
        {
            newItem.Id = items.Count + 1;
            items.Add(newItem);
            return CreatedAtAction(nameof(Get), new { id = newItem.Id }, newItem);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var item = items.Find(i => i.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            items.Remove(item);
            return NoContent();
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin,User")]
        public ActionResult<Item> Patch(int id, [FromBody] Item updatedItem)
        {
            var item = items.Find(i => i.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            item.Name = updatedItem.Name ?? item.Name;
            item.Description = updatedItem.Description ?? item.Description;
            return item;
        }
    }
}
