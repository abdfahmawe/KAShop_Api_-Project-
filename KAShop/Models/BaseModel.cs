namespace KAShop.Models
{
    public enum Status
    {
        Active,
        InActive
    }
    public class BaseModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Status status { get; set; } = Status.Active;
    }
}
