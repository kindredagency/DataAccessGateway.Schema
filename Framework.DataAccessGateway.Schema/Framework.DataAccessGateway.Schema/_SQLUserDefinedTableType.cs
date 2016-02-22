namespace Framework.DataAccessGateway.Schema
{
    public class _SQLUserDefinedTableType
    {
        public int ID { get; set; }

        public string TYPE_NAME { get; set; }

        public int COLUMN_ID { get; set; }
         
        public string COLUMN { get; set; }

        public string DATA_TYPE { get; set; }

        public bool IS_NULLABLE { get; set; } 

        public int LENGTH { get; set; }

        public int PRECISION { get; set; }

        public int SCALE { get; set; }

        public string COLLATION { get; set; }
    }
}
