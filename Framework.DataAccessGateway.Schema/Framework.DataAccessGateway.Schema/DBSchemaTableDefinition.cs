using System.Linq;

namespace Framework.DataAccessGateway.Schema
{
    /// <summary>
    ///     Summary description for DBSchemaTableDefinition
    /// </summary>
    public class DBSchemaTableDefinition
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get; set;
        }

        /// <summary>
        ///     List of column definition objects
        /// </summary>
        public DBSchemaTableColumnDefinitionCollection ColumnDefinitionList
        {
            get; set;
        }

        /// <summary>
        ///     No of primary keys in the table
        /// </summary>
        public int NoOfPrimaryKeys
        {
            get
            {
                var count = 0;

                foreach (var column in ColumnDefinitionList)
                {
                    if (column.DBSchemaConstraintDefinitionList.Where(c => c.Constraint == ConstraintType.PrimaryKey).Count() > 0)
                    {
                        count++;
                    }
                }

                return count;
            }
        }
       
        /// <summary>
        ///     Initializes a new instance of the <see cref="DBSchemaTableDefinition" /> class.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        public DBSchemaTableDefinition(string tableName)
        {
            Name = tableName;
            ColumnDefinitionList = new DBSchemaTableColumnDefinitionCollection();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DBSchemaTableDefinition" /> class.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnDefinitionList">The column definition list.</param>
        public DBSchemaTableDefinition(string tableName, DBSchemaTableColumnDefinitionCollection columnDefinitionList)
        {
            Name = tableName;
            ColumnDefinitionList = columnDefinitionList;
        }

        /// <summary>
        ///     Makes a copy of the instance
        /// </summary>
        /// <returns>DBSchemaTableDefinition</returns>
        public DBSchemaTableDefinition Copy()
        {
            var dbSchemaTableDefinitionCopy = new DBSchemaTableDefinition(Name);

            // Assign attributes
            foreach (var dbSchemaTableColumnDefinition in ColumnDefinitionList)
            {
                dbSchemaTableDefinitionCopy.ColumnDefinitionList.Add(dbSchemaTableColumnDefinition.Copy());
            }

            return dbSchemaTableDefinitionCopy;
        }
    }
}