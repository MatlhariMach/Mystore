using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mystore.Models


{
    public class ProductViewModel
    {
       
        
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public int CategoryId { get; set; }
            public Category? Category { get; set; }
            public IFormFile? ImageFile { get; set; }
            public string? ContentType { get; set; } = string.Empty;
            public byte[]? Data { get; set; } = Array.Empty<byte>();
        

    }



}
