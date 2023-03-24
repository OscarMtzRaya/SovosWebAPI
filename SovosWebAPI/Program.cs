using Microsoft.EntityFrameworkCore;
using SovosWebProject.Data;
using SovosWebProject.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/categories", async (ApplicationDBContext dBContext) =>
{
    return await dBContext.Categories.ToListAsync();
});

app.MapGet("categories/{id:int}", async (int id, ApplicationDBContext dBContext) => 
{
    if (id == 0) return Results.NotFound();
    Category? category = await dBContext.Categories.FindAsync(id);
    if (category == null) return Results.NotFound();
    return Results.Ok(category);
});

app.MapPost("/categories", async (Category category, ApplicationDBContext dbContext) =>
{
    if (category == null) return Results.BadRequest();
    await dbContext.Categories.AddAsync(category);
    await dbContext.SaveChangesAsync();
    return Results.Ok(category);
});

app.MapDelete("/categories/{id:int}", async (int id, ApplicationDBContext dbContext) => 
{
    if (id == 0) return Results.BadRequest();
    Category? category = await dbContext.Categories.FindAsync(id);
    
    if (category is null) return Results.NotFound();

    dbContext.Categories.Remove(category);
    await dbContext.SaveChangesAsync();
    return Results.Ok();
});

app.MapPut("/categories/{id:int}", async (int id, Category category, ApplicationDBContext dBContext) => 
{
    if (category.Id != id) return Results.NotFound("IDs do not match");
    if (id == 0) return Results.BadRequest("ID must no be zero");
    Category? remoteCategory = await dBContext.Categories.FindAsync(id);

    if (remoteCategory is null) return Results.NotFound("Category does not exist");
    remoteCategory.Name = category.Name;
    remoteCategory.DisplayOrder = category.DisplayOrder;
    remoteCategory.CreatedDateTime = category.CreatedDateTime;
    await dBContext.SaveChangesAsync();
    return Results.Ok(remoteCategory);
});

app.Run();
