using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using ServiceContracts.DTO;
using ServiceContracts.StocksService;
using Services.Helpers;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;

namespace Services.StocksService
{
    public class StocksSellOrdersService : ISellOrdersService
    {
        //private field
        private readonly IStocksRepository _stocksRepository;


        /// <summary>
        /// Constructor of StocksService class that executes when a new object is created for the class
        /// </summary>
        public StocksSellOrdersService(IStocksRepository stocksRepository)
        {
            _stocksRepository = stocksRepository;
        }



        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            //Validation: sellOrderRequest can't be null
            if (sellOrderRequest == null)
                throw new ArgumentNullException(nameof(sellOrderRequest));

            //Model validation
            ValidationHelper.ModelValidation(sellOrderRequest);

            //convert sellOrderRequest into SellOrder type
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();

            //generate SellOrderID
            sellOrder.SellOrderID = Guid.NewGuid();

            //add sell order object to sell orders list
            SellOrder SellOrderFromRepo = await _stocksRepository.CreateSellOrder(sellOrder);

            //convert the SellOrder object into SellOrderResponse type
            return sellOrder.ToSellOrderResponse();
        }


        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            //Convert all SellOrder objects into SellOrderResponse objects
            List<SellOrder> sellOrders = await _stocksRepository.GetSellOrders();

            return sellOrders.Select(temp => temp.ToSellOrderResponse()).ToList();
        }
    }
}


