using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADQuerySample.ADFilters
{
    public class ADFilterCondition
    {
        #region Enums
        public enum ADFilterOperators { LESSTHAN, GREATERTHAN, LESSTHANEQUALTO, GREATERTHANEQUALTO, EQUALTO, NOTEQUALTO , STARTSEARCHWITH, ENDSEARCHWITH };

        #endregion

        #region Properties 
        public string ADAttributeValue { get; set; }
        public string ADAttributeName { get; set; }

        public ADFilterOperators ADFilterOperator { set; get; }
        #endregion

        #region Constructor 
        public ADFilterCondition(string attributeName, ADFilterOperators adFilterOperator, string adAttributeValue)
        {
            ADAttributeName = attributeName;
            ADFilterOperator = adFilterOperator;
            ADAttributeValue = adAttributeValue;
        }
        #endregion

        #region Public Method 
        public string GetFilter()
        {

            if (this.ADFilterOperator == ADFilterOperators.STARTSEARCHWITH)
                return $"({this.ADAttributeName}={this.ADAttributeValue}*)";
            if (this.ADFilterOperator == ADFilterOperators.NOTEQUALTO)
                return $"(!{this.ADAttributeName}={this.ADAttributeValue}*)";

            return $"({this.ADAttributeName}{this.GetActualFilterOperator()}{this.ADAttributeValue})";
        }
        #endregion

        #region PrivateMethod
        private string GetActualFilterOperator()
        {
            string _filterOperator = string.Empty;

            switch (this.ADFilterOperator)
            {
                case ADFilterOperators.EQUALTO:
                case ADFilterOperators.STARTSEARCHWITH:
                    _filterOperator = "=";
                    break;
                case ADFilterOperators.GREATERTHAN:
                    _filterOperator = ">";
                    break;
                case ADFilterOperators.GREATERTHANEQUALTO:
                    _filterOperator = ">=";
                    break;
                case ADFilterOperators.LESSTHAN:
                    _filterOperator = "<";
                    break;
                case ADFilterOperators.LESSTHANEQUALTO:
                    _filterOperator = "<=";
                    break;
                case ADFilterOperators.ENDSEARCHWITH:
                    _filterOperator = "=*";
                    break;
                default:
                    _filterOperator = "=";
                    break;
            }

            return _filterOperator;
        }
        #endregion 
    }
}
