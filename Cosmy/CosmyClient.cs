using Cosmy.Config;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmy
{
    public class CosmyClient : ICosmyClient
    {
        private readonly DocumentClient documentClient;
        private readonly CosmyConfiguration configuration;

        public CosmyClient(DocumentClient documentClient, CosmyConfiguration configuration)
        {
            this.documentClient = documentClient;
            this.configuration = configuration;
        }

        public IQueryable<T> CreateDocumentQuery<T>(string collection, object partitionKey = null)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(configuration.Database, collection);

            var partitionOptions = (partitionKey != null) ? new FeedOptions { PartitionKey = new PartitionKey(partitionKey) } : null;

            var query = this.documentClient.CreateDocumentQuery<T>(uri, partitionOptions);

            return query;
        }

        public async Task<IEnumerable<T>> ExecuteQuery<T>(IQueryable<T> source) where T : class
        {
            return await source.ExecuteQuery();
        }

        public async Task<IEnumerable<TResult>> ExecuteQuery<T, TResult>(IQueryable<T> source) where T : class
        {
            return await source.ExecuteQuery<T,TResult>();
        }

        public async Task UpdateDocumentAsync<T>(Guid documentId, string collection, T data, object partitionKey = null) where T : class
        {
            await this.UpdateDocumentAsync(documentId.ToString(), collection, data, partitionKey);
        }

        public async Task UpdateDocumentAsync<T>(string documentId, string collection, T data, object partitionKey = null) where T : class
        {
            var uri = UriFactory.CreateDocumentUri(configuration.Database, collection, documentId);

            var partitionOptions = (partitionKey != null) ? new RequestOptions { PartitionKey = new PartitionKey(partitionKey) } : null;

            await this.documentClient.ReplaceDocumentAsync(uri, data, partitionOptions);
        }

        public async Task DeleteDocumentAsync(Guid documentId, string collection, object partitionKey = null)
        {
            await this.DeleteDocumentAsync(documentId.ToString(), collection, partitionKey);
        }

        public async Task DeleteDocumentAsync(string documentId, string collection, object partitionKey = null)
        {
            var partitionOptions = (partitionKey != null) ? new RequestOptions { PartitionKey = new PartitionKey(partitionKey) } : null;

            var uri = UriFactory.CreateDocumentUri(configuration.Database, collection, documentId);

            await this.documentClient.DeleteDocumentAsync(uri, partitionOptions);
        }
    }
}
