namespace Framework.DataAccessGateway.Schema
{
    /// <summary>
    /// Class _SQLTableDefinition.
    /// </summary>
    internal class _SQLTableDefinition
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
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string TYPE { get; set; }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>The default value.</value>
        public string DEFAULT_VALUE { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>The length.</value>
        public int? LENGTH { get; set; }

        /// <summary>
        /// Gets or sets the precision.
        /// </summary>
        /// <value>The precision.</value>
        public int? PRECISION { get; set; }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        /// <value>The scale.</value>
        public int? SCALE { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="_SQLTableDefinition"/> is isnullable.
        /// </summary>
        /// <value><c>true</c> if isnullable; otherwise, <c>false</c>.</value>
        public bool ISNULLABLE { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="_SQLTableDefinition"/> is isidentity.
        /// </summary>
        /// <value><c>true</c> if isidentity; otherwise, <c>false</c>.</value>
        public bool ISIDENTITY { get; set; }

        /// <summary>
        /// Gets or sets the identity seed value.
        /// </summary>
        /// <value>The identity seed value.</value>
        public int? IDENTITY_SEEDVALUE { get; set; }

        /// <summary>
        /// Gets or sets the identity increment value.
        /// </summary>
        /// <value>The identity increment value.</value>
        public int? IDENTITY_INCREMENTVALUE { get; set; }
    }
}