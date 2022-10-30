using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using System;

namespace Services
{
 public class StocksService : IStocksService
 {
  //private field
  private readonly List<BuyOrder> _buyOrders;
  private readonly List<SellOrder> _sellOrders;


  /// <summary>
  /// Constructor of StocksService class that executes when a new object is created for the class
  /// </summary>
  public StocksService()
  {
   _buyOrders = new List<BuyOrder>();
   _sellOrders = new List<SellOrder>();
  }


  public BuyOrderResponse CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
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
   _buyOrders.Add(buyOrder);

   //convert the BuyOrder object into BuyOrderResponse type
   return buyOrder.ToBuyOrderResponse();
  }


  public SellOrderResponse CreateSellOrder(SellOrderRequest? sellOrderRequest)
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
   _sellOrders.Add(sellOrder);

   //convert the SellOrder object into SellOrderResponse type
   return sellOrder.ToSellOrderResponse();
  }


  public List<BuyOrderResponse> GetBuyOrders()
  {
   //Convert all BuyOrder objects into BuyOrderResponse objects
   return _buyOrders
    .OrderByDescending(temp => temp.DateAndTimeOfOrder)
    .Select(temp => temp.ToBuyOrderResponse()).ToList();
  }


  public List<SellOrderResponse> GetSellOrders()
  {
   //Convert all SellOrder objects into SellOrderResponse objects
   return _sellOrders
    .OrderByDescending(temp => temp.DateAndTimeOfOrder)
    .Select(temp => temp.ToSellOrderResponse()).ToList();
  }
 }
}


