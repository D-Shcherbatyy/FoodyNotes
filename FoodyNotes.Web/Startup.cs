using FoodyNotes.DataAccess.MsSql;
using FoodyNotes.Infrastructure.Implementation;
using FoodyNotes.Infrastructure.Implementation.Authentication;
using FoodyNotes.Infrastructure.Interfaces;
using FoodyNotes.Infrastructure.Interfaces.Authentication;
using FoodyNotes.Infrastructure.Interfaces.Authentication.Dtos;
using FoodyNotes.UseCases;
using FoodyNotes.UseCases.Authentication.Commands;
using FoodyNotes.Web.Middlewares;
using FoodyNotes.Web.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace FoodyNotes.Web
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<IDbContext, ApplicationDbContext>();
      services.AddCors();

      services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
      
      services.AddScoped<IUserService, UserService>();
      services.AddScoped<IJwtTokenService, JwtTokenService>();
      services.AddScoped<IRefreshTokenService, RefreshTokenService>();
      services.AddScoped<IGoogleService, GoogleService>();
      services.AddScoped<HttpService>();

      services.AddMediatR(typeof(AuthenticateCommand));

      services.AddTransient(typeof(IPipelineBehavior<AuthenticateCommand,AuthenticateResponseDto>), typeof(TestPipelineBehavior));
      
      services.AddControllers();

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "FoodyNotes.Web", Version = "v1" });
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FoodyNotes.Web v1"));
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseCors(x => x
        .SetIsOriginAllowed(origin => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());

      //app.UseAuthorization(); - our authorization is based on IAuthorizationFilter
      app.UseMiddleware<ErrorHandlerMiddleware>();
      app.UseMiddleware<JwtMiddleware>();

      app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
  }
}