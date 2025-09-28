using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Core.DTOs;
using Talabat.Core.Models.Basket;
using Talabat.Core.Repositories;

namespace Talabat.APIs.Controllers
{
    public class BasketController : ApiBaseController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasket([FromQuery] string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);

            if (basket is null)
            {
                basket = new CustomerBasket(id);
                return basket;
            }
            else
                return basket;
        }


        [HttpPost] 
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket) 
        {
            CustomerBasket mappedBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);
            CustomerBasket? createdOrUpdatedBasket = await _basketRepository.UpdateBasketAsync(mappedBasket);
            if (createdOrUpdatedBasket is null)
                return BadRequest(new ApiResponse(400, "An Error With Your Basket"));
            else
                return createdOrUpdatedBasket;
        }

        [HttpDelete] 
        public async Task<ActionResult<bool>> DeleteBasket([FromQuery] string id)
        {
            return await _basketRepository.DeleteBasketAsync(id);
        }
    }
}
