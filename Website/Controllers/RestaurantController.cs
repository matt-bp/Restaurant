using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Website.Models;

namespace Website.Controllers;

public class RestaurantController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public RestaurantController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Open(DateTime date, DateTime time)
    {
        Console.WriteLine("hello!");

        var model = new OpenViewModel {
            Date = DateOnly.FromDateTime(date),
            Time = TimeOnly.FromDateTime(time),
            OpenRestaurants = new List<Restaurant> {
                new Restaurant{ 
                    Name = "Bob's",
                    Availabilities = new List<Restaurant.Availability> {
                        new Restaurant.Availability {
                            Day = DayOfWeek.Monday,
                            Open = TimeOnly.Parse("09:00"),
                            Close = TimeOnly.Parse("20:00")
                        }
                    }
                }
            }
        };

        return View(model);
    }
}
