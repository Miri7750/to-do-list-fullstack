// // var builder = WebApplication.CreateBuilder(args);
// // var app = builder.Build();

// // app.MapGet("/", () => "Hello World!");

// // app.Run();

// using Microsoft.EntityFrameworkCore;
// using TodoApi;

// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.
// builder.Services.AddEndpointsApiExplorer(); // Required for endpoint discovery
// builder.Services.AddSwaggerGen(); // Add Swagger services

// // Register the DbContext with MySQL
// builder.Services.AddDbContext<ToDoDbContext>(options =>
//     options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"),
//     new MySqlServerVersion(new Version(8, 0, 2))));

// var app = builder.Build(); // Build the application

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger(); // Enable middleware to serve generated Swagger as a JSON endpoint
//     app.UseSwaggerUI(options => // Enable middleware to serve swagger-ui
//     {
//         options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
//         options.RoutePrefix = string.Empty; // Serve the UI at the app's root
//     });
// }

// // Define your API endpoints
// app.MapGet("/item", async (ToDoDbContext context) =>
// {
//     var items = await context
//     .Items
//     .ToListAsync(); // Fetch items from the database
//     return items; // Return the items
// });

// app.MapGet("/item/{id}", async (int id, ToDoDbContext context) =>
// {
//     var item = await context.Items.FindAsync(id); // Fetch item from the database
//     return item is not null ? Results.Ok(item) : Results.NotFound(); // Return the item or 404 if not found
// });

// app.MapPost("/item", async (Item newItem, ToDoDbContext context) =>
// {
//     context.Items.Add(newItem); // Add the new item to the context
//     await context.SaveChangesAsync(); // Save changes to the database
//     return Results.Created($"/item/{newItem.Id}", newItem); // Return 201 Created response
// });

// app.MapPut("/item/{id}", async (int id, Item updatedItem, ToDoDbContext context) =>
// {
//     var existingItem = await context.Items.FindAsync(id); // Find the existing item
//     if (existingItem is null) return Results.NotFound(); // Return 404 if not found

//     existingItem.Name = updatedItem.Name; // Update the item properties
//     existingItem.IsComplete = updatedItem.IsComplete; // Update completion status
//     await context.SaveChangesAsync(); // Save changes to the database
//     return Results.NoContent(); // Return 204 No Content response
// });

// app.MapDelete("/item/{id}", async (int id, ToDoDbContext context) =>
// {
//     var existingItem = await context.Items.FindAsync(id); // Find the existing item
//     if (existingItem is null) return Results.NotFound(); // Return 404 if not found

//     context.Items.Remove(existingItem); // Remove the item from the context
//     await context.SaveChangesAsync(); // Save changes to the database
//     return Results.NoContent(); // Return 204 No Content response
// });

// app.Run(); // Run the application
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi;


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Load the connection string from appsettings.json and inject the TodoContext into the builder
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"),
    new MySqlServerVersion(new Version(8, 0, 2))));

//Add the Swagger generator
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Use the CORS policy
app.UseCors(MyAllowSpecificOrigins);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo API");
        c.RoutePrefix = string.Empty;
    });
}

app.MapGet("/item", async (ToDoDbContext db)=>{
    return Results.Ok(await db.Items.ToListAsync());
});
app.MapGet("/item/{id}", async (ToDoDbContext db, int id) => { 
    var eItem=await db.Items.FindAsync(id);
    if(eItem ==null )
    return Results.NotFound();
    return Results.Ok(eItem);
 });
app.MapPost("/items", async (ToDoDbContext db, Item item) =>
{
    db.Items.Add(item);  
    await db.SaveChangesAsync();    
    return Results.Created($"/items/{item.Id}", item);
});
app.MapPut("/item/{id}", async (ToDoDbContext db,int id,[FromBody]Item item)=>{
    var eItem=await db.Items.FindAsync(id);
    if(eItem ==null )
    return Results.NotFound();
    eItem.Name=item.Name;
    eItem.IsComplete=item.IsComplete;
    await db.SaveChangesAsync();
    return Results.Ok(eItem);

});
app.MapDelete("/item/{id}", async(ToDoDbContext db,int id) =>{
    var eItem=await db.Items.FindAsync(id);
    if(eItem ==null )
    return Results.NotFound();
    var item=db.Items.Remove(eItem);
Console.WriteLine(item);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.Run();

