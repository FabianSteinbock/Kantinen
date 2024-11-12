using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Azure.Storage.Blobs;
using static System.Net.WebRequestMethods;



namespace IBAS_kantine
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        // Make MenuItems a public property so it can be accessed in Razor
        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

        public void OnGet()
        {
            //Her bruges Connectionstring
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=kantinemenustorage;AccountKey=qMlBcp7yi0VYR6j+ZTq7h2cTSWeQSajA6ixa6v9GVlxw4GbHK5sdprPJABtDn00Evf7RKoQY7aDI+AStnTn1OQ==;BlobEndpoint=https://kantinemenustorage.blob.core.windows.net/;FileEndpoint=https://kantinemenustorage.file.core.windows.net/;TableEndpoint=https://kantinemenustorage.table.core.windows.net/;QueueEndpoint=https://kantinemenustorage.queue.core.windows.net/";
             
            /*bud på at lave det uden connectionstring - udfordringen er, at den returner null hver gang. 
            var tableUri = Environment.GetEnvironmentVariable("AZURE_STORAGETABLE_CONNECTIONSTRING");
            */
            
            var tableName = "KantineMenuen";
            TableClient tableClient = new TableClient(connectionString, tableName);
            Pageable<TableEntity> entities = tableClient.Query<TableEntity>();


            var dayOrder = new List<string> { "mandag", "tirsdag", "onsdag", "torsdag", "fredag" };
            foreach (TableEntity entity in entities)
            {
                var menuItem = new MenuItem
                {
                    Dag = entity["RowKey"].ToString().ToLower(),
                    KoldRet = entity["KoldRet"].ToString(),
                    VarmRet = entity["VarmRet"].ToString()
                };
                MenuItems.Add(menuItem);
            }
            MenuItems = MenuItems.OrderBy(item => dayOrder.IndexOf(item.Dag)).ToList();




          
        }
    }
}
