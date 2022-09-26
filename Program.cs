using Librarymanagement.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using AutoWrapper;
using System.Text;







 var builder = WebApplication.CreateBuilder(args);


builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

builder.Services.AddRazorPages();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
         ValidateLifetime = true,
        ClockSkew=TimeSpan.Zero
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

//services cors
 builder.Services.AddCors(p => p.AddPolicy("corspolicy", builder =>
    {
        builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
    }));
builder.Services.AddHealthChecks();
builder.Services.AddMvc();


var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
        //app cors
        app.UseCors("corspolicy");

         app.UseApiResponseAndExceptionWrapper
       (
        new AutoWrapperOptions 
        {
        UseCustomSchema = true,
        IgnoreNullValue=false
        }
       );

       
        app.UseHttpsRedirection();

        app.UseStaticFiles();

         app.UseAuthentication(); //first 

        app.UseAuthorization();//Second

        app.MapControllers();
        
        app.MapRazorPages();

        app.Run();
       
       
      

        

















//===================================================================================================================

// using Librarymanagement.Models;
// using Microsoft.AspNetCore;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.IdentityModel.Tokens;
// using AutoWrapper;
// using System.Text;





// // Add services to the container.

// //JWT Token


// var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
// {
//     options.RequireHttpsMetadata = false;
//     options.SaveToken = true;
//     options.TokenValidationParameters = new TokenValidationParameters()
//     {
//         ValidateIssuer = true,
//         ValidateAudience = true,
//         ValidAudience = builder.Configuration["Jwt:Audience"],
//         ValidIssuer = builder.Configuration["Jwt:Issuer"],
//         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
//          ValidateLifetime = true,
//         ClockSkew=TimeSpan.Zero
//     };
// });

// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// builder.Services.AddEndpointsApiExplorer();

// builder.Services.AddSwaggerGen();

// //services cors
//  builder.Services.AddCors(p => p.AddPolicy("corspolicy", builder =>
//     {
//         builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
//     }));
// builder.Services.AddHealthChecks();
// builder.Services.AddMvc();


// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
//         //app cors
//         app.UseCors("corspolicy");

//          app.UseApiResponseAndExceptionWrapper
//        (
//         new AutoWrapperOptions 
//         {
//         UseCustomSchema = true,
//         IgnoreNullValue=false
//         }
//        );

       
//         app.UseHttpsRedirection();

//          app.UseAuthentication(); //first 

//         app.UseAuthorization();//Second

//         app.MapControllers();

//         app.Run();
       
       
      

        

















