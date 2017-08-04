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
        public void StoreAsync_WhenPersistedGrantStored_ExpectSuccess()
        {
            //var persistedGrant = CreateTestObject();

            //using (var context = new PersistedGrantDbContext(options, StoreOptions))
            //{
            //    var store = new PersistedGrantStore(context, FakeLogger<PersistedGrantStore>.Create());
            //    store.StoreAsync(persistedGrant).Wait();
            //}

            //using (var context = new PersistedGrantDbContext(options, StoreOptions))
            //{
            //    var foundGrant = context.PersistedGrants.FirstOrDefault(x => x.Key == persistedGrant.Key);
            //    Assert.NotNull(foundGrant);
            //}

            throw new NotImplementedException();
        }

        [Fact]
        public void GetAsync_WithKeyAndPersistedGrantExists_ExpectPersistedGrantReturned()
        {
            //var persistedGrant = CreateTestObject();

            //using (var context = new PersistedGrantDbContext(options, StoreOptions))
            //{
            //    context.PersistedGrants.Add(persistedGrant.ToEntity());
            //    context.SaveChanges();
            //}

            //PersistedGrant foundPersistedGrant;
            //using (var context = new PersistedGrantDbContext(options, StoreOptions))
            //{
            //    var store = new PersistedGrantStore(context, FakeLogger<PersistedGrantStore>.Create());
            //    foundPersistedGrant = store.GetAsync(persistedGrant.Key).Result;
            //}

            //Assert.NotNull(foundPersistedGrant);

            throw new NotImplementedException();
        }

        [Fact]
        public void GetAsync_WithSubAndTypeAndPersistedGrantExists_ExpectPersistedGrantReturned()
        {
            //var persistedGrant = CreateTestObject();

            //using (var context = new PersistedGrantDbContext(options, StoreOptions))
            //{
            //    context.PersistedGrants.Add(persistedGrant.ToEntity());
            //    context.SaveChanges();
            //}

            //IList<PersistedGrant> foundPersistedGrants;
            //using (var context = new PersistedGrantDbContext(options, StoreOptions))
            //{
            //    var store = new PersistedGrantStore(context, FakeLogger<PersistedGrantStore>.Create());
            //    foundPersistedGrants = store.GetAllAsync(persistedGrant.SubjectId).Result.ToList();
            //}

            //Assert.NotNull(foundPersistedGrants);
            //Assert.NotEmpty(foundPersistedGrants);

            throw new NotImplementedException();
        }

        [Fact]
        public void RemoveAsync_WhenKeyOfExistingReceived_ExpectGrantDeleted()
        {
            //var persistedGrant = CreateTestObject();

            //using (var context = new PersistedGrantDbContext(options, StoreOptions))
            //{
            //    context.PersistedGrants.Add(persistedGrant.ToEntity());
            //    context.SaveChanges();
            //}

            //using (var context = new PersistedGrantDbContext(options, StoreOptions))
            //{
            //    var store = new PersistedGrantStore(context, FakeLogger<PersistedGrantStore>.Create());
            //    store.RemoveAsync(persistedGrant.Key).Wait();
            //}

            //using (var context = new PersistedGrantDbContext(options, StoreOptions))
            //{
            //    var foundGrant = context.PersistedGrants.FirstOrDefault(x => x.Key == persistedGrant.Key);
            //    Assert.Null(foundGrant);
            //}

            throw new NotImplementedException();
        }

        [Fact]
        public void RemoveAsync_WhenSubIdAndClientIdOfExistingReceived_ExpectGrantDeleted()
        {
            //var persistedGrant = CreateTestObject();

            //using (var context = new PersistedGrantDbContext(options, StoreOptions))
            //{
            //    context.PersistedGrants.Add(persistedGrant.ToEntity());
            //    context.SaveChanges();
            //}

            //using (var context = new PersistedGrantDbContext(options, StoreOptions))
            //{
            //    var store = new PersistedGrantStore(context, FakeLogger<PersistedGrantStore>.Create());
            //    store.RemoveAllAsync(persistedGrant.SubjectId, persistedGrant.ClientId).Wait();
            //}

            //using (var context = new PersistedGrantDbContext(options, StoreOptions))
            //{
            //    var foundGrant = context.PersistedGrants.FirstOrDefault(x => x.Key == persistedGrant.Key);
            //    Assert.Null(foundGrant);
            //}

            throw new NotImplementedException();
        }

        [Fact]
        public void RemoveAsync_WhenSubIdClientIdAndTypeOfExistingReceived_ExpectGrantDeleted()
        {
            //var persistedGrant = CreateTestObject();

            //using (var context = new PersistedGrantDbContext(options, StoreOptions))
            //{
            //    context.PersistedGrants.Add(persistedGrant.ToEntity());
            //    context.SaveChanges();
            //}

            //using (var context = new PersistedGrantDbContext(options, StoreOptions))
            //{
            //    var store = new PersistedGrantStore(context, FakeLogger<PersistedGrantStore>.Create());
            //    store.RemoveAllAsync(persistedGrant.SubjectId, persistedGrant.ClientId, persistedGrant.Type).Wait();
            //}

            //using (var context = new PersistedGrantDbContext(options, StoreOptions))
            //{
            //    var foundGrant = context.PersistedGrants.FirstOrDefault(x => x.Key == persistedGrant.Key);
            //    Assert.Null(foundGrant);
            //}

            throw new NotImplementedException();
        }

        [Fact]
        public void Store_should_create_new_record_if_key_does_not_exist()
        {
            var persistedGrant = CreateTestObject();
            var store = new PersistedGrantStore(GetConnectionString(), new FakeLogger<PersistedGrantStore>());

            var missingGrant = store.GetAsync(persistedGrant.Key).Result;
            Assert.Null(missingGrant);

            store.StoreAsync(persistedGrant).Wait();

            var foundGrant = store.GetAsync(persistedGrant.Key).Result;
            Assert.NotNull(foundGrant);
        }

        [Fact]
        public void Store_should_update_record_if_key_already_exists()
        {
            //var persistedGrant = CreateTestObject();

            //using (var context = new PersistedGrantDbContext(options, StoreOptions))
            //{
            //    context.PersistedGrants.Add(persistedGrant.ToEntity());
            //    context.SaveChanges();
            //}

            //var newDate = persistedGrant.Expiration.Value.AddHours(1);
            //using (var context = new PersistedGrantDbContext(options, StoreOptions))
            //{
            //    var store = new PersistedGrantStore(context, FakeLogger<PersistedGrantStore>.Create());
            //    persistedGrant.Expiration = newDate;
            //    store.StoreAsync(persistedGrant).Wait();
            //}

            //using (var context = new PersistedGrantDbContext(options, StoreOptions))
            //{
            //    var foundGrant = context.PersistedGrants.FirstOrDefault(x => x.Key == persistedGrant.Key);
            //    Assert.NotNull(foundGrant);
            //    Assert.Equal(newDate, persistedGrant.Expiration);
            //}

            throw new NotImplementedException();
        }
    }
}