using Azure.Data.Tables;
using Azure.Identity;
var builder = WebApplication.CreateBuilder(args);




var tableName = "KantineMenuen";
var storageAccountUrl = new Uri("https://kantinemenustorage.table.core.windows.net");

// Register TableClient with DefaultAzureCredential for managed identity
builder.Services.AddSingleton<TableClient>(new TableClient(storageAccountUrl, tableName, new DefaultAzureCredential()));

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
