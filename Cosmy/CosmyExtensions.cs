using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cosmy
{
    public static class CosmyExtensions
    {
        public static IQueryable<T> AddPaging<T>(this IQueryable<T> source, int skip, int take)
        {
            return source.Skip(skip).Take(take);
        }

        public static IQueryable<TResult> PageInternalMember<T, TResult>(this IQueryable<T> source, int skip, int take,
           Expression<Func<T, IEnumerable<TResult>>> memberSelector)
        {
            return source.SelectMany(memberSelector).Skip(skip).Take(take);
        }

        public static async Task<IEnumerable<T>> ExecuteQuery<T>(this IQueryable<T> source) where T : class
        {
            var documentQuery = GenerateDocumentQuery(source);

            List<T> results = new List<T>();

            while (documentQuery.HasMoreResults)
            {
                results.AddRange(await documentQuery.ExecuteNextAsync<T>());
            }

            return results;
        }

        public static async Task<IEnumerable<TResult>> ExecuteQuery<T, TResult>(this IQueryable<T> source) where T : class
        {
            var documentQuery = GenerateDocumentQuery(source);

            List<TResult> results = new List<TResult>();

            while (documentQuery.HasMoreResults)
            {
                results.AddRange(await documentQuery.ExecuteNextAsync<TResult>());
            }

            return results;
        }

        public static IDocumentQuery<T> GenerateDocumentQuery<T>(this IQueryable<T> source)
        {
            return source.AsDocumentQuery();
        }

        public static string ExtractQuery<T>(this IQueryable<T> source)
        {
            var docQuery = source.GenerateDocumentQuery();

            return docQuery.ToString();
        }
    }
}
