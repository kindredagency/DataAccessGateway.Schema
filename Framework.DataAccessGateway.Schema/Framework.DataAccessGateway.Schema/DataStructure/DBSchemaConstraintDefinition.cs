namespace Framework.DataAccessGateway.Schema.DataStructure
{
    /// <summary>
    /// Class DBSchemaConstraintDefinition.
    /// </summary>
    public class DBSchemaConstraintDefinition
    {
        /// <summary>
        /// Copies this instance.
        /// </summary>
        /// <returns>DBSchemaConstraintDefinition.</returns>
        public DBSchemaConstraintDefinition Copy()
        {
            var dbSchemaConstraintDefinitionCopy = new DBSchemaConstraintDefinition(ConstraintName, Constraint);

            dbSchemaConstraintDefinitionCopy.TableName = TableName;
            dbSchemaConstraintDefinitionCopy.ColumnName = ColumnName;
            dbSchemaConstraintDefinitionCopy.RelatedTableName = RelatedTableName;
            dbSchemaConstraintDefinitionCopy.RelatedColumnName = RelatedColumnName;

            return dbSchemaConstraintDefinitionCopy;
        }

        /// <summary>
        /// Gets or sets the name of the related column.
        /// </summary>
        /// <value>The name of the related column.</value>
        public string RelatedColumnName { get; set; }

        /// <summary>
        /// Gets or sets the name of the related table.
        /// </summary>
        /// <value>The name of the related table.</value>
        public string RelatedTableName { get; set; }

        /// <summary>
        /// Gets or sets the name of the column.
        /// </summary>
        /// <value>The name of the column.</value>
        public string ColumnName { get; set; }

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        /// <value>The name of the table.</value>
        public string TableName { get; set; }

        /// <summary>
        /// Gets or sets the constraint.
        /// </summary>
        /// <value>The constraint.</value>
        public ConstraintType Constraint { get; set; }

        /// <summary>
        /// Gets the name of the constraint.
        /// </summary>
        /// <value>The name of the constraint.</value>
        public string ConstraintName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DBSchemaConstraintDefinition"/> class.
        /// </summary>
        /// <param name="constraintName">Name of the constraint.</param>
        public DBSchemaConstraintDefinition(string constraintName)
        {
            ConstraintName = constraintName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DBSchemaConstraintDefinition"/> class.
        /// </summary>
        /// <param name="constraintName">Name of the constraint.</param>
        /// <param name="constraintType">Type of the constraint.</param>
        public DBSchemaConstraintDefinition(string constraintName, ConstraintType constraintType)
        {
            ConstraintName = constraintName;
            Constraint = constraintType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DBSchemaConstraintDefinition"/> class.
        /// </summary>
        /// <param name="constraintType">Type of the constraint.</param>
        public DBSchemaConstraintDefinition(ConstraintType constraintType)
        {
            ConstraintName = null;
            Constraint = constraintType;
        }
    }
}