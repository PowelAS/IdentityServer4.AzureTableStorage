// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AzureTableStorage.Mappers;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace IdentityServer4.AzureTableStorage.Stores
{
    public class PersistedGrantStore : IPersistedGrantStore
    {
        private readonly string _connectionString;
        private readonly ILogger _logger;
        private CloudTable _cloudTable;

        public PersistedGrantStore(string connectionString, ILogger<PersistedGrantStore> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        async Task<CloudTable> InitTable()
        {
            if (_cloudTable != null) return _cloudTable;

            var storageAccount = CloudStorageAccount.Parse(_connectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            _cloudTable = tableClient.GetTableReference("IdentityServer4PersistedGrantStore");
            await _cloudTable.CreateIfNotExistsAsync();

            return _cloudTable;
        }

        public async Task StoreAsync(PersistedGrant token)
        {
            var persistedGrant = token.ToEntity();
            var table = await InitTable();
            var operation = TableOperation.InsertOrReplace(persistedGrant);
            var result = await table.ExecuteAsync(operation);
            _logger.LogDebug("stored {persistedGrantKey} with result {result}", token.Key, result.HttpStatusCode);
        }

        public async Task<PersistedGrant> GetAsync(string key)
        {
            var table = await InitTable();
            PersistedGrant model = null;
            var query = new TableQuery<Entities.PersistedGrant>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, key));
            TableContinuationToken continuationToken = null;

            do
            {
                var result = await table.ExecuteQuerySegmentedAsync(query, continuationToken);

                if (result.Results.Count > 0)
                {
                    model = result.Results[0].ToModel();
                    break;
                }

                continuationToken = result.ContinuationToken;
            } while (continuationToken != null);

            _logger.LogDebug("{persistedGrantKey} found in database: {persistedGrantKeyFound}", key, model != null);
            return model;
        }

        public Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            //var persistedGrants = _context.PersistedGrants.Where(x => x.SubjectId == subjectId).ToList();
            //var model = persistedGrants.Select(x => x.ToModel());

            //_logger.LogDebug("{persistedGrantCount} persisted grants found for {subjectId}", persistedGrants.Count, subjectId);

            //return Task.FromResult(model);

            throw new NotImplementedException();

        }

        public Task RemoveAsync(string key)
        {
            //var persistedGrant = _context.PersistedGrants.FirstOrDefault(x => x.Key == key);
            //if (persistedGrant!= null)
            //{
            //    _logger.LogDebug("removing {persistedGrantKey} persisted grant from database", key);

            //    _context.PersistedGrants.Remove(persistedGrant);

            //    try
            //    {
            //        _context.SaveChanges();
            //    }
            //    catch(DbUpdateConcurrencyException ex)
            //    {
            //        _logger.LogInformation("exception removing {persistedGrantKey} persisted grant from database: {error}", key, ex.Message);
            //    }
            //}
            //else
            //{
            //    _logger.LogDebug("no {persistedGrantKey} persisted grant found in database", key);
            //}

            //return Task.FromResult(0);

            throw new NotImplementedException();
        }

        public Task RemoveAllAsync(string subjectId, string clientId)
        {
            //var persistedGrants = _context.PersistedGrants.Where(x => x.SubjectId == subjectId && x.ClientId == clientId).ToList();

            //_logger.LogDebug("removing {persistedGrantCount} persisted grants from database for subject {subjectId}, clientId {clientId}", persistedGrants.Count, subjectId, clientId);

            //_context.PersistedGrants.RemoveRange(persistedGrants);

            //try
            //{
            //    _context.SaveChanges();
            //}
            //catch (DbUpdateConcurrencyException ex)
            //{
            //    _logger.LogInformation("removing {persistedGrantCount} persisted grants from database for subject {subjectId}, clientId {clientId}: {error}", persistedGrants.Count, subjectId, clientId, ex.Message);
            //}

            //return Task.FromResult(0);

            throw new NotImplementedException();
        }

        public Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            //var persistedGrants = _context.PersistedGrants.Where(x =>
            //    x.SubjectId == subjectId &&
            //    x.ClientId == clientId &&
            //    x.Type == type).ToList();

            //_logger.LogDebug("removing {persistedGrantCount} persisted grants from database for subject {subjectId}, clientId {clientId}, grantType {persistedGrantType}", persistedGrants.Count, subjectId, clientId, type);

            //_context.PersistedGrants.RemoveRange(persistedGrants);

            //try
            //{
            //    _context.SaveChanges();
            //}
            //catch (DbUpdateConcurrencyException ex)
            //{
            //    _logger.LogInformation("exception removing {persistedGrantCount} persisted grants from database for subject {subjectId}, clientId {clientId}, grantType {persistedGrantType}: {error}", persistedGrants.Count, subjectId, clientId, type, ex.Message);
            //}

            //return Task.FromResult(0);

            throw new NotImplementedException();
        }
    }
}