using FlicoProject.BusinessLayer.Abstract;
using FlicoProject.BusinessLayer.Concrete;
using FlicoProject.DataAccessLayer.Abstract;
using FlicoProject.DataAccessLayer.Concrete;
using FlicoProject.DataAccessLayer.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<Context>();
builder.Services.AddScoped<IAirportDal, EFAirportDal>();
builder.Services.AddScoped<IAirportService, AirportManager>();
builder.Services.AddScoped<IWarehouseDal, EFWarehouseDal>();
builder.Services.AddScoped<IWarehouseService, WarehouseManager>();
builder.Services.AddScoped<IProductDal, EFProductDal>();
builder.Services.AddScoped<IProductService, ProductManager>();
builder.Services.AddScoped<IStockDetailDal, EFStockDEtailDal>();
builder.Services.AddScoped<IStockDetailService, StockDetailManager>();
builder.Services.AddScoped<IClosetDal, EFClosetDal>();
builder.Services.AddScoped<IClosetService, ClosetManager>();
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("FlicoApiCors", opts =>
    {
        opts.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
builder.Services.AddControllers();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("FlicoApiCors");
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();