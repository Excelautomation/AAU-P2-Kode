using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARK.Model.Search
{
    public interface ISearchable<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        bool SearchMatching(params Func<T, bool>[] filters);
        bool SearchMatching(string searchExpression);
    }
}
