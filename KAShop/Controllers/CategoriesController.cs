using Azure.Core;
using KAShop.Data;
using KAShop.DTO.Requist;
using KAShop.DTO.Response;
using KAShop.Models;
using KAShop.Models.Category;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace KAShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ApplicationDbContext _context;
        public CategoriesController(IStringLocalizer<SharedResource> localizer  , ApplicationDbContext context)
        {
            _localizer = localizer;
            _context = context; 
        }



        [HttpPost("")]
        public IActionResult Create([FromBody] CategoryRequistDTO requist)
        {
            _context.categories.Add(requist.Adapt<Category>());
            _context.SaveChanges();
            return Ok(new {message = _localizer["added-success"], data = requist.Adapt<CategoryResponseDTO>() });
        }
        [HttpGet("")]
        public IActionResult GetAll()
        {
            var categories = _context.categories.OrderByDescending(c => c.CreatedAt).ToList().Adapt<List<CategoryResponseDTO>>();

            return Ok(new {message = _localizer["succsess"], data = categories });
        }
        [HttpGet("forUser")]
        public IActionResult Index([FromQuery]string lang = "ar")
        {
           var categories = _context.categories.Include(c=> c.categoryTranslations).ToList().Adapt<List<CategoryResponseDTO>>();

            var result = categories.Select(cat => new
            {
                Id = cat.Id,
                Name = cat.categoryTranslations.FirstOrDefault(c => c.Language == lang).Name,
            });
            return Ok(new { message = _localizer["succsess"].Value, data = result });
        }

        [HttpGet("{id}")]
        public IActionResult Details([FromRoute]int id)
        {
            var category = _context.categories.Find(id);
            if(category is not null)
            {
                return Ok(new { message = _localizer["succsess"], data = category.Adapt<CategoryResponseDTO>() });
            }
            else
            {
                return NotFound(new { message = "there is no category with this id" });
            }
        }
        [HttpPatch("{id}")]
        public IActionResult Update([FromRoute]int id ,[FromBody ] CategoryRequistDTO requist)
        {
            var category = _context.categories.Find(id);
            if (category is not null)
            {
                requist.Adapt(category);
                _context.categories.Update(category);
                _context.SaveChanges();
                return Ok(new { message = _localizer["succsess"], data = category.Adapt<CategoryResponseDTO>() });
            }
            else
            {
                return NotFound(new { message = _localizer["not-found"] });
            }
        }
        [HttpPatch("{id}/Status")]
        public IActionResult UpdateStatus([FromRoute] int id)
        {
            var category = _context.categories.Find(id);
            if (category is not null)
            {
                category.status = category.status == Status.Active ? Status.InActive : Status.Active;
                _context.SaveChanges();
                return Ok(new { message = _localizer["succsess"] });
            }
            else
            {
                return NotFound(new { message = _localizer["not-found"] });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult delete([FromRoute] int id)
        {
            var category = _context.categories.Find(id);
            if(category is null)
            {
                return NotFound(new { message = _localizer["not-found"] });
            }
            _context.categories.Remove(category);
            _context.SaveChanges();
            return Ok(new {message= _localizer["succsess"]  });
        }

        [HttpDelete("")]
        public IActionResult deleteAll()
        {
            var categories = _context.categories.ToList();
            if (!categories.Any())
            {
                return NotFound(new { message = _localizer["not-found"] });
            }
            _context.RemoveRange(categories);
            _context.SaveChanges();
            return Ok(new { message = _localizer["succsess"] });
        }
    }
}
