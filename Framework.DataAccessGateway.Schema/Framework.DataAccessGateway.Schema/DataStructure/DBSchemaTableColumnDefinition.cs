using Framework.DataAccessGateway.Core;
using Framework.DataAccessGateway.Schema.Collection;

namespace Framework.DataAccessGateway.Schema.DataStructure
{
    /// <summary>
    ///     Summary description for DBSchemaTableColumnDefinition
    /// </summary>
    public class DBSchemaTableColumnDefinition
    {
        #region Public Methods

        /// <summary>
        ///     Makes a copy of the instance
        /// </summary>
        /// <returns>DBSchemaTableColumnDefinition</returns>
        public DBSchemaTableColumnDefinition Copy()
        {
            var dbSchemaTableColumnDefinitionCopy = new DBSchemaTableColumnDefinition(ColumnName);

            dbSchemaTableColumnDefinitionCopy.DataType = DataType;

            foreach (var dbSchemaConstraintDefinition in dbSchemaConstraintDefinitionList)
            {
                dbSchemaTableColumnDefinitionCopy.dbSchemaConstraintDefinitionList.Add(
                    dbSchemaConstraintDefinition.Copy());
            }

            dbSchemaTableColumnDefinitionCopy.DefaultValue = DefaultValue;
            dbSchemaTableColumnDefinitionCopy.IdentityIncrement = IdentityIncrement;
            dbSchemaTableColumnDefinitionCopy.IdentitySeed = IdentitySeed;
            dbSchemaTableColumnDefinitionCopy.isIdentity = isIdentity;
            dbSchemaTableColumnDefinitionCopy.isNullable = isNullable;
            dbSchemaTableColumnDefinitionCopy.Length = Length;

            return dbSchemaTableColumnDefinitionCopy;
        }

        #endregion Public Methods

        #region Private Varialbles

        private DBSchemaConstraintDefinitionCollection dbSchemaConstraintDefinitionList =
            new DBSchemaConstraintDefinitionCollection();

        private bool isNullable = true;
        private bool isIdentity;

        #endregion Private Varialbles

        #region Public Properties

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
            get { return dbSchemaConstraintDefinitionList; }
            set { dbSchemaConstraintDefinitionList = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is nullable.
        /// </summary>
        /// <value><c>true</c> if this instance is nullable; otherwise, <c>false</c>.</value>
        public bool IsNullable
        {
            get { return isNullable; }
            set { isNullable = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is identity.
        /// </summary>
        /// <value><c>true</c> if this instance is identity; otherwise, <c>false</c>.</value>
        public bool IsIdentity
        {
            get { return isIdentity; }
            set
            {
                isIdentity = value;

                //Default to 1,1 values
                IdentitySeed = 1;
                IdentityIncrement = 1;
            }
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

        #endregion Public Properties

        #region Constructor

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
        }

        /// <summary>
        ///     Constructor definition
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="dataType"></param>
        /// <param name="length"></param>
        /// <param name="defaultValue"></param>
        /// <param name="isNullable"></param>
        public DBSchemaTableColumnDefinition(string columnName, DBHandlerDataType dataType, int length,
            string defaultValue, bool isNullable)
        {
            IdentityIncrement = null;
            IdentitySeed = null;
            ColumnName = columnName;
            DataType = dataType;
            DefaultValue = defaultValue;
            Length = length;
            this.isNullable = isNullable;
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
        public DBSchemaTableColumnDefinition(string columnName, DBHandlerDataType dataType, int length,
            DBSchemaConstraintDefinition dbSchemaConstraintDefinitionList, string defaultValue, bool isNullable,
            bool isIdentity, int? identitySeed, int? identityIncrement)
        {
            ColumnName = columnName;
            DataType = dataType;
            DefaultValue = defaultValue;
            Length = length;
            this.dbSchemaConstraintDefinitionList.Add(dbSchemaConstraintDefinitionList);
            this.isNullable = isNullable;
            this.isIdentity = isIdentity;
            IdentitySeed = identitySeed;
            IdentityIncrement = identityIncrement;
        }

        #endregion Constructor
    }
}