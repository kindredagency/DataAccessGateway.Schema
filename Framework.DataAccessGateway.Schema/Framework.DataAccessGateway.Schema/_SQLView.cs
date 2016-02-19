namespace Framework.DataAccessGateway.Schema
{
    /// <summary>
    /// Class _SQLVIEW.
    /// </summary>
    internal class _SQLView
    {
        /// <summary>
        /// Gets or sets the object identifier.
        /// </summary>
        /// <value>The object identifier.</value>
        public int OBJECT_ID { get; set; }
        /// <summary>
        /// Gets or sets the name of the schema.
        /// </summary>
        /// <value>The name of the schema.</value>
        public string SCHEMA_NAME { get; set; }
        /// <summary>
        /// Gets or sets the name of the view.
        /// </summary>
        /// <value>The name of the view.</value>
        public string VIEW_NAME { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="_SQLView"/> is is indexed.
        /// </summary>
        /// <value><c>true</c> if is indexed; otherwise, <c>false</c>.</value>
        public bool ISINDEXED { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="_SQLView"/> is is indexable.
        /// </summary>
        /// <value><c>true</c> if is indexable; otherwise, <c>false</c>.</value>
        public bool ISINDEXABLE { get; set; }
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string TEXT  { get; set;  }
    }
}