using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using M05_UF3_P2_Template.App_Code.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace M05_UF3_P2_Template.Pages.Products
{
    public class ProductModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public int Game_Id { get; set; }
        [BindProperty]
        public Product product { get; set; }

        public List<Company> companies = new List<Company>();
        public void OnGet()
        {
            DataTable dt = DatabaseManager.Select("Company", new string[] { "*" }, "");
            foreach (DataRow dataRow in dt.Rows)
            {
                companies.Add(new Company(dataRow));
            }
            if (Id > 0)
            {
                product = new Product(Id);
                if(product.Type == Product.TYPE.GAME)
                {
                    Game_Id = (int)DatabaseManager.Select("Game", new string[] { "Id" }, "Product_Id = " + Id + "").Rows[0][0];
                }
            }
        }
        public void OnPost()
        {
            product.Id = Id;
            if (Id > 0)
            {
                product.Update();
            }
            else
            {
                product.Add();
                Id = (int)DatabaseManager.Scalar("Product", DatabaseManager.SCALAR_TYPE.MAX, new string[] { "Id" }, "");
                if (product.Type == Product.TYPE.GAME)
                {
                    Game game = new Game();
                    game.Product_Id = Id;
                    game.Add();
                }
                else
                {

                }
                OnGet();
            }
        }
    }
}
