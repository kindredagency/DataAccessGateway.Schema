namespace Framework.DataAccessGateway.Schema
{
    public class DBUserDefinedTableTypeDefinition
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DBUserDefinedTableTypeDefinitionColumn[] Columns { get; set; }
    }
}
