
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi;


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins, builder =>
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


    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo API");
        c.RoutePrefix = string.Empty;
    });


app.MapGet("/item", async (ToDoDbContext db)=>{
    try{
        var items=await db.Items.ToListAsync();
        return Results.Ok(items);
    }
    catch(Exception ex){
        return Results.Problem("An error occurred while retrieving items: " + ex.Message);
    }
});
app.MapGet("/item/{id}", async (ToDoDbContext db, int id) => { 
    try{
    var eItem=await db.Items.FindAsync(id);
    if(eItem ==null )
    return Results.NotFound();
    return Results.Ok(eItem);}
    catch(Exception ex){
        return Results.Problem("An error occurred while retrieving item: " + ex.Message);
    }
    
 });
 app.MapPost("/items", async (ToDoDbContext db, Item item) => {
    try {
        if (item == null)
            return Results.BadRequest("Item cannot be null");
        db.Items.Add(item);
        await db.SaveChangesAsync();
        return Results.Created($"/items/{item.Id}", item);
    }
    catch (Exception ex) {
        return Results.Problem("An error occurred while creating item: " + ex.Message+ "\n"+ex.InnerException?.Message);
    }
});

app.MapPut("/item/{id}", async (ToDoDbContext db,int id,[FromBody]Item item)=>{
   try{

   
    var eItem=await db.Items.FindAsync(id);
    if(eItem ==null )
    return Results.NotFound();
    eItem.Name=item.Name;
    eItem.IsComplete=item.IsComplete;
    await db.SaveChangesAsync();
    return Results.Ok(eItem);
   }
    catch(Exception ex){
          return Results.Problem("An error occurred while updating item: " + ex.Message);
     }

});
app.MapDelete("/item/{id}", async(ToDoDbContext db,int id) =>{
    try{
    if(id<=0)
    return Results.BadRequest("Id must be greater than 0");
    var eItem=await db.Items.FindAsync(id);
    if(eItem ==null )
    return Results.NotFound();
    var item=db.Items.Remove(eItem);
    
Console.WriteLine(item);
    await db.SaveChangesAsync();
    return Results.Ok();
    }
    catch(Exception ex){
        return Results.Problem("An error occurred while deleting item: " + ex.Message);
    }
});
app.MapGet("/", () => "ToDo API is runing");

app.Run();

