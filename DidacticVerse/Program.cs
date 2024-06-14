using Amazon.SecretsManager;
using DidacticVerse;
using DidacticVerse.Accounts;
using DidacticVerse.AWS;
using DidacticVerse.Helpers;
using DidacticVerse.Services;
using EfVueMantle;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Amazon.SecretsManager.Extensions.Caching;
using Amazon.Runtime.CredentialManagement;
using Amazon.Runtime;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);


//TODO break this out

var chain = new CredentialProfileStoreChain();
AWSCredentials basicProfile;
if (!chain.TryGetAWSCredentials(builder.Configuration["AWS:Profile"], out basicProfile))
{
    throw new Exception($"Could not get AWS profile credentials for {builder.Configuration["AWS:Profile"]}");
}
var secretsManagerClient = new AmazonSecretsManagerClient(basicProfile, builder.Configuration.GetAWSOptions().Region);
var cache = new SecretsManagerCache(secretsManagerClient);
var googleResult = cache.GetSecretString(builder.Configuration["AWS:GoogleIdentity"]).Result;
var googleSettings = JsonConvert.DeserializeObject<GoogleConfig>(googleResult);
if (googleSettings == null)
{
    throw new Exception($"The settings were null");
}














builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddGoogle(googleOptions =>
{
    googleOptions.ClientId = googleSettings.ClientId;
    googleOptions.ClientSecret = googleSettings.ClientSecret;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization();


builder.Services.AddCors();

builder.Services.AddControllers();
builder.Services.AddScoped<AccountService, AccountService>();
builder.Services.AddScoped<BetaFeedbackService, BetaFeedbackService>();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<CommentService, CommentService>();

builder.Services.AddScoped<DiscussionReportService, DiscussionReportService>();
builder.Services.AddScoped<DiscussionService, DiscussionService>();

builder.Services.AddScoped<ImageService, ImageService>();

builder.Services.AddScoped<TopicService, TopicService>();

builder.Services.AddTransient<AccountProvider, AccountProvider>();
builder.Services.AddTransient<Secrets, Secrets>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//DB
builder.Services.AddMvc().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddDbContext<DidacticVerseContext>(
    options =>
    {
        options.UseSqlServer(
                builder.Configuration.GetConnectionString(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development ? "DidacticVerseDev" : "DidacticVerseProd"),
                providerOptions =>
                {
                    providerOptions.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds);
                    providerOptions.EnableRetryOnFailure();
                    providerOptions.UseNetTopologySuite();
                }
            );
    });

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //using (var serviceScope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope())
    //{
    //    if (serviceScope == null)
    //    {
    //        throw new InvalidOperationException("Service scope undefined in Program.cs");
    //    }
    //    var context = serviceScope.ServiceProvider.GetRequiredService<DidacticVerseContext>();
    //    //context.Database.Migrate();
    //}
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(x =>
        x.AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins("http://localhost:8080")
            .AllowCredentials()
            .WithExposedHeaders()
    );
    ModelExport.ExportDataObjects("../../didactic-verse/src/data");
}
else
{
    app.UseCors(x =>
        x.AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins("https://connect-her.org")
            .AllowCredentials()
            .WithExposedHeaders()
    );
}

//app.UseExceptionHandler("/Error");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
