using System.Security.Cryptography;

using BookManager.Data;
using BookManager.Endpoints;
using BookManager.Services;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

string PostgreSQLConnectionString = Environment.GetEnvironmentVariable("POSTGRESQL_STRING") ?? throw new Exception("POSTGRESQL_STRING is missing from evironment");

// JWT environment
string Issuer = Environment.GetEnvironmentVariable("ISSUER") ?? throw new Exception("ISSUER is missing from evironment");
string Audience = Environment.GetEnvironmentVariable("AUDIENCE") ?? throw new Exception("AUDIENCE is missing from evironment");
string PublicPath = Environment.GetEnvironmentVariable("PUBLIC_PATH") ?? "./keys/public_key.pem";
string PrivatePath = Environment.GetEnvironmentVariable("PRIVATE_PATH") ?? "./keys/private_key.pem";

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddNpgsql<BookManagerContext>(PostgreSQLConnectionString);

RSA publicKey = RSAKeyloader.LoadKey(PublicPath);
builder.Services.AddJWTAuthentication(Issuer, Audience, publicKey);

RSA privateKey = RSAKeyloader.LoadKey(PrivatePath);
builder.Services.AddJWTAuthorization(privateKey);

builder.Services.AddSingleton(provider => new JWTService(Issuer, Audience, provider.GetRequiredService<RsaSecurityKey>()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseBookEndpoint();
app.UseUserEndpoint();

app.Run();
