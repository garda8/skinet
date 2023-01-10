using API.Errors;
using Infastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : BaseApiController
    {
        private readonly IProductsRepository _repo;
        private readonly IConfiguration _config;

        public BuggyController(IProductsRepository repo,  IConfiguration config)
        {
            this._repo = repo;
            this._config = config;
            //StoreContext? Har nog inte denna..
        }
        
        [HttpGet("notfound")]
        public async Task<IActionResult> GotNotFoundRequest(CancellationToken cancellationToken)
        {
            var thing = await _repo.GetProductByIdAsync(9000, cancellationToken);
            
            if (thing == null)
            {
                return NotFound(new ApiResponse(404));
                //return NotFound(new ApiResponse(404, "Heja björklöven"));  //status 404

            }
                return Ok();
        }

        [HttpGet("servererror")]
        public async Task<IActionResult> GotServerError(CancellationToken cancellationToken)
        {
            var thing = await _repo.GetProductByIdAsync(9000, cancellationToken);

            var thingToReturn = thing.ToString();  //System null reference exception här..

            if (thing == null)
            {
                return NotFound();

            }
            return Ok();
        }

        [HttpGet("badrequest")]
        public async Task<IActionResult> GetBadRequest(CancellationToken cancellationToken)
        {
            return BadRequest(new ApiResponse(400)); //400 
        }

        [HttpGet("badrequest/{id}")]
        public async Task<IActionResult> GetNotFoundRequest(int id, CancellationToken cancellationToken)
        {
            return BadRequest();
        }

        //[HttpGet("notfound")]
        //public ActionResult GotNotFoundRequest()
        //{
        //    return Ok();
        //}

        //[HttpGet("notfound")]
        //public ActionResult GotNotFoundRequest()
        //{
        //    return Ok();
        //}



    }
}
