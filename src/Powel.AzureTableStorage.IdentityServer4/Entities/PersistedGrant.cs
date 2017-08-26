// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Powel.AzureTableStorage.IdentityServer4.Entities
{
    public class PersistedGrant : TableEntity
    {
        // a PersistedGrant.Key will map to TableEntity.PartitionKey
        // public string Key { get; set; }
        // a PersistedGrant.SubjectId will map to TableEntity.RowKey
        // public string SubjectId { get; set; }
        public string Type { get; set; }
        public string ClientId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? Expiration { get; set; }
        public string Data { get; set; }

        public override string ToString()
        {
            return $"{{PartitionKey (Key): '{PartitionKey}', RowKey (SubjectId): '{RowKey}', Type: '{Type}', ClientId: '{ClientId}', CreationTime: '{CreationTime}', Expiration: '{Expiration}', Data: '{Data}'}}";
        }
    }
}