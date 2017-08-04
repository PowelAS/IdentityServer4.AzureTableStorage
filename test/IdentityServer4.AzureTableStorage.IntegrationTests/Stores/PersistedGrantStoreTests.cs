// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Linq;
using Host;
using IdentityServer4.AzureTableStorage.Mappers;
using IdentityServer4.AzureTableStorage.Options;
using IdentityServer4.AzureTableStorage.Stores;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace IdentityServer4.AzureTableStorage.IntegrationTests.Stores
{
    public class PersistedGrantStoreTests
    {
        private static readonly OperationalStoreOptions StoreOptions = new OperationalStoreOptions();

        private static PersistedGrant CreateTestObject()
        {
            return new PersistedGrant
            {
                Key = Guid.NewGuid().ToString(),
                Type = "authorization_code",
                ClientId = Guid.NewGuid().ToString(),
                SubjectId = Guid.NewGuid().ToString(),
                CreationTime = new DateTime(2016, 08, 01),
                Expiration = new DateTime(2016, 08, 31),
                Data = Guid.NewGuid().ToString()
            };
        }

        private static string GetConnectionString()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<Startup>()
                .Build();

            return configuration["IdentityServerTableStorageConnectionString"];
        }

        [Fact]
        public void GetAsync_WithSubAndTypeAndPersistedGrantExists_ExpectPersistedGrantReturned()
        {
            var store = new PersistedGrantStore(GetConnectionString(), new FakeLogger<PersistedGrantStore>());

            var persistedGrant = CreateTestObject();
            store.StoreAsync(persistedGrant).Wait();

            var foundPersistedGrants = store.GetAllAsync(persistedGrant.SubjectId).Result.ToList();
            Assert.NotNull(foundPersistedGrants);
            Assert.NotEmpty(foundPersistedGrants);
            Assert.Equal(1, foundPersistedGrants.Count);
            Assert.Equal(persistedGrant.SubjectId, foundPersistedGrants[0].SubjectId);
        }

        [Fact]
        public void RemoveAsync_WhenKeyOfExistingReceived_ExpectGrantDeleted()
        {
            var store = new PersistedGrantStore(GetConnectionString(), new FakeLogger<PersistedGrantStore>());

            var persistedGrant = CreateTestObject();
            store.StoreAsync(persistedGrant).Wait();

            store.RemoveAsync(persistedGrant.Key).Wait();

            var foundGrant = store.GetAsync(persistedGrant.Key).Result;
            Assert.Null(foundGrant);
        }

        [Fact]
        public void RemoveAsync_WhenSubIdAndClientIdOfExistingReceived_ExpectGrantDeleted()
        {
            var store = new PersistedGrantStore(GetConnectionString(), new FakeLogger<PersistedGrantStore>());

            var persistedGrant = CreateTestObject();
            store.StoreAsync(persistedGrant).Wait();

            store.RemoveAllAsync(persistedGrant.SubjectId, persistedGrant.ClientId).Wait();

            var foundGrant = store.GetAsync(persistedGrant.Key).Result;
            Assert.Null(foundGrant);
        }

        [Fact]
        public void RemoveAsync_WhenSubIdClientIdAndTypeOfExistingReceived_ExpectGrantDeleted()
        {
            var store = new PersistedGrantStore(GetConnectionString(), new FakeLogger<PersistedGrantStore>());

            var persistedGrant = CreateTestObject();
            store.StoreAsync(persistedGrant).Wait();

            store.RemoveAllAsync(persistedGrant.SubjectId, persistedGrant.ClientId, persistedGrant.Type).Wait();

            var foundGrant = store.GetAsync(persistedGrant.Key).Result;
            Assert.Null(foundGrant);
        }

        [Fact]
        public void Store_should_create_new_record_if_key_does_not_exist()
        {
            var store = new PersistedGrantStore(GetConnectionString(), new FakeLogger<PersistedGrantStore>());
            var persistedGrant = CreateTestObject();

            var missingGrant = store.GetAsync(persistedGrant.Key).Result;
            Assert.Null(missingGrant);

            store.StoreAsync(persistedGrant).Wait();

            var foundGrant = store.GetAsync(persistedGrant.Key).Result;
            Assert.NotNull(foundGrant);
        }

        [Fact]
        public void Store_should_update_record_if_key_already_exists()
        {
            var store = new PersistedGrantStore(GetConnectionString(), new FakeLogger<PersistedGrantStore>());

            var persistedGrant = CreateTestObject();
            store.StoreAsync(persistedGrant).Wait();

            var newDate = persistedGrant.Expiration.Value.AddHours(1);
            persistedGrant.Expiration = newDate;
            store.StoreAsync(persistedGrant).Wait();

            var foundGrant = store.GetAsync(persistedGrant.Key).Result;
            Assert.NotNull(foundGrant);
            Assert.Equal(newDate, persistedGrant.Expiration);
        }
    }
}