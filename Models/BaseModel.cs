namespace LOT_TASK.Models
{
    public class BaseModel
    {
        public Guid Id { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
