using Microsoft.Extensions.Configuration;
using System;

namespace VagasConsoleApp.Config
{
    public static class AppConfig
    {
        private static IConfiguration _configuration;

        public static void Initialize()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();
        }

        public static string GetConnectionString(string name)
        {
            if (_configuration == null)
                throw new InvalidOperationException("AppConfig não inicializado. Chame AppConfig.Initialize() primeiro.");

            return _configuration.GetConnectionString(name);
        }
    }
}