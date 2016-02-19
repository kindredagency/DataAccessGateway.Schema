namespace Framework.DataAccessGateway.Schema
{
    /// <summary>
    /// Class _SQLConstraint.
    /// </summary>
    internal class _SQLConstraint
    {
        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        /// <value>The name of the table.</value>
        public string TABLE_NAME { get; set; }

        /// <summary>
        /// Gets or sets the name of the column.
        /// </summary>
        /// <value>The name of the column.</value>
        public string COLUMN_NAME { get; set; }

        /// <summary>
        /// Gets or sets the type of the constraint.
        /// </summary>
        /// <value>The type of the constraint.</value>
        public string CONSTRAINT_TYPE { get; set; }

        /// <summary>
        /// Gets or sets the name of the constraint.
        /// </summary>
        /// <value>The name of the constraint.</value>
        public string CONSTRAINT_NAME { get; set; }

        /// <summary>
        /// Gets or sets the name of the related table.
        /// </summary>
        /// <value>The name of the related table.</value>
        public string RELATED_TABLE_NAME { get; set; }

        /// <summary>
        /// Gets or sets the name of the related column.
        /// </summary>
        /// <value>The name of the related column.</value>
        public string RELATED_COLUMN_NAME { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>The position.</value>
        public int? POSITION { get; set; }

        /// <summary>
        /// Gets or sets the related position.
        /// </summary>
        /// <value>The related position.</value>
        public int? RELATED_POSITION { get; set; }
    }
}