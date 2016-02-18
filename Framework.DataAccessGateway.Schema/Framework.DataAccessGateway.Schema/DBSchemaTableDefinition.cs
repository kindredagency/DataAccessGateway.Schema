using System.Linq;

namespace Framework.DataAccessGateway.Schema
{
    /// <summary>
    ///     Summary description for DBSchemaTableDefinition
    /// </summary>
    public class DBSchemaTableDefinition
    {
        #region Public Methods

        /// <summary>
        ///     Makes a copy of the instance
        /// </summary>
        /// <returns>DBSchemaTableDefinition</returns>
        public DBSchemaTableDefinition Copy()
        {
            var dbSchemaTableDefinitionCopy = new DBSchemaTableDefinition(tableName);

            // Assign attributes
            foreach (var dbSchemaTableColumnDefinition in columnDefinitionList)
            {
                dbSchemaTableDefinitionCopy.columnDefinitionList.Add(dbSchemaTableColumnDefinition.Copy());
            }

            return dbSchemaTableDefinitionCopy;
        }

        #endregion Public Methods

        #region Private Variables

        private string tableName;

        private DBSchemaTableColumnDefinitionCollection columnDefinitionList =
            new DBSchemaTableColumnDefinitionCollection();

        #endregion Private Variables

        #region Public Properties

        /// <summary>
        ///     Table Name
        /// </summary>
        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }

        /// <summary>
        ///     List of column definition objects
        /// </summary>
        public DBSchemaTableColumnDefinitionCollection ColumnDefinitionList
        {
            get { return columnDefinitionList; }
            set { columnDefinitionList = value; }
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
                    if (
                        column.DBSchemaConstraintDefinitionList.Where(c => c.Constraint == ConstraintType.PrimaryKey)
                            .Count() > 0)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        #endregion Public Properties

        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="DBSchemaTableDefinition" /> class.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        public DBSchemaTableDefinition(string tableName)
        {
            this.tableName = tableName;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DBSchemaTableDefinition" /> class.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnDefinitionList">The column definition list.</param>
        public DBSchemaTableDefinition(string tableName, DBSchemaTableColumnDefinitionCollection columnDefinitionList)
        {
            this.tableName = tableName;
            this.columnDefinitionList = columnDefinitionList;
        }

        #endregion Constructor
    }
}