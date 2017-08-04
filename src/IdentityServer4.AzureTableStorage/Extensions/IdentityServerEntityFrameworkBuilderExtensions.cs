// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.AzureTableStorage.Stores;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using System;
using IdentityServer4.AzureTableStorage.Options;
using IdentityServer4.AzureTableStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityServerEntityFrameworkBuilderExtensions
    {
        public static IIdentityServerBuilder AddOperationalStore(
            this IIdentityServerBuilder builder,
            string connectionString,
            Action<OperationalStoreOptions> storeOptionsAction = null,
            Action<TokenCleanupOptions> tokenCleanUpOptions = null)
        {
            builder.Services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();

            var storeOptions = new OperationalStoreOptions();
            storeOptionsAction?.Invoke(storeOptions);
            builder.Services.AddSingleton(storeOptions);

            var tokenCleanupOptions = new TokenCleanupOptions();
            tokenCleanUpOptions?.Invoke(tokenCleanupOptions);
            builder.Services.AddSingleton(tokenCleanupOptions);
            builder.Services.AddSingleton<TokenCleanup>();
            
            return builder;
        }

        public static IApplicationBuilder UseIdentityServerEfTokenCleanup(this IApplicationBuilder app, IApplicationLifetime applicationLifetime)
        {
            var tokenCleanup = app.ApplicationServices.GetService<TokenCleanup>();
            if(tokenCleanup == null)
            {
                throw new InvalidOperationException("AddOperationalStore must be called on the service collection.");
            }
            applicationLifetime.ApplicationStarted.Register(tokenCleanup.Start);
            applicationLifetime.ApplicationStopping.Register(tokenCleanup.Stop);

            return app;
        }
    }
}