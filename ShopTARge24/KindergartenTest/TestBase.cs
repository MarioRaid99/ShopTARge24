using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShopTARge24.ApplicationServices.Services;
using ShopTARge24.Core.ServiceInterface;
using ShopTARge24.Data;

namespace ShopTARge24.KindergartenTest
{
    public abstract class TestBase : IDisposable
    {
        protected readonly IServiceProvider serviceProvider;

        protected TestBase()
        {
            var services = new ServiceCollection();

            // InMemory andmebaas – iga test saab oma puhta DB
            services.AddDbContext<ShopTARge24Context>(options =>
            {
                options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            });

            // Teenused, mida testid kasutavad
            services.AddScoped<IKindergartenService, KindergartenService>();
            services.AddScoped<IFileService, FileService>();

            // Võlts IHostEnvironment FileService jaoks
            services.AddSingleton<IHostEnvironment>(sp => new HostingEnvironment
            {
                EnvironmentName = Environments.Development,
                ApplicationName = "ShopTARge24",
                ContentRootPath = Directory.GetCurrentDirectory()
            });

            serviceProvider = services.BuildServiceProvider();
        }

        public void Dispose()
        {
            if (serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        // Lihtne HostEnvironment implementatsioon
        private class HostingEnvironment : IHostEnvironment
        {
            public string EnvironmentName { get; set; } = Environments.Development;
            public string ApplicationName { get; set; } = "ShopTARge24";
            public string ContentRootPath { get; set; } = Directory.GetCurrentDirectory();
            public Microsoft.Extensions.FileProviders.IFileProvider ContentRootFileProvider { get; set; } = default!;
        }
    }
}