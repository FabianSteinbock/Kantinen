using Azure.Data.Tables;
using Azure;
using IBAS_kantine;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Azure.Core;


namespace IBAS_kantine;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    // Make menuItems a public property so it can be accessed in Razor
    public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

    public void OnGet()
    {
        var tableName = "KantineMenuen";
        var accountName = "kantinemenustorage"; // Replace with your Azure Storage accoun
        var tableEndpoint = $"https://kantinemenustorage.table.core.windows.net/KantineMenuen";

        // Initialize TableClient with TableSharedKeyCredential
        var credential = new TableSharedKeyCredential(accountName, accountKey);
        var tableClient = new TableClient(new Uri(tableEndpoint), tableName, credential);


        Pageable<TableEntity> entities = tableClient.Query<TableEntity>();

        // Define the day order
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

        // Sort MenuItems based on the dayOrder list
        MenuItems = MenuItems.OrderBy(item => dayOrder.IndexOf(item.Dag)).ToList();
    }

}
