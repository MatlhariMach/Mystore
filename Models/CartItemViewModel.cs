namespace Mystore.Models
{
    public class CartItemViewModel
    {
        public int Id { get; set; }
        public Double TotalPrice { get; set; }

        public int Quantity { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public string UserId { get; set; }

        public Double Total => TotalPrice * Quantity;

        public string Description { get; set; } 
        public string Name { get; set; }

        public Double Subtotal => Total;
    }

}
