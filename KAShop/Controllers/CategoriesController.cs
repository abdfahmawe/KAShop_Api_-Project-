using Azure.Core;
using KAShop.Data;
using KAShop.DTO.Requist;
using KAShop.DTO.Response;
using KAShop.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Create(CategoryRequistDTO requist)
        {
            context.categories.Add(requist.Adapt<Category>());
            context.SaveChanges();
            return Ok(new {message = "the add done " , data = requist.Adapt<CategoryResponseDTO>() });
        }
        [HttpGet("")]
        public IActionResult GetAll()
        {
            var categories = context.categories.OrderByDescending(c => c.CreatedAt).ToList().Adapt<List<CategoryResponseDTO>>();

            return Ok(new {message ="here is the categories ", data = categories });
        }
        [HttpGet("forUser")]
        public IActionResult Index()
        {
            var categories = context.categories.Where(c => c.status==Status.Active).ToList().Adapt<List<CategoryResponseDTO>>();

            return Ok(new { message = "here is the categories ", data = categories });
        }

        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            var category = context.categories.Find(id);
            if(category is not null)
            {
                return Ok(new { message = "succsses", data = category.Adapt<CategoryResponseDTO>() });
            }
            else
            {
                return NotFound(new { message = "there is no category with this id" });
            }
        }
        [HttpPatch("{id}")]
        public IActionResult Update(int id , CategoryRequistDTO requist)
        {
            var category = context.categories.Find(id);
            if (category is not null)
            {
                requist.Adapt(category);
                context.categories.Update(category);
                context.SaveChanges();
                return Ok(new { message = "succsses", data = category.Adapt<CategoryResponseDTO>() });
            }
            else
            {
                return NotFound(new { message = "there is no category with this id" });
            }
        }
        [HttpPatch("{id}/Status")]
        public IActionResult UpdateStatus(int id)
        {
            var category = context.categories.Find(id);
            if (category is not null)
            {
                category.status = category.status == Status.Active ? Status.InActive : Status.Active;
                context.SaveChanges();
                return Ok(new { message = "status changed succssusfle"});
            }
            else
            {
                return NotFound(new { message = "there is no category with this id" });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult delete(int id)
        {
            var category = context.categories.Find(id);
            if(category is null)
            {
                return NotFound(new { message = "there is no category with this id" });
            }
            context.categories.Remove(category);
            context.SaveChanges();
            return Ok(new {message= "remove done succssefuly"});
        }

        [HttpDelete("")]
        public IActionResult deleteAll()
        {
            var categories = context.categories.ToList();
            if (!categories.Any())
            {
                return NotFound(new { message = "there is no categories at all" });
            }
            context.RemoveRange(categories);
            context.SaveChanges();
            return Ok(new { message = "remove all categories done succssefuly" });
        }
    }
}
