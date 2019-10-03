using Cosmy.Config;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cosmy
{
    public class CosmyClient
    {
        private readonly DocumentClient documentClient;
        private readonly CosmyConfiguration configuration;

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
