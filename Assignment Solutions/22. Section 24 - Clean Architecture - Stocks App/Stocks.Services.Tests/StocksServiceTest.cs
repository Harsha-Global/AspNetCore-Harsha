using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using RepositoryContracts;
using ServiceContracts.DTO;
using ServiceContracts.StocksService;
using Services;
using Services.StocksService;
using System.Diagnostics;
using System.Xml.Linq;

namespace Tests.ServiceTests
{
 public class StocksServiceTest
 {
  private readonly IBuyOrdersService _stocksBuyOrdersService;
  private readonly ISellOrdersService _stocksSellOrdersService;

  private readonly Mock<IStocksRepository> _stocksRepositoryMock;
  private readonly IStocksRepository _stocksRepository;

  private readonly IFixture _fixture;

  public StocksServiceTest()
  {
   _fixture = new Fixture();

   _stocksRepositoryMock = new Mock<IStocksRepository>();
   _stocksRepository = _stocksRepositoryMock.Object;

   _stocksBuyOrdersService = new StocksBuyOrdersService(_stocksRepository);
   _stocksSellOrdersService = new StocksSellOrdersService(_stocksRepository);
  }



  #region CreateBuyOrder


  [Fact]
  public async Task CreateBuyOrder_NullBuyOrder_ToBeArgumentNullException()
  {
   //Arrange
   BuyOrderRequest? buyOrderRequest = null;

   //Mock
   BuyOrder buyOrderFixture = _fixture.Build<BuyOrder>()
    .Create();
   _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrderFixture);

   //Act
   Func<Task> action = async () =>
   {
    await _stocksBuyOrdersService.CreateBuyOrder(buyOrderRequest);
   };

   //Assert
   await action.Should().ThrowAsync<ArgumentNullException>();
  }



  [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
  [InlineData(0)] //passing parameters to the tet method
  public async Task CreateBuyOrder_QuantityIsLessThanMinimum_ToBeArgumentException(uint buyOrderQuantity)
  {
   ///Arrange
   BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
    .With(temp => temp.Quantity, buyOrderQuantity)
    .Create();

   //Mock
   BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
   _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

   //Act
   Func<Task> action = async () =>
   {
    await _stocksBuyOrdersService.CreateBuyOrder(buyOrderRequest);
   };

   //Assert
   await action.Should().ThrowAsync<ArgumentException>();
  }


  [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
  [InlineData(100001)] //passing parameters to the tet method
  public async Task CreateBuyOrder_QuantityIsGreaterThanMaximum_ToBeArgumentException(uint buyOrderQuantity)
  {
   //Arrange
   BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
    .With(temp => temp.Quantity, buyOrderQuantity)
    .Create();

   //Mock
   BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
   _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

   //Act
   Func<Task> action = async () =>
   {
    await _stocksBuyOrdersService.CreateBuyOrder(buyOrderRequest);
   };

   //Assert
   await action.Should().ThrowAsync<ArgumentException>();
  }


  [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
  [InlineData(0)] //passing parameters to the tet method
  public async Task CreateBuyOrder_PriceIsLessThanMinimum_ToBeArgumentException(uint buyOrderPrice)
  {
   //Arrange
   BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
    .With(temp => temp.Price, buyOrderPrice)
    .Create();

   //Mock
   BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
   _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

   //Act
   Func<Task> action = async () =>
   {
    await _stocksBuyOrdersService.CreateBuyOrder(buyOrderRequest);
   };

   //Assert
   await action.Should().ThrowAsync<ArgumentException>();
  }


  [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
  [InlineData(10001)] //passing parameters to the tet method
  public async Task CreateBuyOrder_PriceIsGreaterThanMaximum_ToBeArgumentException(uint buyOrderPrice)
  {
   //Arrange
   BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
    .With(temp => temp.Price, buyOrderPrice)
    .Create();

   //Mock
   BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
   _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

   //Act
   Func<Task> action = async () =>
   {
    await _stocksBuyOrdersService.CreateBuyOrder(buyOrderRequest);
   };

   //Assert
   await action.Should().ThrowAsync<ArgumentException>();
  }


  [Fact]
  public async Task CreateBuyOrder_StockSymbolIsNull_ToBeArgumentException()
  {
   //Arrange
   BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
    .With(temp => temp.StockSymbol, null as string)
    .Create();

   //Mock
   BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
   _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

   //Act
   Func<Task> action = async () =>
   {
    await _stocksBuyOrdersService.CreateBuyOrder(buyOrderRequest);
   };

   //Assert
   await action.Should().ThrowAsync<ArgumentException>();
  }


  [Fact]
  public async Task CreateBuyOrder_DateOfOrderIsLessThanYear2000_ToBeArgumentException()
  {
   //Arrange
   BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
    .With(temp => temp.DateAndTimeOfOrder, Convert.ToDateTime("1999-12-31"))
    .Create();

   //Mock
   BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
   _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

   //Act
   Func<Task> action = async () =>
   {
    await _stocksBuyOrdersService.CreateBuyOrder(buyOrderRequest);
   };

   //Assert
   await action.Should().ThrowAsync<ArgumentException>();
  }


  [Fact]
  public async Task CreateBuyOrder_ValidData_ToBeSuccessful()
  {
   //Arrange
   BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
    .Create();

   //Mock
   BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
   _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

   //Act
   BuyOrderResponse buyOrderResponseFromCreate = await _stocksBuyOrdersService.CreateBuyOrder(buyOrderRequest);

   //Assert
   buyOrder.BuyOrderID = buyOrderResponseFromCreate.BuyOrderID;
   BuyOrderResponse buyOrderResponse_expected = buyOrder.ToBuyOrderResponse();
   buyOrderResponseFromCreate.BuyOrderID.Should().NotBe(Guid.Empty);
   buyOrderResponseFromCreate.Should().Be(buyOrderResponse_expected);
  }


  #endregion




  #region CreateSellOrder


  [Fact]
  public async Task CreateSellOrder_NullSellOrder_ToBeArgumentNullException()
  {
   //Arrange
   SellOrderRequest? sellOrderRequest = null;

   //Mock
   SellOrder sellOrderFixture = _fixture.Build<SellOrder>()
    .Create();
   _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrderFixture);

   //Act
   Func<Task> action = async () =>
   {
    await _stocksSellOrdersService.CreateSellOrder(sellOrderRequest);
   };

   //Assert
   await action.Should().ThrowAsync<ArgumentNullException>();
  }



  [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
  [InlineData(0)] //passing parameters to the tet method
  public async Task CreateSellOrder_QuantityIsLessThanMinimum_ToBeArgumentException(uint sellOrderQuantity)
  {
   //Arrange
   SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
    .With(temp => temp.Quantity, sellOrderQuantity)
    .Create();

   //Mock
   SellOrder sellOrder = sellOrderRequest.ToSellOrder();
   _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

   //Act
   Func<Task> action = async () =>
   {
    await _stocksSellOrdersService.CreateSellOrder(sellOrderRequest);
   };

   //Assert
   await action.Should().ThrowAsync<ArgumentException>();
  }


  [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
  [InlineData(100001)] //passing parameters to the tet method
  public async Task CreateSellOrder_QuantityIsGreaterThanMaximum_ToBeArgumentException(uint sellOrderQuantity)
  {
   //Arrange
   SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
    .With(temp => temp.Quantity, sellOrderQuantity)
    .Create();

   //Mock
   SellOrder sellOrder = sellOrderRequest.ToSellOrder();
   _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

   //Act
   Func<Task> action = async () =>
   {
    await _stocksSellOrdersService.CreateSellOrder(sellOrderRequest);
   };

   //Assert
   await action.Should().ThrowAsync<ArgumentException>();
  }


  [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
  [InlineData(0)] //passing parameters to the tet method
  public async Task CreateSellOrder_PriceIsLessThanMinimum_ToBeArgumentException(uint sellOrderPrice)
  {
   //Arrange
   SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
    .With(temp => temp.Price, sellOrderPrice)
    .Create();

   //Mock
   SellOrder sellOrder = sellOrderRequest.ToSellOrder();
   _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

   //Act
   Func<Task> action = async () =>
   {
    await _stocksSellOrdersService.CreateSellOrder(sellOrderRequest);
   };

   //Assert
   await action.Should().ThrowAsync<ArgumentException>();
  }


  [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
  [InlineData(10001)] //passing parameters to the tet method
  public async Task CreateSellOrder_PriceIsGreaterThanMaximum_ToBeArgumentException(double buyOrderPrice)
  {
   //Arrange
   SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
    .With(temp => temp.Price, buyOrderPrice)
    .Create();

   //Mock
   SellOrder sellOrder = sellOrderRequest.ToSellOrder();
   _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

   //Act
   Func<Task> action = async () =>
   {
    await _stocksSellOrdersService.CreateSellOrder(sellOrderRequest);
   };

   //Assert
   await action.Should().ThrowAsync<ArgumentException>();
  }


  [Fact]
  public async Task CreateSellOrder_StockSymbolIsNull_ToBeArgumentException()
  {
   //Arrange
   SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
    .With(temp => temp.StockSymbol, null as string)
    .Create();

   //Mock
   SellOrder sellOrder = sellOrderRequest.ToSellOrder();
   _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

   //Act
   Func<Task> action = async () =>
   {
    await _stocksSellOrdersService.CreateSellOrder(sellOrderRequest);
   };

   //Assert
   await action.Should().ThrowAsync<ArgumentException>();
  }


  [Fact]
  public async Task CreateSellOrder_DateOfOrderIsLessThanYear2000_ToBeArgumentException()
  {
   //Arrange
   SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
    .With(temp => temp.DateAndTimeOfOrder, Convert.ToDateTime("1999-12-31"))
    .Create();

   //Mock
   SellOrder sellOrder = sellOrderRequest.ToSellOrder();
   _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

   //Act
   Func<Task> action = async () =>
   {
    await _stocksSellOrdersService.CreateSellOrder(sellOrderRequest);
   };

   //Assert
   await action.Should().ThrowAsync<ArgumentException>();
  }


  [Fact]
  public async Task CreateSellOrder_ValidData_ToBeSuccessful()
  {
   //Arrange
   SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
    .Create();


   //Mock
   SellOrder sellOrder = sellOrderRequest.ToSellOrder();
   _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

   //Act
   SellOrderResponse sellOrderResponseFromCreate = await _stocksSellOrdersService.CreateSellOrder(sellOrderRequest);

   //Assert
   sellOrder.SellOrderID = sellOrderResponseFromCreate.SellOrderID;
   SellOrderResponse sellOrderResponse_expected = sellOrder.ToSellOrderResponse();
   sellOrderResponseFromCreate.SellOrderID.Should().NotBe(Guid.Empty);
   sellOrderResponseFromCreate.Should().Be(sellOrderResponse_expected);
  }


  #endregion




  #region GetBuyOrders

  //The GetAllBuyOrders() should return an empty list by default
  [Fact]
  public async Task GetAllBuyOrders_DefaultList_ToBeEmpty()
  {
   //Arrange
   List<BuyOrder> buyOrders = new List<BuyOrder>();

   //Mock
   _stocksRepositoryMock.Setup(temp => temp.GetBuyOrders()).ReturnsAsync(buyOrders);

   //Act
   List<BuyOrderResponse> buyOrdersFromGet = await _stocksBuyOrdersService.GetBuyOrders();

   //Assert
   Assert.Empty(buyOrdersFromGet);
  }


  [Fact]
  public async Task GetAllBuyOrders_WithFewBuyOrders_ToBeSuccessful()
  {
   //Arrange
   List<BuyOrder> buyOrder_requests = new List<BuyOrder>() {
    _fixture.Build<BuyOrder>().Create(),
    _fixture.Build<BuyOrder>().Create()
   };

   List<BuyOrderResponse> buyOrders_list_expected = buyOrder_requests.Select(temp => temp.ToBuyOrderResponse()).ToList();
   List<BuyOrderResponse> buyOrder_response_list_from_add = new List<BuyOrderResponse>();

   //Mock
   _stocksRepositoryMock.Setup(temp => temp.GetBuyOrders()).ReturnsAsync(buyOrder_requests);

   //Act
   List<BuyOrderResponse> buyOrders_list_from_get = await _stocksBuyOrdersService.GetBuyOrders();


   //Assert
   buyOrders_list_from_get.Should().BeEquivalentTo(buyOrders_list_expected);
  }

  #endregion




  #region GetSellOrders

  //The GetAllSellOrders() should return an empty list by default
  [Fact]
  public async Task GetAllSellOrders_DefaultList_ToBeEmpty()
  {
   //Arrange
   List<SellOrder> sellOrders = new List<SellOrder>();

   //Mock
   _stocksRepositoryMock.Setup(temp => temp.GetSellOrders()).ReturnsAsync(sellOrders);

   //Act
   List<SellOrderResponse> sellOrdersFromGet = await _stocksSellOrdersService.GetSellOrders();

   //Assert
   Assert.Empty(sellOrdersFromGet);
  }


  [Fact]
  public async Task GetAllSellOrders_WithFewSellOrders_ToBeSuccessful()
  {
   //Arrange
   List<SellOrder> sellOrder_requests = new List<SellOrder>() {
    _fixture.Build<SellOrder>().Create(),
    _fixture.Build<SellOrder>().Create()
   };

   List<SellOrderResponse> sellOrders_list_expected = sellOrder_requests.Select(temp => temp.ToSellOrderResponse()).ToList();
   List<SellOrderResponse> sellOrder_response_list_from_add = new List<SellOrderResponse>();

   //Mock
   _stocksRepositoryMock.Setup(temp => temp.GetSellOrders()).ReturnsAsync(sellOrder_requests);

   //Act
   List<SellOrderResponse> sellOrders_list_from_get = await _stocksSellOrdersService.GetSellOrders();


   //Assert
   sellOrders_list_from_get.Should().BeEquivalentTo(sellOrders_list_expected);
  }

  #endregion

 }
}


