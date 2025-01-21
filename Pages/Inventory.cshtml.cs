using Grpc.Net.Client;
using GrpcService1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace CaseBattleNew.Pages
{
    public class InventoryModel : PageModel
    {
        
        public class GroupedUserItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string ImageUrl { get; set; }
            public decimal Value { get; set; }
            public int Quantity { get; set; }

            public List<int> ItemIds { get; set; } = new List<int>();

        }

 
        public List<GroupedUserItem> GroupedUserItems { get; set; } = new List<GroupedUserItem>();

        public async Task OnGet()
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:5003");
            var itemsClient = new ItemGrpc.ItemGrpcClient(channel);

           
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

            if (!string.IsNullOrEmpty(userIdClaim))
            {
                var itemsResponse = await itemsClient.GetUserItemsAsync(new GetUserItemsRequest { UserId = Int32.Parse(userIdClaim) });


                GroupedUserItems = itemsResponse.Items
                .GroupBy(item => item.Item.Id)
                .Select(group => new GroupedUserItem
                {
                    Id = group.Key,
                    Name = group.First().Item.Name,
                    ImageUrl = group.First().Item.ImageUrl,
                    Value = (decimal)group.First().Item.Value,
                    Quantity = group.Count(),
                    ItemIds = group.Select(item => item.Id).ToList()

                })
                .ToList();

            }
        }
        public async Task<JsonResult> OnPostSellItem(int userItemId)
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:5003");
            var client = new ItemGrpc.ItemGrpcClient(channel);

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return new JsonResult(new { Success = false, Message = "User not authenticated." });
            }

            var response = await client.SellItemAsync(new SellItemRequest
            {
                UserId = int.Parse(userIdClaim),
                UserItemId = userItemId
            });

          
            return new JsonResult(new
            {
                Success = response.Success,
                NewBalance = response.NewBalance
            });
        }

    }
}
