using Framework.DataAccessGateway.Core;

namespace Framework.DataAccessGateway.Schema
{
    /// <summary>
    ///     Summary description for DBSchemaTableColumnDefinition
    /// </summary>
    public class DBSchemaTableColumnDefinition
    {
        /// <summary>
        ///     Column Name
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        ///     Column Data Type
        /// </summary>
        public DBHandlerDataType DataType { get; set; }

        /// <summary>
        ///     Column default value
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        ///     Length of the data type
        /// </summary>
        public int? Length { get; set; }

        /// <summary>
        ///     States if it a primary key, foreign key etc.
        /// </summary>
        public DBSchemaConstraintDefinitionCollection DBSchemaConstraintDefinitionList
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is nullable.
        /// </summary>
        /// <value><c>true</c> if this instance is nullable; otherwise, <c>false</c>.</value>
        public bool IsNullable
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is identity.
        /// </summary>
        /// <value><c>true</c> if this instance is identity; otherwise, <c>false</c>.</value>
        public bool IsIdentity
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the identity seed.
        /// </summary>
        /// <value>The identity seed.</value>
        public int? IdentitySeed { get; set; }

        /// <summary>
        /// Gets or sets the identity increment.
        /// </summary>
        /// <value>The identity increment.</value>
        public int? IdentityIncrement { get; set; }
    

        /// <summary>
        ///     Constructor definition
        /// </summary>
        /// <param name="columnName">column Name</param>
        public DBSchemaTableColumnDefinition(string columnName)
        {
            IdentityIncrement = null;
            IdentitySeed = null;
            Length = null;
            DefaultValue = null;
            ColumnName = columnName;
            DBSchemaConstraintDefinitionList = new DBSchemaConstraintDefinitionCollection();
        }

        /// <summary>
        ///     Constructor definition
        /// </summary>
        /// <param name="columnName">column name</param>
        /// <param name="dataType">data type</param>
        public DBSchemaTableColumnDefinition(string columnName, DBHandlerDataType dataType)
        {
            IdentityIncrement = null;
            IdentitySeed = null;
            Length = null;
            DefaultValue = null;
            ColumnName = columnName;
            DataType = dataType;
            DBSchemaConstraintDefinitionList = new DBSchemaConstraintDefinitionCollection();
        }

        /// <summary>
        ///     Constructor definition
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="dataType"></param>
        /// <param name="length"></param>
        /// <param name="defaultValue"></param>
        /// <param name="isNullable"></param>
        public DBSchemaTableColumnDefinition(string columnName, DBHandlerDataType dataType, int length,  string defaultValue, bool isNullable)
        {
            IdentityIncrement = null;
            IdentitySeed = null;
            ColumnName = columnName;
            DataType = dataType;
            DefaultValue = defaultValue;
            Length = length;
            IsNullable = isNullable;
            DBSchemaConstraintDefinitionList = new DBSchemaConstraintDefinitionCollection();
        }

        /// <summary>
        ///     Constructor definition
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="dataType"></param>
        /// <param name="length"></param>
        /// <param name="dbSchemaConstraintDefinitionList"></param>
        /// <param name="defaultValue"></param>
        /// <param name="isNullable"></param>
        /// <param name="isIdentity"></param>
        /// <param name="identitySeed"></param>
        /// <param name="identityIncrement"></param>
        public DBSchemaTableColumnDefinition(string columnName, DBHandlerDataType dataType, int length,  DBSchemaConstraintDefinition dbSchemaConstraintDefinitionList, string defaultValue, bool isNullable,  bool isIdentity, int? identitySeed, int? identityIncrement)
        {
            ColumnName = columnName;
            DataType = dataType;
            DefaultValue = defaultValue;
            Length = length;            
            IsNullable = isNullable;
            IsIdentity = isIdentity;
            IdentitySeed = identitySeed;
            IdentityIncrement = identityIncrement;
            DBSchemaConstraintDefinitionList = new DBSchemaConstraintDefinitionCollection();
            DBSchemaConstraintDefinitionList.Add(dbSchemaConstraintDefinitionList);
        }       


        /// <summary>
        ///     Makes a copy of the instance
        /// </summary>
        /// <returns>DBSchemaTableColumnDefinition</returns>
        public DBSchemaTableColumnDefinition Copy()
        {
            var dbSchemaTableColumnDefinitionCopy = new DBSchemaTableColumnDefinition(ColumnName);

            dbSchemaTableColumnDefinitionCopy.DataType = DataType;

            foreach (var dbSchemaConstraintDefinition in DBSchemaConstraintDefinitionList)
            {
                dbSchemaTableColumnDefinitionCopy.DBSchemaConstraintDefinitionList.Add(dbSchemaConstraintDefinition.Copy());
            }

            dbSchemaTableColumnDefinitionCopy.DefaultValue = DefaultValue;
            dbSchemaTableColumnDefinitionCopy.IdentityIncrement = IdentityIncrement;
            dbSchemaTableColumnDefinitionCopy.IdentitySeed = IdentitySeed;
            dbSchemaTableColumnDefinitionCopy.IsIdentity = IsIdentity;
            dbSchemaTableColumnDefinitionCopy.IsNullable = IsNullable;
            dbSchemaTableColumnDefinitionCopy.Length = Length;

            return dbSchemaTableColumnDefinitionCopy;
        }
    }
}