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
 public class StocksBuyOrdersService : IBuyOrdersService
 {
  //private field
  private readonly IStocksRepository _stocksRepository;


  /// <summary>
  /// Constructor of StocksService class that executes when a new object is created for the class
  /// </summary>
  public StocksBuyOrdersService(IStocksRepository stocksRepository)
  {
   _stocksRepository = stocksRepository;
  }


  public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
  {
   //Validation: buyOrderRequest can't be null
   if (buyOrderRequest == null)
    throw new ArgumentNullException(nameof(buyOrderRequest));

   //Model validation
   ValidationHelper.ModelValidation(buyOrderRequest);

   //convert buyOrderRequest into BuyOrder type
   BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

   //generate BuyOrderID
   buyOrder.BuyOrderID = Guid.NewGuid();

   //add buy order object to buy orders list
   BuyOrder buyOrderFromRepo = await _stocksRepository.CreateBuyOrder(buyOrder);

   //convert the BuyOrder object into BuyOrderResponse type
   return buyOrder.ToBuyOrderResponse();
  }


  public async Task<List<BuyOrderResponse>> GetBuyOrders()
  {
   //Convert all BuyOrder objects into BuyOrderResponse objects
   List<BuyOrder> buyOrders = await _stocksRepository.GetBuyOrders();

   return buyOrders.Select(temp => temp.ToBuyOrderResponse()).ToList();
  }
 }
}


