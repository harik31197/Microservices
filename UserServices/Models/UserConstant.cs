using System.Collections.Generic;

namespace UserServices.Models
{
  public class UserConstant
  {

    public static List<UserModel> Users = new List<UserModel>()
    {
      new UserModel()
      {
        Username = "harik",
        Password = "hari@123",
        Name = "Harikrishnan",
        Role = "Admin"

      },
      new UserModel()
      {
        Username = "gopik",
        Password = "gopi@123",
        Name = "Gopikrishnan",
        Role = "User"

      },

    };
  }
}
