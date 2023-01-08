using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Infastructure.Repositories;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsRepository _repo;
        public ProductsController(IProductsRepository repo)
        {
            _repo = repo;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetProducts(CancellationToken cancellationToken)
        {
            var products = await _repo.GetProductsAsync(cancellationToken);

            return Ok(products);
        }

        [HttpGet ("{id}")]
        public async Task<IActionResult> GetProduct(int id, CancellationToken cancellationToken)
        {
            var res = await _repo.GetProductByIdAsync(id, cancellationToken);

            if (res != null) { 
                return Ok(res);
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
