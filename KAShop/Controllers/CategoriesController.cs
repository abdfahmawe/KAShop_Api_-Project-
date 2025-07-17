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
        ApplicationDbContext context = new ApplicationDbContext();

        private readonly IStringLocalizer<SharedResource> _localizer;
        public CategoriesController(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }



        [HttpPost("")]
        public IActionResult Create([FromBody] CategoryRequistDTO requist)
        {
            context.categories.Add(requist.Adapt<Category>());
            context.SaveChanges();
            return Ok(new {message = _localizer["added-success"], data = requist.Adapt<CategoryResponseDTO>() });
        }
        [HttpGet("")]
        public IActionResult GetAll()
        {
            var categories = context.categories.OrderByDescending(c => c.CreatedAt).ToList().Adapt<List<CategoryResponseDTO>>();

            return Ok(new {message = _localizer["succsess"], data = categories });
        }
        [HttpGet("forUser")]
        public IActionResult Index([FromQuery]string lang = "ar")
        {
           var categories = context.categories.Include(c=> c.categoryTranslations).ToList().Adapt<List<CategoryResponseDTO>>();

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
            var category = context.categories.Find(id);
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
            var category = context.categories.Find(id);
            if (category is not null)
            {
                requist.Adapt(category);
                context.categories.Update(category);
                context.SaveChanges();
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
            var category = context.categories.Find(id);
            if (category is not null)
            {
                category.status = category.status == Status.Active ? Status.InActive : Status.Active;
                context.SaveChanges();
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
            var category = context.categories.Find(id);
            if(category is null)
            {
                return NotFound(new { message = _localizer["not-found"] });
            }
            context.categories.Remove(category);
            context.SaveChanges();
            return Ok(new {message= _localizer["succsess"]  });
        }

        [HttpDelete("")]
        public IActionResult deleteAll()
        {
            var categories = context.categories.ToList();
            if (!categories.Any())
            {
                return NotFound(new { message = _localizer["not-found"] });
            }
            context.RemoveRange(categories);
            context.SaveChanges();
            return Ok(new { message = _localizer["succsess"] });
        }
    }
}
