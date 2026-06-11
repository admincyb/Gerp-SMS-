using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data;
using System.ComponentModel;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Web;
using System.Linq.Expressions;
//using System.Data.;

namespace ERP.Utilities
{
    public static class Extensions
    {
        /// <summary>
        /// This Extension methode is used to create a
        /// Deep Copy of any Serializable Object
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="source">Object</param>
        /// <returns>Copied Object</returns>
        #region DeepClone
        public static T DeepClone<T>(this T source)
        {
            if (!typeof(T).IsSerializable) throw new ArgumentException("The type must be serializable.", "source");

            if (Object.ReferenceEquals(source, null)) return default(T);

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
        #endregion

        /// <summary>
        /// This is an Extension For IList<T> Convert to DataTable"/>
        /// </summary>
        /// <typeparam name="T">Type Of Object</typeparam>
        /// <param name="data">List</param>
        /// <returns>DataTable</returns>
        #region ToDataTable
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                //table.Columns.Add(prop.Name, prop.PropertyType);
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }
        #endregion

        /// <summary>
        /// This is an Extension For DataTable Convert to List<typeparam name="TSource"></typeparam>"/>
        /// </summary>
        /// <typeparam name="T">Type Of Object</typeparam>
        /// <param name="data">List</param>
        /// <returns>List<TSource></returns>
        #region ToList
        public static List<TSource> ToList<TSource>(this DataTable dataTable) where TSource : new()
        {
            var dataList = new List<TSource>();

            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;

            var objFieldNames = (from PropertyInfo aProp in typeof(TSource).GetProperties(flags)
                                 select new
                                 {
                                     Name = aProp.Name,
                                     Type = Nullable.GetUnderlyingType(aProp.PropertyType) ?? aProp.PropertyType
                                 }).ToList();

            var dataTblFieldNames = (from DataColumn aHeader in dataTable.Columns
                                     select new { Name = aHeader.ColumnName, Type = aHeader.DataType }).ToList();

            var commonFields = objFieldNames.Intersect(dataTblFieldNames).ToList();

            foreach (DataRow dataRow in dataTable.AsEnumerable().ToList())
            {
                var aTSource = new TSource();
                foreach (var aField in commonFields)
                {
                    PropertyInfo propertyInfos = aTSource.GetType().GetProperty(aField.Name);
                    var value = dataRow[aField.Name];

                    if (value == DBNull.Value)
                    {
                        value = null;
                    }

                    propertyInfos.SetValue(aTSource, value, null);
                }
                dataList.Add(aTSource);
            }

            return dataList;
        }
        #endregion

        /// <summary>
        /// This is an Extension method for checking the string is Null or Empty or WhiteSpace
        /// </summary>
        /// <param name="value">String</param>
        /// <returns>bool</returns>
        #region IsNullOrEmptyOrWhiteSpace
        public static bool IsNullOrEmptyOrWhitespace(this string value)
        {
            return string.IsNullOrWhiteSpace(value) || value == string.Empty;
        }
        #endregion

        #region StartDateOfTheMonth
        /// <summary>
        /// This is an Extension Which Returns the First Date of the given Month
        /// </summary>
        /// <param name="date">DateTime</param>
        /// <returns>DateTime</returns>
        public static DateTime StartDateOfTheMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }
        #endregion
        #region StartDateOfTheYear
        /// <summary>
        /// This is an Extension Which Returns the First Date of the current year
        /// </summary>
        /// <param name="date">DateTime</param>
        /// <returns>DateTime</returns>
        public static DateTime StartDateOfTheYear(this DateTime date)
        {
            return new DateTime(date.Year, 1, 1);
        }
        #endregion

        #region EndDateOfTheMonth
        /// <summary>
        /// This is an Extension Which Returns the End Date of the given Month
        /// </summary>
        /// <param name="date">DateTime</param>
        /// <returns>DateTime</returns>
        public static DateTime EndDateOfTheMonth(this DateTime date)
        {
            int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            return new DateTime(date.Year, date.Month, daysInMonth);
        }
        #endregion
        #region EndDateOfTheYear
        /// <summary>
        /// This is an Extension Which Returns the End Date of the current year
        /// </summary>
        /// <param name="date">DateTime</param>
        /// <returns>DateTime</returns>
        public static DateTime EndDateOfTheYear(this DateTime date)
        {
            return new DateTime(date.Year, 12, 31);
        }
        #endregion
        #region HtmlDecoding DropdownListItem
        /// <summary>
        /// This is an Extension for Html Decoding DropDown List Items
        /// </summary>
        /// <param name="items">ListItemCollection</param>
        /// <returns>void</returns>
        public static void HtmlDecode(this ListItemCollection items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Text = HttpUtility.HtmlDecode(items[i].Text);
            }
        }
        #endregion

        #region HtmlDecoding String
        /// <summary>
        /// This is an Extension for Html Decoding String
        /// </summary>
        /// <param name="items">Strig</param>
        /// <returns>String</returns>
        public static string HtmlDecode(this string value)
        {
            if (value.IsNullOrEmptyOrWhitespace()) return value;
            return HttpUtility.HtmlDecode(value);
        }
        #endregion

        #region HtmlEncoding String
        /// <summary>
        /// This is an Extension for Html Decoding String
        /// </summary>
        /// <param name="items">Strig</param>
        /// <returns>String</returns>
        public static string HtmlEncode(this string value)
        {
            if (value.IsNullOrEmptyOrWhitespace()) return value;
            return HttpUtility.HtmlEncode(value);
        }
        #endregion

        #region List Sort 
        /// <summary>
        /// This is an Extension for List Sorting
        /// </summary>
        /// <param name="items">Strig</param>
        /// <returns>String</returns>
        public static IEnumerable<T> ListSort<T>(this IEnumerable<T> source, string sortExpression, SortDirection sortDirection)
        {
            var param = Expression.Parameter(typeof(T), string.Empty);
            try
            {
                var fields = sortExpression.Split('.');
                Expression property = null;
                Expression parentParam = param;
                foreach (var field in fields)
                {
                    property = Expression.Property(parentParam, field);
                    parentParam = property;

                }

                var sortLambda =
                    Expression.Lambda<Func<T, object>>(
                      Expression.Convert(property, typeof(object)), param);
                if (sortDirection == SortDirection.Ascending)
                {
                    return source.AsQueryable<T>().
                         OrderBy<T, object>(sortLambda);
                }
                else
                {
                    return source.AsQueryable<T>().
                        OrderByDescending<T, object>(sortLambda);
                }
            }
            catch (ArgumentException)
            {
                throw;
            }
        }
        #endregion

        public static void AddCssClass(this WebControl control, string cssClass)
        {
            List<string> classes;
            if (!string.IsNullOrWhiteSpace(control.CssClass))
            {
                classes = control.CssClass.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (!classes.Contains(cssClass))
                    classes.Add(cssClass);
            }
            else
            {
                classes = new List<string> { cssClass };
            }
            control.CssClass = string.Join(" ", classes.ToArray());
        }

        public static void RemoveCssClass(this WebControl control, string cssClass)
        {
            List<string> classes = new List<string>();
            if (!string.IsNullOrWhiteSpace(control.CssClass))
            {
                classes = control.CssClass.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            classes.Remove(cssClass);
            control.CssClass = string.Join(" ", classes.ToArray());
        }
        public static string GetExceptionMessage(this Exception @exception)
        {
            if (@exception.InnerException == null) return @exception.Message;
            return @exception.InnerException.GetExceptionMessage();
        }
        #region GetFormatedDateString
        /// <summary>
        /// This is an Extension for Get Formated Date String
        /// </summary>
        /// <param name="objDateTime">object</param>
        /// <param name="format">string</param>
        /// <returns>String</returns>
        public static string GetFormatedDateString(this object objDateTime, string format)
        {
            string result = string.Empty;
            try
            {
                string date = Convert.ToString(objDateTime);
                DateTime dt;
                if (DateTime.TryParse(date, out dt)) result = dt.ToString(format);
            }
            catch (Exception ex)
            {
            }
            return result;
        }
        #endregion

        public static string GetInnerExceptionMsg(this Exception @this)
        {
            if (@this.InnerException != null)
                return GetExceptionMessage(@this.InnerException);

            return @this.Message;
        }
    }
}
