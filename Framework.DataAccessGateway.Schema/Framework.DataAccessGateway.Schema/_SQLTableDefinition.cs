namespace Framework.DataAccessGateway.Schema
{
    /// <summary>
    /// Class _SQLTableDefinition.
    /// </summary>
    internal class _SQLTableDefinition
    {
        public string TABLE_NAME { get; set; }

        public string COLUMN_NAME { get; set; }

        public string TYPE { get; set; }

        public string DEFAULT_VALUE { get; set; }

        public int? LENGTH { get; set; }

        public bool ISNULLABLE { get; set; }

        public bool ISIDENTITY { get; set; }

        public int? IDENTITY_SEEDVALUE { get; set; }

        public int? IDENTITY_INCREMENTVALUE { get; set; }
    }
}