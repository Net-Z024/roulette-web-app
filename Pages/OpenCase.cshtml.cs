using Grpc.Net.Client;
using GrpcService1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Channels;

namespace CaseBattleNew.Pages
{
    public class OpenCaseModel : PageModel
    {
        public List<ItemDto> ChestItems { get; set; } = new List<ItemDto>();

        public List<ChestDto> Chests { get; set; } = new List<ChestDto>();

        public async Task<JsonResult> OnPostGetChestItems(int chestId)
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:5003");
            var client = new ItemGrpc.ItemGrpcClient(channel);

            // Make a request to open a case
            var response = await client.GetChestItemsAsync(new GetChestItemsRequest { ChestId = 2 });

            var items = response.Items.ToList();
            Console.WriteLine("Retrieved Items:");
            foreach (var item in items)
            {
                Console.WriteLine($"Name: {item.Name}");
                Console.WriteLine($"Image URL: {item.ImageUrl}");
                Console.WriteLine("------------------------");
            }
            return new JsonResult(new
            {
                items = items
            });
        }
        public async Task<JsonResult> OnPostOpenCase(int chestId)
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:5003");
            var client = new ChestGrpc.ChestGrpcClient(channel);

            // Make a request to open a chest
            var response = await client.OpenChestAsync(new OpenChestRequest { UserId = 1, ChestId = chestId });

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

            // Retrieve chests
            var chestClient = new ChestGrpc.ChestGrpcClient(channel);
            var chestResponse = await chestClient.GetChestsAsync(new GetChestsRequest { });
            Chests = chestResponse.Chests.ToList();

            Console.WriteLine("Retrieved Chests:");
            foreach (var chest in Chests)
            {
                Console.WriteLine($"Chest ID: {chest.Id}, Name: {chest.Name}, Price: {chest.Price}");
            }

           
        }
    }
}
