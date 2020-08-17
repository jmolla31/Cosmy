using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cosmy
{
    public interface ICosmyClient
    {
        IQueryable<T> CreateDocumentQuery<T>(string collection, object partitionKey = null);
        Task DeleteDocumentAsync(Guid documentId, string collection, object partitionKey = null);
        Task DeleteDocumentAsync(string documentId, string collection, object partitionKey = null);
        Task<IEnumerable<TResult>> ExecuteQuery<T, TResult>(IQueryable<T> source) where T : class;
        Task<IEnumerable<T>> ExecuteQuery<T>(IQueryable<T> source) where T : class;
        Task<T> GetDocumentAsync<T>(Guid documentId, string collection, object partitionKey = null) where T : class;
        Task<T> GetDocumentAsync<T>(string documentId, string collection, object partitionKey = null) where T : class;
        Task<object> GetDocumentAsync(string documentId, string collection, Type @type, object partitionKey = null);
        Task<Guid> CreateDocumentAsync<T>(string collection, T @object, object partitionKey = null) where T : class;
        Task UpdateDocumentAsync<T>(Guid documentId, string collection, T data, object partitionKey = null) where T : class;
        Task UpdateDocumentAsync<T>(string documentId, string collection, T data, object partitionKey = null) where T : class;
    }
}