using Framework.DataAccessGateway.Core;
using System.Linq;

namespace Framework.DataAccessGateway.Schema
{
    internal static class ExtensionMethods
    {
        /// <summary>
        /// To the type of the database handler data.
        /// </summary>
        /// <param name="sqlDataType">The value.</param>
        /// <returns>DBHandlerDataType.</returns>
        public static DBHandlerDataType ToDBHandlerDataType(this string sqlDataType)
        {
            return DBHandlerDataMapping.Mappings.Where(c => c.SqlDataType.ToString().ToLower() == sqlDataType.ToLower()).FirstOrDefault().DBHandlerDataType;
        }

        /// <summary>
        /// To the type of the constraint.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ConstraintType.</returns>
        public static ConstraintType ToConstraintType(this string value)
        {
            switch (value)
            {
                case "PRIMARY KEY":
                    return ConstraintType.PrimaryKey;

                case "FOREIGN KEY":
                    return ConstraintType.ForeignKey;

                case "UNIQUE":
                    return ConstraintType.Unique;
            }

            return ConstraintType.NotDefined;
        }
    }
}
