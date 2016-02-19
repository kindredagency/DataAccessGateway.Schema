namespace Framework.DataAccessGateway.Schema
{
    /// <summary>
    /// Class DBSchemaViewDefinition.
    /// </summary>
    public class DBSchemaViewDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DBSchemaViewDefinition" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public DBSchemaViewDefinition(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Copies this instance.
        /// </summary>
        /// <returns>DBSchemaViewDefinition.</returns>
        public DBSchemaViewDefinition Copy()
        {
            var dbSchemaViewrDefinition = new DBSchemaViewDefinition(Name);

            dbSchemaViewrDefinition.IsIndexed = IsIndexed;
            dbSchemaViewrDefinition.IsIndexable = IsIndexable;
            dbSchemaViewrDefinition.Definition = Definition;

            return dbSchemaViewrDefinition;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is indexed.
        /// </summary>
        /// <value><c>true</c> if this instance is indexed; otherwise, <c>false</c>.</value>
        public bool IsIndexed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is indexable.
        /// </summary>
        /// <value><c>true</c> if this instance is indexable; otherwise, <c>false</c>.</value>
        public bool IsIndexable { get; set; }

        /// <summary>
        /// Gets or sets the definition.
        /// </summary>
        /// <value>The definition.</value>
        public string Definition { get; set; }
     
    }
}