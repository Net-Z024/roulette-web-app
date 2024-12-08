using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CaseBattleNew.Pages
{
    public class OpenCaseModel : PageModel
    {
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
    }
}
