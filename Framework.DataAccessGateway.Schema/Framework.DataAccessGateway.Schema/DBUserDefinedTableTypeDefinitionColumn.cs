﻿namespace Framework.DataAccessGateway.Schema
{
    public class DBUserDefinedTableTypeDefinitionColumn
    {
        public int ColumnId { get; set; }

        public string Name { get; set; }

        public string DataType { get; set; }

        public bool IsNullable { get; set; }

        public int Length { get; set; }

        public int Precision { get; set; }

        public int Scale { get; set; }

        public string Collation { get; set; }
    }
}
