﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Infrastructure;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository _repository;

        public CartController(IProductRepository repository)
        {
            _repository = repository;
        }

        public RedirectToActionResult AddToCart(int productId, string returnUrl)
        {
            var product = _repository.Products.FirstOrDefault(p => p.ProductId == productId);

            if (product != null)
            {
                var cart = GetCart();
                cart.AddItem(product, 1);
                SaveCart(cart);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToActionResult RemoveFromCart(int productId, string returnUrl)
        {
            var product = _repository.Products.FirstOrDefault(p => p.ProductId == productId);

            if (product != null)
            {
                var cart = GetCart();
                cart.RemoveLine(product);
                SaveCart(cart);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        private Cart GetCart()
        {
            var cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            return cart;
        }

        private void SaveCart(Cart cart)
        {
            HttpContext.Session.SetJson("Cart", cart);
        }
    }
}