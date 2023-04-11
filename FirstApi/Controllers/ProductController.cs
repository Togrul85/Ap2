using FirstApi.Data.DAL;
using FirstApi.Dtos.ProductDtos;
using FirstApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FirstApi.Controllers
{

    public class ProductController : BaseController
    {
        private readonly AppDbContext _appDbContext;

        public ProductController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _appDbContext.Products
                .Where(p=>p.IsDeleted)
                .ToList();

            ProductListDto productListDto = new ();
            productListDto.TotalCount= products.Count;
            List<ProductListItemDto> listitemDto = new();
            foreach (var item in products)
            {
                ProductListItemDto productListItemDto= new();
                productListItemDto.Name= item.Name;
                productListItemDto.CostPrice= item.CostPrice;
                productListItemDto.SalePrice= item.SalePrice;
                productListItemDto.CreatedDate= item.CreatedDate;
                productListItemDto.UpdateDate= item.UpdateDate;
                listitemDto.Add(productListItemDto);
            }
            productListDto.Items = listitemDto;
            return StatusCode(200, productListDto);
            //return Ok(new {Code=1001,products});
        }

        [Route("{id}")]
        [HttpGet]
        public IActionResult GetOne(int id)

        {
            Product product = _appDbContext.Products
                .Where(p=>!p.IsDeleted)
                .FirstOrDefault(p => p.Id == id);

            if (product == null) return StatusCode(StatusCodes.Status404NotFound);
            ProductReturnDto productReturnDto = new() 
            { 
            Name = product.Name,
            SalePrice= product.SalePrice,
            CostPrice= product.CostPrice,
            CreatedDate= product.CreatedDate,
            UpdateDate= product.UpdateDate,


            };

            return Ok(productReturnDto);
        }
        [HttpPost]
        public IActionResult AddProduct(ProductCreateDto productCreateDto)
        {
            Product newProduct = new()
            {
                Name = productCreateDto.Name,
                SalePrice = productCreateDto.SalePrice,
                CostPrice = productCreateDto.CostPrice,

            };
            _appDbContext.Products.Add(newProduct);
            _appDbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created, newProduct);
        }

        [HttpDelete("{id}")]

        public IActionResult DeleteProduct(int id)
        {
            var product = _appDbContext.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            _appDbContext.Products.Remove(product);
            _appDbContext.SaveChanges();
            return StatusCode(StatusCodes.Status204NoContent);

        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, ProductUpdateDto productUpdateDto)
        { 
        var existProduct =_appDbContext.Products.FirstOrDefault(p=>p.Id==id);
            if (existProduct == null) return NotFound();
            existProduct.Name = productUpdateDto.Name;
            existProduct.SalePrice= productUpdateDto.SalePrice;
            existProduct.CostPrice= productUpdateDto.CostPrice;
            existProduct.IsActive = productUpdateDto.IsActive;
            _appDbContext.SaveChanges();
            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPatch]
        public IActionResult ChangeStatus(bool IsActive,Product product)
        {
            var existProduct = _appDbContext.Products.FirstOrDefault(p => p.Id == product.Id);
            if (existProduct == null) return NotFound();
            existProduct.IsActive = IsActive;
            _appDbContext.SaveChanges();
            return StatusCode(StatusCodes.Status204NoContent);

        }

    }

}
