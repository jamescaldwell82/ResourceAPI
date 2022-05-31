//Step 06. Scaffold an API controller using Resource as the Model and ResourcesContext as the Data Context.
//Have students view the browser after building the project...test the GET functionality and show the structure of the data...note that there are null values for the Category for each result.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResourceAPI.Models;

namespace ResourceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourcesController : ControllerBase
    {
        private readonly ResourcesContext _context;

        public ResourcesController(ResourcesContext context)
        {
            _context = context;
        }

        // GET: api/Resources
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Resource>>> GetResources()
        {
          if (_context.Resources == null)
          {
              return NotFound();
          }
            //Step 07. Modify the GET functionality to include Categories
            var resources = await _context.Resources.Include("Category").Select(x => new Resource()
            {
                //Assign each resource in our data set to a new Resource object for this application.
                ResourceId = x.ResourceId,
                Name = x.Name,
                Description = x.Description,
                Url = x.Url,
                LinkText = x.LinkText,
                CategoryId = x.CategoryId,
                Category = x.Category != null ? new Category()
                {
                    CategoryId = x.Category.CategoryId,
                    CategoryName = x.Category.CategoryName,
                    CategoryDescription = x.Category.CategoryDescription
                } : null
            }).ToListAsync();

            return Ok(resources);
        }


        // GET: api/Resources/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Resource>> GetResource(int id)
        {
          if (_context.Resources == null)
          {
              return NotFound();
          }
          //Step 08. Modify the code below to include Categories
            var resource = await _context.Resources.Where(x => x.ResourceId == id).Select(x => new Resource()
            {
                //Assign each resource in our data set to a new Resource object for this application.
                ResourceId = x.ResourceId,
                Name = x.Name,
                Description = x.Description,
                Url = x.Url,
                LinkText = x.LinkText,
                CategoryId = x.CategoryId,
                Category = x.Category != null ? new Category()
                {
                    CategoryId = x.Category.CategoryId,
                    CategoryName = x.Category.CategoryName,
                    CategoryDescription = x.Category.CategoryDescription
                } : null
            }).FirstOrDefaultAsync();

            if (resource == null)
            {
                return NotFound();
            }

            return resource;
        }

        // PUT: api/Resources/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResource(int id, Resource resource)
        {
            if (id != resource.ResourceId)
            {
                return BadRequest();
            }

            _context.Entry(resource).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResourceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Resources
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Resource>> PostResource(Resource resource)
        {
          if (_context.Resources == null)
          {
              return Problem("Entity set 'ResourcesContext.Resources'  is null.");
          }
          //Step 10. Modify the code below to manage how a Resource is posted
            Resource newResource = new Resource()
            {
                Name = resource.Name,
                Description = resource.Description,
                Url = resource.Url,
                LinkText = resource.LinkText,
                CategoryId = resource.CategoryId
            };

            _context.Resources.Add(newResource);
            await _context.SaveChangesAsync();

            return Ok(newResource);
        }

        // DELETE: api/Resources/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResource(int id)
        {
            if (_context.Resources == null)
            {
                return NotFound();
            }
            var resource = await _context.Resources.FindAsync(id);
            if (resource == null)
            {
                return NotFound();
            }

            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ResourceExists(int id)
        {
            return (_context.Resources?.Any(e => e.ResourceId == id)).GetValueOrDefault();
        }
    }
}
