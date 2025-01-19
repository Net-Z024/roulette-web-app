using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Grpc.Net.Client;
using CaseBattleNew;
using System.Threading.Tasks;
using GrpcService1;
using System;

namespace CaseBattleNew.Pages
{
    public class LoginModel : PageModel
    {
        private readonly GrpcChannel _channel;
        private readonly UserGrpc.UserGrpcClient _grpcClient;

        public string Username { get; set; }
        public string Password { get; set; }

        public LoginModel()
        {
            _channel = GrpcChannel.ForAddress("http://localhost:5003");
            _grpcClient = new UserGrpc.UserGrpcClient(_channel);
        }

        public void OnGet()
        {
            Console.WriteLine("Start");
        }

        public async Task<IActionResult> OnPostAsync(string username, string password)
        {
            Console.WriteLine("Processing login...");
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("Validation error: Username and Password cannot be empty.");
                ModelState.AddModelError(string.Empty, "Username and Password cannot be empty.");
                return Page();
            }
            Console.WriteLine("USERNAME:" + username);
            var request = new LoginRequest
            {
                Username = username,
                Password = password
            };

            try
            {
                Console.WriteLine($"Sending login request to gRPC server with Username: {request.Username}");
                var response = await _grpcClient.LoginAsync(request);

                if (response.Success)
                {
                    HttpContext.Response.Cookies.Append("AuthToken", response.Token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTimeOffset.UtcNow.AddHours(1)
                    });

                    Console.WriteLine("Login successful, redirecting to the homepage.");
                    return RedirectToPage("/Index");
                }
                else
                {
                    Console.WriteLine($"Login failed: {response.Message}");
                    ModelState.AddModelError(string.Empty, response.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during login: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Login failed, please try again.");
            }

            return Page();
        }
    }
}
