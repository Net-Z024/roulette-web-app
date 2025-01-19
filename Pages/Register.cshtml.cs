using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Grpc.Net.Client;
using CaseBattleNew;
using System.Threading.Tasks;
using GrpcService1;
using Microsoft.AspNetCore.Identity;

namespace CaseBattleNew.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly GrpcChannel _channel;
        private readonly UserGrpc.UserGrpcClient _grpcClient;
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }


        public RegisterModel()
        {
            _channel = GrpcChannel.ForAddress("http://localhost:5003"); 
            _grpcClient = new UserGrpc.UserGrpcClient(_channel);
        }

        public async Task<IActionResult> OnPostAsync(string username,string password,string email)
        {
            Console.WriteLine("hello register");
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Please correct the errors below and try again.";
                return Page();
            }
            Console.WriteLine("username:" + username + "password" + password);
            var request = new RegisterRequest
            {
                Username = username,
                Password = password,
                Email = email
            };
            
            try
            {
                var response = await _grpcClient.RegisterAsync(request);

                if (response.Success)
                {
                    ViewData["Message"] = "Registration successful! You can now log in.";
                    return RedirectToPage("/Login");
                }
                else
                {
                    ViewData["Message"] = response.Message;
                    ModelState.AddModelError(string.Empty, response.Message);
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Registration failed, please try again.");
            }

            return Page();
        }
    }
}
