using ADQuerySample.ADFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices; 

namespace ADQuerySample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Print All Users 
            PrintResults(GetUsers(AllUserFilter()));

            // Print Sepcific Users 
            PrintResults(GetUsers(GetUserCollectionFilter( "ABC" , "XYZ" )));

            Console.ReadLine();
        }

        /// <summary>
        /// Returns (&(objectCategory=person)(objectClass=user))
        /// </summary>
        /// <returns></returns>
        private static string AllUserFilter()
        {
            string _filterQuery = string.Empty;
            string _error = string.Empty; 
                        
            ADFilter _filter = new ADFilter(ADFilterBuilder.ADFilterExpression.AND);
            _filter.Add(new ADFilterCondition("objectCategory", ADFilterCondition.ADFilterOperators.EQUALTO, "person"));
            _filter.Add(new ADFilterCondition("objectClass", ADFilterCondition.ADFilterOperators.EQUALTO, "user"));

            return ADFilterBuilder.GetFilter(_filter, out _error);

        }
        /// <summary>
        /// Returns (&(objectCategory=person)(objectClass=user)(|(cn=USER1)(cn=USER2)))
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        private static string GetUserCollectionFilter(params string[] users)
        {
            string _filterQuery = string.Empty;
            string _error = string.Empty;

            ADFilter _childFilter = new ADFilter(ADFilterBuilder.ADFilterExpression.OR);
            users.ToList<string>().ForEach(delegate (string user)
            {
                _childFilter.Add(new ADFilterCondition("cn", ADFilterCondition.ADFilterOperators.EQUALTO, user));
            }
            );


            ADFilter _filter = new ADFilter(ADFilterBuilder.ADFilterExpression.AND,_childFilter);
            _filter.Add(new ADFilterCondition("objectCategory", ADFilterCondition.ADFilterOperators.EQUALTO, "person"));
            _filter.Add(new ADFilterCondition("objectClass", ADFilterCondition.ADFilterOperators.EQUALTO, "user"));

            return ADFilterBuilder.GetFilter(_filter,out _error);

        }

        private static SearchResultCollection GetUsers(string filter)
        {
            SearchResultCollection _searchResultCollections = null;

            using (DirectoryEntry rootEntry = new DirectoryEntry())
            {
                rootEntry.Path = $"LDAP://xyz.com:389/dc=xyz,dc=com";
                //rootEntry.Username = ;
                //rootEntry.Password = ;

                using (DirectorySearcher _searcher = new DirectorySearcher(rootEntry))
                {
                    _searcher.Filter = filter;
                    //_searcher.PageSize = 1000;

                    _searchResultCollections = _searcher.FindAll();
                }
            }

            return _searchResultCollections;
        }

        private static void PrintResults(SearchResultCollection results)
        {
            int counter = 1;
            results.Cast<SearchResult>().ToList<SearchResult>().ForEach(delegate (SearchResult result)
            {
                Console.WriteLine($" {counter++} : {result.GetDirectoryEntry().Properties["cn"]?.Value}");
            });
        }
    }
}
