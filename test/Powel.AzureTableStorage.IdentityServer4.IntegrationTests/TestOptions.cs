using System;
using System.Collections.Generic;
using System.Text;
using Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Powel.AzureTableStorage.IdentityServer4.Options;

namespace Powel.AzureTableStorage.IdentityServer4.IntegrationTests
{
    public class TestOptions : IOptions<DatabaseOptions>
    {
        private readonly IConfigurationRoot _configuration;

        public TestOptions()
        {
            _configuration = new ConfigurationBuilder()
                .AddUserSecrets<Startup>()
                .Build();
        }

        public DatabaseOptions Value => new DatabaseOptions
        {
            ConnectionString = _configuration["IdentityServerTableStorageConnectionString"]
        };
    }
}
