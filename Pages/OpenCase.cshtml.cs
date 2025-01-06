using Grpc.Net.Client;
using GrpcService1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CaseBattleNew.Pages
{
    public class OpenCaseModel : PageModel
    {
        public List<ItemDto> CaseItems { get; set; } = new List<ItemDto>();
        public async Task<JsonResult> OnPostOpenCase()
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:5003");
            var client = new ChestGrpc.ChestGrpcClient(channel);

            // Make a request to open a case
            var response = await client.OpenChestAsync(new OpenChestRequest{ UserId = 1, ChestId = 2 });
            Console.WriteLine("TEST AAAAAAAAAAAAA" + response.ReceivedItem);
            // Return the result as JSON
            return new JsonResult(new
            {
                Index = response.ReceivedItem.Id, // The index of the segment
                Name = response.ReceivedItem.Name,   // The name of the selected item
                ImageUrl = response.ReceivedItem.ImageUrl // Optional, if you want to display an image
            });
        }
        public async Task OnGet()
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:5003");
            var client = new ItemGrpc.ItemGrpcClient(channel);

                // Make a request to open a case
                var response = await client.GetChestItemsAsync(new GetChestItemsRequest { ChestId=2});
            
                var items = response.Items.ToList();
                Console.WriteLine("Retrieved Items:");
                foreach (var item in items)
                {
                    Console.WriteLine($"Name: {item.Name}");
                    Console.WriteLine($"Image URL: {item.ImageUrl}");
                    Console.WriteLine("------------------------");
                }
            CaseItems = items;
                
        }
    }
}
