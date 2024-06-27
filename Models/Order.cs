namespace Mystore.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        public string UserId { get; set; }

     /*   public User User { get; set; } */
        public ICollection<OrderItem> OrderItems { get; set; } 
    }
   
}

