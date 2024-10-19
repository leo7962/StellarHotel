using Microsoft.AspNetCore.Mvc;
using Moq;
using StellarHotel.Controllers;
using StellarHotel.Dtos;
using StellarHotel.Interfaces;

namespace StellarHotelTests;

public class ReservationsControllerTests
{
    private readonly ReservationsController _controller;
    private readonly Mock<IReservationService> _mockReservationService;
    private readonly Mock<IRoomService> _mockRoomService;

    public ReservationsControllerTests()
    {
        _mockReservationService = new Mock<IReservationService>();
        _mockRoomService = new Mock<IRoomService>();
        _controller = new ReservationsController(_mockReservationService.Object, _mockRoomService.Object);
    }

    [Fact]
    public async Task GetAvailableRooms_ReturnsOkResult_WithListOfRooms()
    {
        //Arrange
        var roomSearchDto = new RoomSearchDto
        {
            CheckInDate = DateTime.Now,
            CheckOutDate = DateTime.Now.AddDays(2),
            NumberOfGuests = 2,
            IncludesBreakfast = true,
            RoomType = "Junior Suite"
        };

        var rooms = new List<RoomDto>
        {
            new()
            {
                Id = 1, Type = "Junior Suite", MaxOccupancy = 2, NumberOfBeds = 1, HasOceanView = true,
                BaseRate = 100
            },
            new()
            {
                Id = 2, Type = "King Suite", MaxOccupancy = 2, NumberOfBeds = 1, HasOceanView = false,
                BaseRate = 150
            }
        };

        _mockRoomService.Setup(service => service.GetAvailableRoomsAsync(roomSearchDto)).ReturnsAsync(rooms);

        //act
        var result = await _controller.GetAvailableRooms(roomSearchDto);

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<RoomDto>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task CreateReservation_ReturnsCreatedAtAction_WithReservation()
    {
        // Arrange
        var createReservationDto = new CreateRservationDto
        {
            RoomId = 1,
            CheckInDate = DateTime.Now,
            CheckOutDate = DateTime.Now.AddDays(3),
            NumberOfGuests = 2,
            IncludesBreakfast = true
        };

        var reservationDto = new ReservationDto
        {
            Id = 1,
            RoomId = createReservationDto.RoomId,
            CheckInDate = createReservationDto.CheckInDate,
            CheckOutDate = createReservationDto.CheckOutDate,
            NumberOfGuests = createReservationDto.NumberOfGuests,
            IncludesBreakfast = createReservationDto.IncludesBreakfast,
            TotalPrice = 250
        };

        _mockReservationService.Setup(service => service.CreateReservationAsync(createReservationDto))
            .ReturnsAsync(reservationDto);

        // Act
        var result = await _controller.CreateReservation(createReservationDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnValue = Assert.IsType<ReservationDto>(createdAtActionResult.Value);
        Assert.Equal(1, returnValue.Id);
        Assert.Equal(createReservationDto.RoomId, returnValue.Id);
    }

    [Fact]
    public async Task GetReservationById_ReturnsOkResult_WithReservation()
    {
        //Arrange
        var reservationId = 1;
        var reservationDto = new ReservationDto
        {
            Id = reservationId,
            RoomId = 1,
            CheckInDate = DateTime.Now,
            CheckOutDate = DateTime.Now.AddDays(2),
            NumberOfGuests = 2,
            IncludesBreakfast = true,
            TotalPrice = 200
        };

        _mockReservationService.Setup(service => service.GetReservationByIdAsync(reservationId))
            .ReturnsAsync(reservationDto);

        //Act

        var result = await _controller.GetReservationById(reservationId);

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<ReservationDto>(okResult.Value);
        Assert.Equal(reservationId, returnValue.Id);
    }

    [Fact]
    public async Task GetReservationById_ReturnsNotFound_WhenReservationDoesNotExist()
    {
        // Arrange
        var reservationId = 1;

        _mockReservationService.Setup(service => service.GetReservationByIdAsync(reservationId))
            .ReturnsAsync((ReservationDto)null);

        //Act
        var result = await _controller.GetReservationById(reservationId);

        //Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetAllReservations_ReturnsOkResult_WithListOfReservations()
    {
        // Arrange
        var reservations = new List<ReservationDto>
        {
            new()
            {
                Id = 1, RoomId = 1, CheckInDate = DateTime.Now, CheckOutDate = DateTime.Now.AddDays(2),
                NumberOfGuests = 2, IncludesBreakfast = true, TotalPrice = 200
            },
            new()
            {
                Id = 2, RoomId = 2, CheckInDate = DateTime.Now, CheckOutDate = DateTime.Now.AddDays(3),
                NumberOfGuests = 3, IncludesBreakfast = false, TotalPrice = 300
            }
        };

        _mockReservationService.Setup(service => service.GetAllReservationsAsync())
            .ReturnsAsync(reservations);

        // Act
        var result = await _controller.GetAllReservations();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<ReservationDto>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task CancelReservation_ReturnsNoContent_WhenCancellationIsSuccessful()
    {
        // Arrange
        var reservationId = 1;
        _mockReservationService.Setup(service => service.CancelReservationAsync(reservationId))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.CancelReservation(reservationId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task CancelReservation_ReturnsNotFound_WhenReservationDoesNotExist()
    {
        // Arrange
        var reservationId = 1;
        _mockReservationService.Setup(service => service.CancelReservationAsync(reservationId))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.CancelReservation(reservationId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}