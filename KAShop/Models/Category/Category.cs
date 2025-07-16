namespace KAShop.Models.Category
{

    public class Category : BaseModel
    {
        public List<CategoryTranslation> categoryTranslations { get; set; } 
            = new List<CategoryTranslation>();
       
    }
}
