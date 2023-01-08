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
            var products = await _repo.SelectAllProducts(cancellationToken);

            return Ok(products);
        }

        [HttpGet ("{id}")]
        public async Task<IActionResult> GetProduct(int id, CancellationToken cancellationToken)
        {
            var res = await _repo.SelectProduct(id, cancellationToken);

            if (res != null) { 
                return Ok(res);
            }
            else
            {
                return NotFound("Id "+ id + " was not found");
            }
        }
    }




    
}
