using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Azure.Storage.Blobs;



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
            //Blob connection string hentes her
            var connectionString =Environment.GetEnvironmentVariable("BlobStorageConnectionString");

            //opretter blobserviceclient opjekt
            var blobServiceClient = new BlobServiceClient (connectionString);
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
