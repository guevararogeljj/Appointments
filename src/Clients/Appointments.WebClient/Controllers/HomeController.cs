using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Appointments.WebClient.Models;
using System.Security.Claims;

namespace Appointments.WebClient.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HomeController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var users = new List<UserViewModel>();
        var client = _httpClientFactory.CreateClient("ApiClient");
        var accessToken = HttpContext.User.FindFirst("AccessToken")?.Value;
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
        // Get all users
        //apply token to the request
        
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var usersResponse = await client.GetAsync($"api/v1/Users/{userId}");
        usersResponse.EnsureSuccessStatusCode();
        users.Add(JsonConvert.DeserializeObject<UserViewModel>(await usersResponse.Content.ReadAsStringAsync()));
        ViewBag.Users = users;

        // Get user's chat rooms
        var chatRoomsResponse = await client.GetAsync("api/v1/Chat/my-rooms");
        chatRoomsResponse.EnsureSuccessStatusCode();
        var chatRooms = JsonConvert.DeserializeObject<List<ChatRoomViewModel>>(await chatRoomsResponse.Content.ReadAsStringAsync());
        ViewBag.ChatRooms = chatRooms;

        return View();
    }
}

public class ChatRoomViewModel
{
    public Guid Id { get; set; }
    public UserViewModel User1 { get; set; }
    public UserViewModel User2 { get; set; }
}
