﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Errors;
using Talabat.Core.DTOs;
using Talabat.Core.Models.Order_Aggregate;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
    [Authorize]
    public class OrdersController : ApiBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpPost] 
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto) 
        {
            string? buyerEmail = User.FindFirstValue(ClaimTypes.Email); 
            Address mappedAddress = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);
            Order? order = await _orderService.CreateOrderAsync(buyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, mappedAddress);
            if (order is null)
                return BadRequest(new ApiResponse(400));
            OrderToReturnDto? mappedOrder = _mapper.Map<Order, OrderToReturnDto>(order);
            return Ok(mappedOrder);
        }



        
        [HttpGet] 
        [ProducesResponseType(typeof(IReadOnlyList<Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            string? buyerEmail = User.FindFirstValue(ClaimTypes.Email); 
            IReadOnlyList<Order> orders = await _orderService.GetOrdersForUserAsync(buyerEmail);
            if (orders is null || orders.Count == 0)
                return NotFound(new ApiResponse(404));
            IReadOnlyList<OrderToReturnDto>? mappedOrders = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders);
            return Ok(mappedOrders);
        }


        [HttpGet("{id}")] 
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderForUser(int id)
        {
            string? buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            Order order = await _orderService.GetOrderByIdForUserAsync(id, buyerEmail);
            if (order is null)
                return NotFound(new ApiResponse(404));
            OrderToReturnDto? mappedOrder = _mapper.Map<Order, OrderToReturnDto>(order);
            return Ok(mappedOrder);
        }
        


        #region Get Delivery Methods
        [HttpGet("deliveryMethods")] // GET: api/orders/deliveryMethods
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            IReadOnlyList<DeliveryMethod> deliveryMethods = await _orderService.GetDeliveryMethodsAsync();

            if (deliveryMethods is null || deliveryMethods.Count == 0)
                return NotFound(new ApiResponse(404));
            else
                return Ok(deliveryMethods);
        }
        #endregion
    }
}
