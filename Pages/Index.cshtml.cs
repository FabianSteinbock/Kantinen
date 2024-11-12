using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc.RazorPages;




namespace IBAS_kantine
{
    public class IndexModel : PageModel
    {
        private readonly TableClient _TableClient;

        public IndexModel (TableClient tableClient)
        {
            _TableClient = tableClient;
        }


        // Make MenuItems a public property so it can be accessed in Razor
        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
        public void OnGet()
        {
            Pageable<TableEntity> entities = _TableClient.Query<TableEntity>();

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
