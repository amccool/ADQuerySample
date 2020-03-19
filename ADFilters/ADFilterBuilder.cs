using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADQuerySample.ADFilters
{
    public class ADFilterBuilder
    {
        #region Enum
        public enum ADFilterExpression { AND, OR };
        #endregion 

        #region Variables 
        #endregion

        #region Properties 

        #endregion

        #region Public Method 

        public string GetFilterCondition(IList<ADFilterCondition> filterConditions, ADFilterExpression adFilterExpression)
        {
            StringBuilder _filterCondition = new StringBuilder();
            string _error = string.Empty;

            filterConditions.ToList().ForEach(delegate (ADFilterCondition filterCondition)
            {
                if (!VerifyADFilterCondition(filterCondition, out _error))
                {
                    throw new Exception($"Error Occurred while Adding Filter Condition. {_error}");
                }

                _filterCondition.Append(filterCondition.GetFilter());
            });

            return Convert.ToString($"({GetFilterExpression(adFilterExpression)}{_filterCondition})");
        }

        public static string GetFilter(ADFilter filter, out string error)
        {
            StringBuilder _filters = new StringBuilder();
            string _childFilter = string.Empty;
            string _error = string.Empty;

            if (filter.HasChildrun())
                _childFilter = GetFilter(filter.ChildFilter, out _error);


            filter.ADFilterConditions.ToList().ForEach(delegate (ADFilterCondition filterCondition)
            {
                if (!VerifyADFilterCondition(filterCondition, out _error))
                {
                    throw new Exception($"Error Occurred while Adding Filter Condition. {_error}");
                }

                _filters.Append(filterCondition.GetFilter());
            });

            error = _error;

            if (!string.IsNullOrEmpty(_childFilter)) _filters.Append(_childFilter);

            return Convert.ToString($"({GetFilterExpression(filter.FilterExpression)}{_filters})");

        }
        #endregion


        public static bool VerifyADFilterCondition(ADFilterCondition adFilterCondition, out string error)
        {
            bool _success = false;
            error = string.Empty;

            if (adFilterCondition == null)
                error = "ADFilterCondition can not be null";
            else if (string.IsNullOrEmpty(adFilterCondition.ADAttributeName))
                error = "ADAttributeName can not be empty";
            else if (string.IsNullOrEmpty(adFilterCondition.ADAttributeValue))
                error = "ADAttributeValue can not be empty";
            else
                _success = true;

            return _success;
        }

        public static char GetFilterExpression(ADFilterExpression adFilterExpression)
        {
            return ((adFilterExpression == ADFilterExpression.AND) ? '&' : '|');
        }

        public static bool VerifyADFilterConditions(IList<ADFilterCondition> adFilterConditions, out string error)
        {
            bool _success = false;
            error = string.Empty;

            if (adFilterConditions == null && adFilterConditions.Count <= 0)
                error = "No Filter Conditions Found";
            else
                _success = true;

            return _success;
        }

    }
}
