using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.Product_Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Talabat.Core;
using Talabat.Core.Models.Product;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : ApiBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;

        public ProductsController(IUnitOfWork unitOfWork,IMapper _mapper)
        {
            _unitOfWork = unitOfWork;
            mapper = _mapper;
        }




        [HttpGet]
        [CachedAttribute(600)] 
        [ProducesResponseType(typeof(IReadOnlyList<ProductToReturnDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<Pagination<ProductToReturnDto>>>> GetProducts([FromQuery] ProductSpecParams? productSpecs) // ActionResult : Detect the Type of Result
        {
            ProductWithBrandAndTypeSpecifications spec = new ProductWithBrandAndTypeSpecifications(productSpecs);
            IReadOnlyList<Product> products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);
            if (products is null)
                return NotFound(new ApiResponse(404));
            IReadOnlyList<ProductToReturnDto> mappedProducts = mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            ProductWithFiltrationForCountSpecification countSpec = new ProductWithFiltrationForCountSpecification(productSpecs);
            int countOfAllProducts = await _unitOfWork.Repository<Product>().GetCountWithSpecAsync(countSpec);

            return Ok(new Pagination<ProductToReturnDto>(productSpecs.PageIndex, productSpecs.PageSize, countOfAllProducts, mappedProducts));
        }


        [HttpGet("{id:int}")] 
        [CachedAttribute(600)] 
        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            ProductWithBrandAndTypeSpecifications spec = new ProductWithBrandAndTypeSpecifications(id);

            Product product = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(spec);
            if (product is null)
                return NotFound(new ApiResponse(404));

            ProductToReturnDto mappedProduct = mapper.Map<Product, ProductToReturnDto>(product);

            return Ok(mappedProduct);
        }

        [HttpGet("brands")] 
        [CachedAttribute(600)]
        [ProducesResponseType(typeof(IReadOnlyList<ProductBrand>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            IReadOnlyList<ProductBrand> brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            if (brands is null)
                return NotFound(new ApiResponse(404));
            return Ok(brands);
        }
       


        [HttpGet("types")] 
        [CachedAttribute(600)]  
        [ProducesResponseType(typeof(IReadOnlyList<ProductType>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            IReadOnlyList<ProductType> types = await _unitOfWork.Repository<ProductType>().GetAllAsync();
            if (types is null)
                return NotFound(new ApiResponse(404));
            return Ok(types);
        }
    }
}
