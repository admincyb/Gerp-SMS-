using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Data.Objects.DataClasses;

namespace ERPManager
{
    public class ManagerUtilities
    {
        /// <summary>
        /// Generates Code String Patterns
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetAutoGenerationCodeFormat(CodeType type)
        {
            string code;
            code = "{0}";
            switch (type)
            {
                case CodeType.CONTRACTBASISCONST:
                    code = "C-{0}";
                    break;
                case CodeType.CONTRACTBASISVAR:
                    code = "V-{0}";
                    break;
            }
            return code;
        }
    }
    #region Code Type Enum
    /// <summary>
    /// Code Type enum for the page
    /// </summary>
    public enum CodeType
    {
        CONTRACTBASISCONST,
        CONTRACTBASISVAR,
    }
    #endregion

    public static class ManagerUtil
    {
        /// <summary>
        /// Sort records based on SortBy and SortDirection
        /// Filter based PageSize and CurrentPage
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cntry"></param>
        /// <param name="utilityObj"></param>
        /// <returns>Filtered Queryable List</returns>
        public static IQueryable<T> SortRecords<T>(this IQueryable<T> cntry, ServiceUtility utilityObj)
        {

            int firstRec = (utilityObj.CurrentPage - 1) * utilityObj.PageSize;
            if (utilityObj.SortBy != null)
            {
                var sorter = utilityObj.SortDirection == "asc" || string.IsNullOrEmpty(utilityObj.SortDirection) ?
                    (string.IsNullOrEmpty(utilityObj.ThenBy) ? EntitySorter<T>.OrderBy(utilityObj.SortBy) : EntitySorter<T>.OrderBy(utilityObj.SortBy).ThenBy(utilityObj.ThenBy))
                    : (string.IsNullOrEmpty(utilityObj.ThenBy) ? EntitySorter<T>.OrderByDescending(utilityObj.SortBy) : EntitySorter<T>.OrderByDescending(utilityObj.SortBy).ThenByDescending(utilityObj.ThenBy));
                    // : EntitySorter<T>.OrderByDescending(utilityObj.SortBy).ThenByDescending(utilityObj.ThenBy);
                IOrderedQueryable<T> sortedList = sorter.Sort(cntry);

                if (utilityObj.CurrentPage != -1 && utilityObj.PageSize != -1) //apply paging
                {
                    return sortedList.Skip(firstRec).Take(utilityObj.PageSize);
                }
                else //apply no paging if curr page or page siz3e is  -1
                {
                    return sortedList;
                }
            }
            else
            {
                return cntry;

            }

            // return loadEntityQry;
        }
        /// <summary>
        /// Delete Entity object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="set"></param>
        /// <param name="entities"></param>
        public static void DeleteObjects<T>(this ObjectSet<T> set, IEnumerable<T> entities) where T : EntityObject
        {
            foreach (var entity in entities)
                set.DeleteObject(entity);
        }
        /// <summary>
        /// Copy Entity Object to another Entity Object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>to Entity</returns>
        public static T copyEntity<T>(T from, T to)
        {
            Type type = typeof(T);
            PropertyInfo[] pList = type.GetProperties();

            foreach (PropertyInfo pInfo in pList)
            {
                if (pInfo.CanWrite)
                {

                    if (pInfo.PropertyType.ToString().Contains("EntityReference") || pInfo.PropertyType.ToString().Contains("EntityCollection")
                        || pInfo.PropertyType.ToString().Contains("EntityState") || pInfo.PropertyType.ToString().Contains("EntityKey"))
                    {
                        continue;
                    }
                    if (pInfo.Name.ToLower().Contains("moddt") && pInfo.PropertyType == typeof(DateTime))
                    {
                        pInfo.SetValue(to, DateTime.Now, null);
                    }
                    else
                    {
                        pInfo.SetValue(to, pInfo.GetValue(from, null), null);
                    }
                }
            }

            return to;
        }
    }
}
