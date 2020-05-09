using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductSellingOnline.Dtos;
using ProductSellingOnline.Services;

namespace ProductSellingOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypesController : ControllerBase
    {
        private IProductTypesServices productTypeServices { get; set; }

        public ProductTypesController(IProductTypesServices _productTypesServices)
        {
            productTypeServices = _productTypesServices;
        }

       //api/ProductTypes
       [HttpGet]
       [ProducesResponseType(400)]
       [ProducesResponseType(200, Type = typeof(IEnumerable<ProductTypeDto>))]
       public IActionResult GetProductTypes()
       {
            var productTypes = productTypeServices.GetProductTypes();

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productTypeDto = new List<ProductTypeDto>();

            foreach (var prod in productTypes)
            {
                productTypeDto.Add(new ProductTypeDto
                {
                    Id = prod.Id,
                    Name = prod.Name
                });
            }

          return Ok(productTypeDto);
       }

        //api/ProductTypes/{id0}
        [HttpGet("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(ProductTypeDto))]
        public IActionResult GetProductType(int id)
        {
            if(!productTypeServices.IsProductTypeExist(id))
            {
                return NotFound();
            }

            var ProductType = productTypeServices.GetProductType(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productTypeDto = new ProductTypeDto()
            {
                Id = ProductType.Id,
                Name = ProductType.Name
            };

            return Ok(productTypeDto);
        }


    }
}