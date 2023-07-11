using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskTriangles.Models;
using TaskTriangles.Services;
using TaskTriangles.Services.Interfaces;
using TaskTriangles.Validation;
using TaskTriangles.Validation.Interfaces;
using TaskTriangles.ViewModels;
using TaskTriangles.Views;

namespace TaskTriangles
{
    public static class Program
    {
        static async Task Main()
        {
            // To customize application configuration such as
            // set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            using IHost host = CreateHost();
            await host.StartAsync();

            IHostApplicationLifetime lifetime =
                host.Services.GetRequiredService<IHostApplicationLifetime>();

            using (IServiceScope scope = host.Services.CreateScope())
            {
                var mainForm = scope.ServiceProvider.GetRequiredService<MainForm>();

                Application.Run(mainForm);
            }

            lifetime.StopApplication();
            await host.StopAsync();
        }

        private static IHost CreateHost()
        {
            var builder = Host.CreateApplicationBuilder();

            builder.Services.AddSingleton<MainForm>();

            ConfigureServices(builder.Services);
            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));

            return builder.Build();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<MainViewModel>();
            services.AddTransient<IFigureService, TriangleService>();
            services.AddTransient<IInputFileValidator, InputFileValidator>();
            services.AddTransient<ICoordinatesValidator, CoordinatesValidator>();
        }
    }
}