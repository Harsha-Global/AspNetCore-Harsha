using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System.Xml.Linq;

namespace Tests
{
 public class StocksServiceTest
 {
  private readonly IStocksService _stocksService;

  public StocksServiceTest()
  {
   _stocksService = new StocksService();
  }



  #region CreateBuyOrder


  [Fact]
  public void CreateBuyOrder_NullBuyOrder_ToBeArgumentNullException()
  {
   //Arrange
   BuyOrderRequest? buyOrderRequest = null;

   //Act
   Assert.Throws<ArgumentNullException>(() =>
   {
    _stocksService.CreateBuyOrder(buyOrderRequest);
   });
  }



  [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
  [InlineData(0)] //passing parameters to the tet method
  public void CreateBuyOrder_QuantityIsLessThanMinimum_ToBeArgumentException(uint buyOrderQuantity)
  {
   //Arrange
   BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = buyOrderQuantity };

   //Act
   Assert.Throws<ArgumentException>(() =>
   {
    _stocksService.CreateBuyOrder(buyOrderRequest);
   });
  }


  [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
  [InlineData(100001)] //passing parameters to the tet method
  public void CreateBuyOrder_QuantityIsGreaterThanMaximum_ToBeArgumentException(uint buyOrderQuantity)
  {
   //Arrange
   BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = buyOrderQuantity };

   //Act
   Assert.Throws<ArgumentException>(() =>
   {
    _stocksService.CreateBuyOrder(buyOrderRequest);
   });
  }


  [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
  [InlineData(0)] //passing parameters to the tet method
  public void CreateBuyOrder_PriceIsLessThanMinimum_ToBeArgumentException(uint buyOrderPrice)
  {
   //Arrange
   BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = buyOrderPrice, Quantity = 1 };

   //Act
   Assert.Throws<ArgumentException>(() =>
   {
    _stocksService.CreateBuyOrder(buyOrderRequest);
   });
  }


  [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
  [InlineData(10001)] //passing parameters to the tet method
  public void CreateBuyOrder_PriceIsGreaterThanMaximum_ToBeArgumentException(uint buyOrderQuantity)
  {
   //Arrange
   BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = buyOrderQuantity };

   //Act
   Assert.Throws<ArgumentException>(() =>
   {
    _stocksService.CreateBuyOrder(buyOrderRequest);
   });
  }


  [Fact]
  public void CreateBuyOrder_StockSymbolIsNull_ToBeArgumentException()
  {
   //Arrange
   BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = null, Price = 1, Quantity = 1 };

   //Act
   Assert.Throws<ArgumentException>(() =>
   {
    _stocksService.CreateBuyOrder(buyOrderRequest);
   });
  }


  [Fact]
  public void CreateBuyOrder_DateOfOrderIsLessThanYear2000_ToBeArgumentException()
  {
   //Arrange
   BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", DateAndTimeOfOrder = Convert.ToDateTime("1999-12-31"), Price = 1, Quantity = 1 };

   //Act
   Assert.Throws<ArgumentException>(() =>
   {
    _stocksService.CreateBuyOrder(buyOrderRequest);
   });
  }


  [Fact]
  public void CreateBuyOrder_ValidData_ToBeSuccessful()
  {
   //Arrange
   BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", DateAndTimeOfOrder = Convert.ToDateTime("2024-12-31"), Price = 1, Quantity = 1 };

   //Act
   BuyOrderResponse buyOrderResponseFromCreate = _stocksService.CreateBuyOrder(buyOrderRequest);

   //Assert
   Assert.NotEqual(Guid.Empty, buyOrderResponseFromCreate.BuyOrderID);
  }


  #endregion




  #region CreateSellOrder


  [Fact]
  public void CreateSellOrder_NullSellOrder_ToBeArgumentNullException()
  {
   //Arrange
   SellOrderRequest? sellOrderRequest = null;

   //Act
   Assert.Throws<ArgumentNullException>(() =>
   {
    _stocksService.CreateSellOrder(sellOrderRequest);
   });
  }



  [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
  [InlineData(0)] //passing parameters to the tet method
  public void CreateSellOrder_QuantityIsLessThanMinimum_ToBeArgumentException(uint sellOrderQuantity)
  {
   //Arrange
   SellOrderRequest? sellOrderRequest = new SellOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = sellOrderQuantity };

   //Act
   Assert.Throws<ArgumentException>(() =>
   {
    _stocksService.CreateSellOrder(sellOrderRequest);
   });
  }


  [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
  [InlineData(100001)] //passing parameters to the tet method
  public void CreateSellOrder_QuantityIsGreaterThanMaximum_ToBeArgumentException(uint sellOrderQuantity)
  {
   //Arrange
   SellOrderRequest? sellOrderRequest = new SellOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = sellOrderQuantity };

   //Act
   Assert.Throws<ArgumentException>(() =>
   {
    _stocksService.CreateSellOrder(sellOrderRequest);
   });
  }


  [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
  [InlineData(0)] //passing parameters to the tet method
  public void CreateSellOrder_PriceIsLessThanMinimum_ToBeArgumentException(uint sellOrderPrice)
  {
   //Arrange
   SellOrderRequest? sellOrderRequest = new SellOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = sellOrderPrice, Quantity = 1 };

   //Act
   Assert.Throws<ArgumentException>(() =>
   {
    _stocksService.CreateSellOrder(sellOrderRequest);
   });
  }


  [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
  [InlineData(10001)] //passing parameters to the tet method
  public void CreateSellOrder_PriceIsGreaterThanMaximum_ToBeArgumentException(uint sellOrderQuantity)
  {
   //Arrange
   SellOrderRequest? sellOrderRequest = new SellOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = sellOrderQuantity };

   //Act
   Assert.Throws<ArgumentException>(() =>
   {
    _stocksService.CreateSellOrder(sellOrderRequest);
   });
  }


  [Fact]
  public void CreateSellOrder_StockSymbolIsNull_ToBeArgumentException()
  {
   //Arrange
   SellOrderRequest? sellOrderRequest = new SellOrderRequest() { StockSymbol = null, Price = 1, Quantity = 1 };

   //Act
   Assert.Throws<ArgumentException>(() =>
   {
    _stocksService.CreateSellOrder(sellOrderRequest);
   });
  }


  [Fact]
  public void CreateSellOrder_DateOfOrderIsLessThanYear2000_ToBeArgumentException()
  {
   //Arrange
   SellOrderRequest? sellOrderRequest = new SellOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", DateAndTimeOfOrder = Convert.ToDateTime("1999-12-31"), Price = 1, Quantity = 1 };

   //Act
   Assert.Throws<ArgumentException>(() =>
   {
    _stocksService.CreateSellOrder(sellOrderRequest);
   });
  }


  [Fact]
  public void CreateSellOrder_ValidData_ToBeSuccessful()
  {
   //Arrange
   SellOrderRequest? sellOrderRequest = new SellOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", DateAndTimeOfOrder = Convert.ToDateTime("2024-12-31"), Price = 1, Quantity = 1 };

   //Act
   SellOrderResponse sellOrderResponseFromCreate = _stocksService.CreateSellOrder(sellOrderRequest);

   //Assert
   Assert.NotEqual(Guid.Empty, sellOrderResponseFromCreate.SellOrderID);
  }


  #endregion




  #region GetBuyOrders

  //The GetAllBuyOrders() should return an empty list by default
  [Fact]
  public void GetAllBuyOrders_DefaultList_ToBeEmpty()
  {
   //Act
   List<BuyOrderResponse> buyOrdersFromGet = _stocksService.GetBuyOrders();

   //Assert
   Assert.Empty(buyOrdersFromGet);
  }


  [Fact]
  public void GetAllBuyOrders_WithFewBuyOrders_ToBeSuccessful()
  {
   //Arrange

   //Create a list of buy orders with hard-coded data
   BuyOrderRequest buyOrder_request_1 = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = 1, DateAndTimeOfOrder = DateTime.Parse("2023-01-01 9:00") };

   BuyOrderRequest buyOrder_request_2 = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = 1, DateAndTimeOfOrder = DateTime.Parse("2023-01-01 9:00") };

   List<BuyOrderRequest> buyOrder_requests = new List<BuyOrderRequest>() { buyOrder_request_1, buyOrder_request_2 };

   List<BuyOrderResponse> buyOrder_response_list_from_add = new List<BuyOrderResponse>();

   foreach (BuyOrderRequest buyOrder_request in buyOrder_requests)
   {
    BuyOrderResponse buyOrder_response = _stocksService.CreateBuyOrder(buyOrder_request);
    buyOrder_response_list_from_add.Add(buyOrder_response);
   }

   //Act
   List<BuyOrderResponse> buyOrders_list_from_get = _stocksService.GetBuyOrders();


   //Assert
   foreach (BuyOrderResponse buyOrder_response_from_add in buyOrder_response_list_from_add)
   {
    Assert.Contains(buyOrder_response_from_add, buyOrders_list_from_get);
   }
  }

  #endregion




  #region GetSellOrders

  //The GetAllSellOrders() should return an empty list by default
  [Fact]
  public void GetAllSellOrders_DefaultList_ToBeEmpty()
  {
   //Act
   List<SellOrderResponse> sellOrdersFromGet = _stocksService.GetSellOrders();

   //Assert
   Assert.Empty(sellOrdersFromGet);
  }


  [Fact]
  public void GetAllSellOrders_WithFewSellOrders_ToBeSuccessful()
  {
   //Arrange

   //Create a list of sell orders with hard-coded data
   SellOrderRequest sellOrder_request_1 = new SellOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = 1, DateAndTimeOfOrder = DateTime.Parse("2023-01-01 9:00") };

   SellOrderRequest sellOrder_request_2 = new SellOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = 1, DateAndTimeOfOrder = DateTime.Parse("2023-01-01 9:00") };

   List<SellOrderRequest> sellOrder_requests = new List<SellOrderRequest>() { sellOrder_request_1, sellOrder_request_2 };

   List<SellOrderResponse> sellOrder_response_list_from_add = new List<SellOrderResponse>();

   foreach (SellOrderRequest sellOrder_request in sellOrder_requests)
   {
    SellOrderResponse sellOrder_response = _stocksService.CreateSellOrder(sellOrder_request);
    sellOrder_response_list_from_add.Add(sellOrder_response);
   }

   //Act
   List<SellOrderResponse> sellOrders_list_from_get = _stocksService.GetSellOrders();


   //Assert
   foreach (SellOrderResponse sellOrder_response_from_add in sellOrder_response_list_from_add)
   {
    Assert.Contains(sellOrder_response_from_add, sellOrders_list_from_get);
   }
  }

  #endregion

 }
}


