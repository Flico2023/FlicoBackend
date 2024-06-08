using FlicoProject.BusinessLayer.Abstract;
using FlicoProject.BusinessLayer.Concrete;
using FlicoProject.BusinessLayer.Concrete.Mail;
using FlicoProject.BusinessLayer.Concrete.Validators.PostCartDtoValidtors;
using FlicoProject.BusinessLayer.Concrete.Validators.PostContactMessage;
using FlicoProject.BusinessLayer.Concrete.Validators.ProductValidators;
using FlicoProject.BusinessLayer.Validators;
using FlicoProject.DataAccessLayer.Abstract;
using FlicoProject.DataAccessLayer.Concrete;
using FlicoProject.DataAccessLayer.EntityFramework;
using FlicoProject.DtoLayer;
using FlicoProject.DtoLayer.ProductDTOs;
using FlicoProject.EntityLayer.Concrete;
using FlicoProject.WebApi.Mappers;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<Context>();
builder.Services.AddScoped<IFaqDal, EFFaqDal>();
builder.Services.AddScoped<IFaqService, FaqManager>();
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
builder.Services.AddScoped<IOutsourceDal, EFOutsourceDal>();
builder.Services.AddScoped<IOutsourceService, OutsourceManager>();
builder.Services.AddScoped<IOutsourceProductDal, EFOutsourceProductDal>();
builder.Services.AddScoped<IOutsourceProductService, OutsourceProductManager>();
builder.Services.AddScoped<IUserDal, EFUserDal>();
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<ICartDal, EFCartDal>();
builder.Services.AddScoped<ICartService, CartManager>();
builder.Services.AddScoped<IContactMessageDal, EFContactMessageDal>();
builder.Services.AddScoped<IContactMessageService, ContactMessageManager>();
builder.Services.AddScoped<IFavouriteDal, EFFavouriteDal>();
builder.Services.AddScoped<IOrderDal, EFOrderDal>();
builder.Services.AddScoped<IOrderService, OrderManager>();
builder.Services.AddScoped<IOrderProductDal, EFOrderProductDal>();
builder.Services.AddScoped<IOrderProductService, OrderProductManager>();

builder.Services.AddScoped<IOrderProductDal, EFOrderProductDal>();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("FlicoApiCors", opts =>
    {
        opts.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.TokenValidationParameters = new
    TokenValidationParameters
    {
        ValidIssuer = "http://localhost",
        ValidAudience = "http://localhost",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("FlicoIsAwesomeFlicoIsAwesomeFlicoIsAwesome")),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
    };
});
builder.Services.AddControllers();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("auth", new OpenApiSecurityScheme
    {
        Description = "Standart Authorization",
        In = ParameterLocation.Header,
        Name = "Auth",
        Type =SecuritySchemeType.ApiKey

    }) ;
    options.OperationFilter<SecurityRequirementsOperationFilter>();

});


//AUTO MAPPER
builder.Services.AddAutoMapper(typeof(AirportProfile));
builder.Services.AddAutoMapper(typeof(ClosetProfile));
builder.Services.AddAutoMapper(typeof(ProductProfile));
builder.Services.AddAutoMapper(typeof(StockProfile));
builder.Services.AddAutoMapper(typeof(WarehouseProfile));
builder.Services.AddAutoMapper(typeof(OutsourceProfile));
builder.Services.AddAutoMapper(typeof(OutsourceProductProfile));
builder.Services.AddAutoMapper(typeof(UserProfile));
builder.Services.AddAutoMapper(typeof(CartProfile));
builder.Services.AddAutoMapper(typeof(ContactMessageProfile));
builder.Services.AddAutoMapper(typeof(FavouriteProfile));
//FLUENT VALIDATION
builder.Services.AddScoped<IValidator<PostContactMessageDto>, PostContactMessageDtoValidator>();
builder.Services.AddScoped<IValidator<PutContactMessageDto>, PutContactMessageDtoValidator>();
builder.Services.AddScoped<IValidator<PostCartDto>, PostCartDtoFluentValidator>();
builder.Services.AddScoped<IValidator<ProductRequestDTO>, ProductRequestDTOValidator>();



//OTHER VALIDATIONS
builder.Services.AddScoped<IPostContactDtoOtherValidators, PostContactDtoOtherValidators>();
builder.Services.AddScoped<IPutContactDtoOtherValidators, PutContactDtoOtherValidators>();

builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddDbContext<Context>();
builder.Services.AddIdentity<AppUser,AppRole>().AddEntityFrameworkStores<Context>();
var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("FlicoApiCors");
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();