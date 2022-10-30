using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;

namespace Services
{
 public class StocksService : IStocksService
 {
  //private field
  private readonly IStocksRepository _stocksRepository;


  /// <summary>
  /// Constructor of StocksService class that executes when a new object is created for the class
  /// </summary>
  public StocksService(IStocksRepository stocksRepository)
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


  public async Task<List<BuyOrderResponse>> GetBuyOrders()
  {
   //Convert all BuyOrder objects into BuyOrderResponse objects
   List<BuyOrder> buyOrders = await _stocksRepository.GetBuyOrders();

   return buyOrders.Select(temp => temp.ToBuyOrderResponse()).ToList();
  }


  public async Task<List<SellOrderResponse>> GetSellOrders()
  {
   //Convert all SellOrder objects into SellOrderResponse objects
   List<SellOrder> sellOrders = await _stocksRepository.GetSellOrders();

   return sellOrders.Select(temp => temp.ToSellOrderResponse()).ToList();
  }
 }
}


