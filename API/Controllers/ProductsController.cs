using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Infastructure.Repositories;
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsRepository _repo;
        private IMapper _mapper;
        private readonly IConfiguration _config;

        public ProductsController(IProductsRepository repo, IMapper mapper, IConfiguration config)
        {
            _repo = repo;
            _mapper = mapper;
            _config = config;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetProducts(CancellationToken cancellationToken)
        {
            var products = await _repo.GetProductsAsync(cancellationToken);
            foreach (var p in products)
            {
                if (!string.IsNullOrEmpty(p.PictureUrl))
                {
                    p.PictureUrl = _config["ApiUrl"] + p.PictureUrl;
                }
            }
            return Ok(_mapper.Map<IEnumerable<ProductViewModel>, IEnumerable<ProductToReturnDto>>(products));
            //var p = products.Select(product => new ProductToReturnDto
            //{
            //    Id = product.Id,
            //    Name = product.Name,
            //    Description = product.Description,
            //    PictureUrl = product.PictureUrl,
            //    Price = product.Price,
            //    ProductBrand = product.ProductBrand,
            //    ProductType = product.ProductType

            //});
            //return Ok(p);
        }

        [HttpGet ("{id}")]
        public async Task<IActionResult> GetProduct(int id, CancellationToken cancellationToken)
        {
            var product = await _repo.GetProductByIdAsync(id, cancellationToken);
            if (!string.IsNullOrEmpty(product.PictureUrl))
            {
                product.PictureUrl = _config["ApiUrl"] + product.PictureUrl;
            }
            if (product != null) {
                var p = _mapper.Map<ProductViewModel, ProductToReturnDto>(product)
                    ;
                //var p = new ProductToReturnDto
                //{
                //    Id = product.Id,
                //    Name = product.Name,
                //    Description = product.Description,
                //    PictureUrl = product.PictureUrl,
                //    Price = product.Price,
                //    ProductBrand = product.ProductBrand,
                //    ProductType = product.ProductType

                //};
                return Ok(p);
            }
            else
            {
                return NotFound("Id "+ id + " was not found");
            }
        }

        [HttpGet("Brands")]
        public async Task<IActionResult> GetProductBrands(CancellationToken cancellationToken)
        {
            var productBrands = await _repo.GetProductBrandsAsync(cancellationToken);

            return Ok(productBrands);
        }

        [HttpGet("Types")]
        public async Task<IActionResult> GetProductTypes(CancellationToken cancellationToken)
        {
            var productTypes = await _repo.GetProductTypesAsync(cancellationToken);

            return Ok(productTypes);
        }





    }





}
