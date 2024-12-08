using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CaseBattleNew.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGet()
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5051");
            var client = new CaseOpener.CaseOpenerClient(channel);
            Console.WriteLine("TEEEEEEEEEEEEEEEEEEEEST");
            // Make a request to open a case
            var response = await client.OpenCaseAsync(new OpenCaseRequest { CaseId = "1" });

            // Return the result as JSON
            Console.WriteLine("HJALO "+ response.Name);
        }
    }
}