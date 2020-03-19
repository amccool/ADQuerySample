using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADQuerySample.ADFilters
{
    public class ADFilter
    {
        #region Variables 
        private IList<ADFilterCondition> __adFilterCondition = null;
        private ADFilter __childFilter = null;
        private ADFilterBuilder.ADFilterExpression __filterExpression;
        #endregion

        #region Properties 
        public IList<ADFilterCondition> ADFilterConditions
        {
            get
            {
                return __adFilterCondition;
            }
        }

        public ADFilter ChildFilter
        {
            get
            {
                return __childFilter;
            }
            set
            {
                __childFilter = value;
            }
        }
        public ADFilterBuilder.ADFilterExpression FilterExpression
        {
            get
            {
                return __filterExpression;
            }
            set
            {
                __filterExpression = value;
            }
        }
        #endregion 

        #region Contructor 
        public ADFilter(ADFilterBuilder.ADFilterExpression filterExpression, ADFilter childFilter)
        {
            __childFilter = childFilter;
            __filterExpression = filterExpression;
        }

        public ADFilter(ADFilterBuilder.ADFilterExpression filterExpression)
        {
            __filterExpression = filterExpression;
        }
        #endregion 

        public void Add(ADFilterCondition adFilterCondition)
        {
            string _error = string.Empty;
            if (__adFilterCondition == null)
                __adFilterCondition = new List<ADFilterCondition>();

            if (!ADFilterBuilder.VerifyADFilterCondition(adFilterCondition, out _error))
                throw new Exception($"Error Occurred while Adding Filter Condition. {_error}");

            __adFilterCondition.Add(adFilterCondition);
        }

        public void AddMultipleConditions(IList<ADFilterCondition> adFilterConditions)
        {
            adFilterConditions.ToList().ForEach(delegate (ADFilterCondition adFilterCondition)
            {
                this.Add(adFilterCondition);
            });
        }

        public bool HasChildrun()
        {
            return (__childFilter != null && __childFilter.ADFilterConditions != null && __childFilter.ADFilterConditions.Count > 0);
        }

    }
}
