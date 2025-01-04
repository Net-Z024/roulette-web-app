using Grpc.Net.Client;
using GrpcService1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CaseBattleNew.Pages
{
    public class OpenCaseModel : PageModel
    {
        public List<ChestDto> Chests { get; set; } = new();
        public async Task<JsonResult> OnPostOpenCase()
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:5003");
            var client = new ChestGrpc.ChestGrpcClient(channel);

            // Make a request to open a case
            var response = await client.OpenChestAsync(new OpenChestRequest());
            Console.WriteLine("TEST AAAAAAAAAAAAA" + response.ReceivedItem);
            // Return the result as JSON
            return new JsonResult(new
            {
                Index = response.ReceivedItem.Id, // The index of the segment
                Name = response.ReceivedItem.Name ,   // The name of the selected item
                ImageUrl = response.ReceivedItem.ImageUrl // Optional, if you want to display an image
            });
        }
        public async Task OnGet()
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:5003");
            var client = new ChestGrpc.ChestGrpcClient(channel);

                // Make a request to open a case
                var response = await client.GetChestsAsync(new GetChestsRequest());

                var items = response.Chests.FirstOrDefault()!.PossibleItems.ToList();
                Console.WriteLine("Retrieved Items:");
                foreach (var item in items)
                {
                    Console.WriteLine($"Name: {item.Item.Name}");
                    Console.WriteLine($"Image URL: {item.Item.ImageUrl}");
                    Console.WriteLine("------------------------");
                }

                Chests = response.Chests.ToList();
        }
    }
}
