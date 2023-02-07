using ButterflyStore.Server.Extensions;
using ButterflyStore.Shared.Validators;
using FluentValidation;
using Treblle.Net.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//Adding Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Adding the configured APP DB CONTEXT 
builder.Services.AddAppDbContext(builder.Configuration);


//Register the IProductService
builder.Services.AddProductsService();

//Register the ICategoryService
builder.Services.AddCategoryService();

//Register the IAuthService
builder.Services.AddAuthService();

//Add Identity.
builder.Services.AddAndConfigureIdentity();

//Add Fluent Validations.
builder.Services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();

//Add Authentication And JWT Bearer.
builder.Services.AddAndConfigureAuthentication(builder.Configuration);

//Add API Versioning.
builder.Services.AddApiVersioningService();


builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseTreblle();
app.UseCors(options =>
{
    options.AllowAnyHeader();
    options.AllowAnyMethod();
    options.AllowAnyOrigin();
});
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
