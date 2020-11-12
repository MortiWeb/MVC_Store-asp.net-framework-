using MVC_Store.Models.Data;
using MVC_Store.Models.ViewModels.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Store.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart/CartPage
        public ActionResult CartPage()
        {
            List<CartVM> cart = Session["cart"] as List<CartVM> ?? new List<CartVM>();
            if (cart.Count == 0 || Session["cart"] == null)
            {
                ViewBag.Msg = "Your cart is empty.";
                return View();
            }
            decimal total = 0m;
            foreach (var item in cart)
            {
                total += item.Total;
            }
            ViewBag.Total = total;
            return View(cart);
        }

        // GET: Cart/CartPartial/{id}
        public ActionResult CartPartial(int? id)
        {
            List<CartVM> cartList = Session["cart"] as List<CartVM> ?? new List<CartVM>();
            CartVM addToCart;
            CartVM model = new CartVM();
            int qty = 0;
            decimal total = 0m;

            if (cartList.Count > 0)
            {
                foreach (var item in cartList)
                {
                    if (id != null && id == item.ProductId)
                    {
                        item.Quantity += 1;
                        id = null;
                    }
                    total += item.Price * item.Quantity;
                    qty += item.Quantity;
                }
            }
            if (id != null)
            {
                using (Db db = new Db())
                {
                    ProductDTO dto = db.Products.Find(id);
                    if (dto != null)
                    {
                        addToCart = new CartVM(dto);
                        cartList.Add(addToCart);
                        total += addToCart.Price;
                        qty++;
                    }
                }
            }
            Session["cart"] = cartList;
            model.Quantity = qty;
            model.Price = total;

            return PartialView("_CartPartial", model);
        }
        // GET: Cart/IncrementProduct
        public JsonResult IncrementProduct(int id)
        {
            int totalQty = 0;
            decimal _price = 0m;
            decimal _grandtotal = 0m;
            int _qty = 0;
            var cartList = Session["cart"] as List<CartVM>;
            foreach (var item in cartList)
            {
                if (item.ProductId == id)
                {
                    _price = item.Price;
                    item.Quantity++;
                    _qty = item.Quantity;
                }
                totalQty += item.Quantity;
                _grandtotal += item.Total;
            }
            Session["cart"] = cartList;
            var result = new { qty = _qty, price = _price, totalqty = totalQty, grandtotal = _grandtotal };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: Cart/IncrementProduct
        public JsonResult DecrementProduct(int id)
        {
            int totalQty = 0;
            decimal _price = 0m;
            decimal _grandtotal = 0m;
            int _qty = 0;
            var cartList = Session["cart"] as List<CartVM>;
            foreach (var item in cartList)
            {
                if (item.ProductId == id)
                {
                    _price = item.Price;
                    if(item.Quantity > 1)
                    {
                        item.Quantity--;
                    }
                    _qty = item.Quantity;
                }
                totalQty += item.Quantity;
                _grandtotal += item.Total;
            }
            Session["cart"] = cartList;
            var result = new { qty = _qty, price = _price, totalqty = totalQty, grandtotal = _grandtotal };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}