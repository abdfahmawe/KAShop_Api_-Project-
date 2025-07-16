using KAShop.Models;

namespace KAShop.DTO.Requist
{
    public class CategoryRequistDTO
    {
        public Status status { get; set; } = Status.Active;
         public List<CategoryTranslationRequist> categoryTranslationRequists { get; set; }
    }
}
