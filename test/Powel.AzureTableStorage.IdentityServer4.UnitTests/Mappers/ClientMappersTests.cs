// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using Powel.AzureTableStorage.IdentityServer4.Entities;
using Powel.AzureTableStorage.IdentityServer4.Mappers;
using Xunit;
using Client = IdentityServer4.Models.Client;

namespace Powel.AzureTableStorage.IdentityServer4.UnitTests.Mappers
{
    public class ClientMappersTests
    {
        [Fact]
        public void ClientAutomapperConfigurationIsValid()
        {
            var model = new Client();
            var mappedEntity = model.ToEntity();
            var mappedModel = mappedEntity.ToModel();
            
            Assert.NotNull(mappedModel);
            Assert.NotNull(mappedEntity);
            ClientMappers.Mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}