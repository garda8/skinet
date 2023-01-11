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
using API.Errors;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : BaseApiController
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
        public async Task<IActionResult> GetProducts(CancellationToken cancellationToken, string sort , int? brandId, int? typeId)
        {
            IEnumerable<ProductViewModel> products = await _repo.GetProductsAsync(cancellationToken, sort, brandId, typeId);
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProduct(int id, CancellationToken cancellationToken)
        {
            var product = await _repo.GetProductByIdAsync(id, cancellationToken);
            
            if (product == null)
            {
                return NotFound(new ApiResponse(404));
            } 
            
            if (!string.IsNullOrEmpty(product.PictureUrl))
            {
                product.PictureUrl = _config["ApiUrl"] + product.PictureUrl;
            }


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
