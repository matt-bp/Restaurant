using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Website.Models;
using Lib.Models;
using Lib.Services;

namespace Website.Controllers;

public class RestaurantController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IOpenRestaurantService _openRestaurantService;

    public RestaurantController(ILogger<HomeController> logger, IOpenRestaurantService openRestaurantService)
    {
        _logger = logger;
        _openRestaurantService = openRestaurantService;
    }

    public IActionResult Open(DateTime date, DateTime time)
    {
        var selectedDate = DateOnly.FromDateTime(date);
        var selectedTime = TimeOnly.FromDateTime(time);

        var openRestaurants = _openRestaurantService.GetOpenRestaurants(selectedDate.DayOfWeek, selectedTime);

        var model = new OpenViewModel {
            Date = DateOnly.FromDateTime(date),
            Time = TimeOnly.FromDateTime(time),
            OpenRestaurants = openRestaurants
        };

        return View(model);
    }
}
