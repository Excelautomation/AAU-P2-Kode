namespace ARK.Model.Search
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ISearchable<out T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        bool SearchMatching(params Func<T, bool>[] filters);
        bool SearchMatching(string searchExpression);
    }
}
