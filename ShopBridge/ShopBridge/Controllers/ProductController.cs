using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopBridge.ApplicationClass;
using ShopBridge.CustomException;
using ShopBridge.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ShopBridge.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        #region Private Methods
        private IProductRepository _productRepository;
        private readonly ILogger<ProductController> _logger;
        #endregion

        #region Constructor
        public ProductController(IProductRepository productRepository, ILogger<ProductController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }
        #endregion

        #region Public Method

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductAC>> GetProductAsync([FromRoute] Guid id)
        {
            try
            {
                return Ok(await _productRepository.GetProductAsync(id));
            }
            catch(ValidationException ve)
            {
                return BadRequest(ve.Message);
            }   
        }

        [HttpGet]
        public async Task<ActionResult<PagedProductAC>> GetAllProductAsync([FromQuery] FilterModelAC filterModelAC)
        {
            return Ok(await _productRepository.GetAllProductsAsync(filterModelAC));
        }

        [HttpPost]
        public async Task<ActionResult<ProductAC>> AddProductAsync([FromBody] ProductAC product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return StatusCode((int)HttpStatusCode.Created, await _productRepository.SaveProductAsync(product));
                } catch(ValidationException ve)
                {
                    return BadRequest(ve.Message);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductAC>> UpdateProductAsync([FromRoute] Guid id, [FromBody] ProductAC product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ProductAC productAC = await _productRepository.UpdateProductAsync(id, product);
                    return StatusCode((int)HttpStatusCode.Created, productAC);
                }
                catch(ValidationException ve)
                {
                    return BadRequest(ve.Message);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductAC>> DeleteProductAsync([FromRoute] Guid id)
        {
            if (ModelState.IsValid)
            {
                try {
                    await _productRepository.DeleteProductAsync(id);
                    return StatusCode((int)HttpStatusCode.OK);
                }
                catch(ValidationException ve)
                {
                    return BadRequest(ve.Message);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        #endregion
    }
}
