using Grpc.Net.Client;
using gRPCServer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CaseBattleNew.Pages
{
    public class OpenCaseModel : PageModel
    {
        public List<CaseItem> CaseItems { get; set; } = new List<CaseItem>();
        public async Task<JsonResult> OnPostOpenCase()
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5051");
            var client = new CaseOpener.CaseOpenerClient(channel);

            // Make a request to open a case
            var response = await client.OpenCaseAsync(new OpenCaseRequest { CaseId = "1" });
            Console.WriteLine("TEST AAAAAAAAAAAAA" + response.Name);
            // Return the result as JSON
            return new JsonResult(new
            {
                Index = response.Index, // The index of the segment
                Name = response.Name,   // The name of the selected item
                ImageUrl = response.ImageUrl // Optional, if you want to display an image
            });
        }
        public async Task OnGet()
        {
        
                using var channel = GrpcChannel.ForAddress("https://localhost:5051");
                var client = new CaseOpener.CaseOpenerClient(channel);

                // Make a request to open a case
                var response = await client.GetCaseItemsAsync(new GetCaseItemsRequest { CaseId = "2" });

                CaseItems = response.Items.ToList();
                Console.WriteLine("Retrieved Items:");
                foreach (var item in response.Items)
                {
                    Console.WriteLine($"Name: {item.Name}");
                    Console.WriteLine($"Image URL: {item.ImageUrl}");
                    Console.WriteLine("------------------------");
                }

            Console.WriteLine("Hej"+CaseItems[0].Name);
            
        }
    }
}
