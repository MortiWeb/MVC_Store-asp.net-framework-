using MVC_Store.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Store.Models.ViewModels.Cart
{
    public class CartVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total {
            get { return Price * Quantity; }
        }
        public string Image { get; set; }

        public CartVM()
        {
        }
        public CartVM(ProductDTO dto)
        {
            ProductId = dto.Id;
            ProductName = dto.Name;
            Quantity = 1;
            Price = dto.Price;
            Image = dto.ImageName;
        }
    }
}