using System.ComponentModel.DataAnnotations.Schema;

namespace Mystore.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public Cart Cart { get; set; }
        public Double TotalPrice { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Name { get; set; }
        public List<Product> Products {  get; set; }

       

    }
}
