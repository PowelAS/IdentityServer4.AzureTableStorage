// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Powel.AzureTableStorage.IdentityServer4.Mappers;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Powel.AzureTableStorage.IdentityServer4.Stores
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

        async Task<Entities.PersistedGrant> GetEntityAsync(string key)
        {
            var table = await InitTable();
            Entities.PersistedGrant model = null;
            var query = new TableQuery<Entities.PersistedGrant>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, key));
            TableContinuationToken continuationToken = null;

            do
            {
                var result = await table.ExecuteQuerySegmentedAsync(query, continuationToken);

                if (result.Results.Count > 0)
                {
                    model = result.Results[0];
                    break;
                }

                continuationToken = result.ContinuationToken;
            } while (continuationToken != null);

            _logger.LogDebug("{persistedGrantKey} found in database: {persistedGrantKeyFound}", key, model != null);
            return model;
        }

        public async Task<PersistedGrant> GetAsync(string key)
        {
            var entity = await GetEntityAsync(key);
            return entity?.ToModel();
        }

        async Task<IEnumerable<Entities.PersistedGrant>> GetAllEntitiesAsync(string subjectId)
        {
            var filter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, subjectId);
            var persistedGrants = await GetFilteredEntitiesAsync(filter);
            var persistedGrantList = persistedGrants.ToList();

            _logger.LogDebug("{persistedGrantCount} persisted grants found for subjectId {subjectId}", persistedGrantList.Count, subjectId);
            return persistedGrantList;
        }

        async Task<IEnumerable<Entities.PersistedGrant>> GetAllEntitiesAsync(string subjectId, string clientId)
        {
            var subjectFilter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, subjectId);
            var clientFilter = TableQuery.GenerateFilterCondition("ClientId", QueryComparisons.Equal, clientId);
            var combinedFilter = TableQuery.CombineFilters(subjectFilter, TableOperators.And, clientFilter);

            var persistedGrants = await GetFilteredEntitiesAsync(combinedFilter);
            var persistedGrantList = persistedGrants.ToList();

            _logger.LogDebug("{persistedGrantCount} persisted grants found for subjectId {subjectId}, clientId {clientId}", persistedGrantList.Count, subjectId, clientId);
            return persistedGrantList;
        }

        async Task<IEnumerable<Entities.PersistedGrant>> GetAllEntitiesAsync(string subjectId, string clientId, string type)
        {
            var subjectFilter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, subjectId);
            var clientFilter = TableQuery.GenerateFilterCondition("ClientId", QueryComparisons.Equal, clientId);
            var typeFilter = TableQuery.GenerateFilterCondition("Type", QueryComparisons.Equal, type);
            var combinedFilter = TableQuery.CombineFilters(subjectFilter, TableOperators.And, TableQuery.CombineFilters(clientFilter, TableOperators.And, typeFilter));

            var persistedGrants = await GetFilteredEntitiesAsync(combinedFilter);
            var persistedGrantList = persistedGrants.ToList();

            _logger.LogDebug("{persistedGrantCount} persisted grants found for subjectId {subjectId}, clientId {type}, clientId {type}", persistedGrantList.Count, subjectId, clientId, type);
            return persistedGrantList;
        }

        async Task<IEnumerable<Entities.PersistedGrant>> GetFilteredEntitiesAsync(string filter)
        {
            var table = await InitTable();
            var query = new TableQuery<Entities.PersistedGrant>().Where(filter);
            TableContinuationToken continuationToken = null;
            var persistedGrants = new List<Entities.PersistedGrant>();

            do
            {
                var result = await table.ExecuteQuerySegmentedAsync(query, continuationToken);
                persistedGrants.AddRange(result.Results);
                continuationToken = result.ContinuationToken;
            } while (continuationToken != null);

            return persistedGrants;
        }

        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            var entities = await GetAllEntitiesAsync(subjectId);
            return entities.Select(e => e.ToModel());
        }

        public async Task RemoveAsync(string key)
        {
            var persistedGrant = await GetEntityAsync(key);
            if (persistedGrant == null)
            {
                _logger.LogDebug("no {persistedGrantKey} persisted grant found in database", key);
                return;
            }

            var table = await InitTable();
            var operation = TableOperation.Delete(persistedGrant);
            var result = await table.ExecuteAsync(operation);
            _logger.LogDebug("removed {persistedGrantKey} from database with result {result}", key, result.HttpStatusCode);
        }

        public async Task RemoveAllAsync(string subjectId, string clientId)
        {
            var persistedGrants = await GetAllEntitiesAsync(subjectId, clientId);
            var persistedGrantList = persistedGrants.ToList();
            _logger.LogDebug("removing {persistedGrantCount} persisted grants from database for subject {subjectId}, clientId {clientId}", persistedGrantList.Count, subjectId, clientId);

            await RemoveAllAsync(persistedGrantList);
        }

        public async Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            var persistedGrants = await GetAllEntitiesAsync(subjectId, clientId, type);
            var persistedGrantList = persistedGrants.ToList();
            _logger.LogDebug("removing {persistedGrantCount} persisted grants from database for subject {subjectId}, clientId {clientId}, grantType {persistedGrantType}", persistedGrantList.Count, subjectId, clientId, type);

            await RemoveAllAsync(persistedGrantList);
        }

        private async Task RemoveAllAsync(IEnumerable<Entities.PersistedGrant> persistedGrants)
        {
            var table = await InitTable();

            foreach (var persistedGrant in persistedGrants)
            {
                var operation = TableOperation.Delete(persistedGrant);
                var result = await table.ExecuteAsync(operation);
                _logger.LogDebug("removed {persistedGrantKey} from database with result {result}", persistedGrant.PartitionKey,
                    result.HttpStatusCode);
            }
        }
    }
}