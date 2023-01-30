using ButterflyStore.Server.Extensions;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

//Adding Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Adding the configured APP DB CONTEXT 
builder.Services.AddAppDbContext(builder.Configuration);

//Registering Auto Mapper
builder.Services.RegisterAutoMapper();

//Register the IProductService
builder.Services.AddProductsService();

//Register the ICategoryService
builder.Services.AddCategoryService();

//Add Identity.
builder.Services.AddAndConfigureIdentity();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
