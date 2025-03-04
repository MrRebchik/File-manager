using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.OpenApi.Models;
using FileManagerAPI.Models;

namespace FileManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateSlimBuilder(args);
            builder.Services.AddDbContext<StorageContext>(options => { 
                options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=storagedb;Trusted_Connection=True;"); 
                options.EnableSensitiveDataLogging(true); });
            builder.Services.AddDbContext<PeopleContext>(options => {
                options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=peopledb;Trusted_Connection=True;");
                options.EnableSensitiveDataLogging(true);
            });
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.AUDIENCE,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                    };
                });
            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApp", Version = "v1" });
            });
            var app = builder.Build();

            using (var serviceScope = app.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                SeedData.SeedPeopleDatabase(services.GetRequiredService<PeopleContext>());
                SeedData.SeedStorageDatabase(services.GetRequiredService<StorageContext>());
            }
            //SeedData.SeedPeopleDatabase(app.Services.GetService<PeopleContext>());
            //SeedData.SeedStorageDatabase(app.Services.GetService<StorageContext>());

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStatusCodePages();
            //app.MapDefaultControllerRoute();
            app.MapControllers();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApp");
            });

            app.Run();
        }
        
    }
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer";
        public const string AUDIENCE = "MyAuthClient";
        const string KEY = "rP2YHcb8sWHtvhi6DzqR1oHoddZ5IaQvN_oOmK4";
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
