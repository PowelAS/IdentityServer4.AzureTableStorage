// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using IdentityServer4.AzureTableStorage.Mappers;
using IdentityServer4.Models;
using Xunit;

namespace IdentityServer4.AzureTableStorage.UnitTests.Mappers
{
    public class PersistedGrantMappersTests
    {
        [Fact]
        public void PersistedGrantAutomapperConfigurationIsValid()
        {
            var model = new PersistedGrant
            {
                Key = Guid.NewGuid().ToString(),
                Type = "authorization_code",
                ClientId = Guid.NewGuid().ToString(),
                SubjectId = Guid.NewGuid().ToString(),
                CreationTime = new DateTime(2016, 08, 01),
                Expiration = new DateTime(2016, 08, 31),
                Data = Guid.NewGuid().ToString()
            };

            var mappedEntity = model.ToEntity();
            var mappedModel = mappedEntity.ToModel();
            
            Assert.NotNull(mappedModel);
            Assert.NotNull(mappedEntity);

            Assert.Equal(model.Key, mappedModel.Key);
            Assert.Equal(model.Key, mappedEntity.PartitionKey);
            Assert.Equal(model.Type, mappedModel.Type);
            Assert.Equal(model.Type, mappedEntity.Type);
            Assert.Equal(model.ClientId, mappedModel.ClientId);
            Assert.Equal(model.ClientId, mappedEntity.ClientId);
            Assert.Equal(model.SubjectId, mappedModel.SubjectId);
            Assert.Equal(model.SubjectId, mappedEntity.RowKey);
            Assert.Equal(model.CreationTime, mappedModel.CreationTime);
            Assert.Equal(model.CreationTime, mappedEntity.CreationTime);
            Assert.Equal(model.Expiration, mappedModel.Expiration);
            Assert.Equal(model.Expiration, mappedEntity.Expiration);
            Assert.Equal(model.Data, mappedModel.Data);
            Assert.Equal(model.Data, mappedEntity.Data);

            PersistedGrantMappers.Mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}