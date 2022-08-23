using System.Collections.Generic;

namespace ProductService.Models
{
  public class ProductConstant
  {
    public static List<ProductModel> Products = new List<ProductModel>()
    {
      new ProductModel()
      {
        Id = 1,
        Name = "Macbook Air",
        Rate = 99000
      },
      new ProductModel()
      {
        Id = 2,
        Name = "HP Pavilion",
        Rate = 87000
      },

    };
  }
}
