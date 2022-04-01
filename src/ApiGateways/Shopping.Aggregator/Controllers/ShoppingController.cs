using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]    
    public class ShoppingController : ControllerBase
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public ShoppingController(ICatalogService catalogService, IBasketService basketService, IOrderService orderService) {
            _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
            _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        }

        [HttpGet("{userName}", Name = "GetShopping")]
        [ProducesResponseType(typeof(ShoppingModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingModel>> GetShopping(string userName) {
            // get basket with username
            var basket = await _basketService.GetBasket(userName);
            // iterate basket items and consume products with basket item productId member
            foreach(var item in basket.Items) {
                // consume catalog microservice to retrieve product information
                var product = await _catalogService.GetCatalog(item.ProductId);
                // map product related members into basketitem dto with extended columns
                // set additional product fields
                item.ProductName = product.Name;
                item.Category = product.Category;
                item.Summary = product.Summary;
                item.Description = product.Description;
                item.ImageFile = product.ImageFile;
            }
            // consume ordering microservice to retrieve order list
            var orders = await _orderService.GetOrdersByUserName(userName);
            // create root ShoppingModel
            var shoppingModel = new ShoppingModel {
                UserName = userName,
                BasketWithProducts = basket,
                Orders = orders
            };
            // return root ShoppingModel dto which inludes all reponses
            return Ok(shoppingModel);
        }
    }
}
