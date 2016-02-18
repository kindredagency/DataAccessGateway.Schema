using Framework.DataAccessGateway.Core;
using Framework.DataAccessGateway.Schema;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;

namespace UnitTest
{
    [TestClass]
    public class UnitTestDBSchemaHandler
    {
        private string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            }
        }      

        [TestMethod]
        public void GetDB()
        {            
            IDBSchemaHandler dbSchemaHandler = new DBSchemaHandler(ConnectionString, DBHandlerType.DbHandlerMSSQL);

            var db = dbSchemaHandler.GetDataBaseDefinition();

            int procCount = db.Procs.Count;
            int tableCount = db.Tables.Count;
            int triggerCount = db.Triggers.Count;           
        }

        [TestMethod]
        public void GetTableDefinition()
        {
            IDBSchemaHandler dbSchemaHandler = new DBSchemaHandler(ConnectionString, DBHandlerType.DbHandlerMSSQL);

            var table = dbSchemaHandler.GetTableDefinition("Data");

            Assert.AreSame(table.Name, "Data");
        }

        [TestMethod]
        public void GetTableDefinitionListing()
        {
            IDBSchemaHandler dbSchemaHandler = new DBSchemaHandler(ConnectionString, DBHandlerType.DbHandlerMSSQL);

            var tables = dbSchemaHandler.GetTableDefinitionListing();

            Assert.AreNotEqual(tables.Count, 0);
        }

        [TestMethod]
        public void GetStoredProcedureDefinition()
        {
            IDBSchemaHandler dbSchemaHandler = new DBSchemaHandler(ConnectionString, DBHandlerType.DbHandlerMSSQL);

            var procedure = dbSchemaHandler.GetStoredProcedureDefinition("InsertAndSelect");

            if(procedure != null)
                Assert.AreEqual(procedure.Name, "InsertAndSelect");
        }

        [TestMethod]
        public void GetStoredProcedureDefinitionListing()
        {
            IDBSchemaHandler dbSchemaHandler = new DBSchemaHandler(ConnectionString, DBHandlerType.DbHandlerMSSQL);

            var procedures = dbSchemaHandler.GetStoredProcedureDefinitionListing();

            Assert.AreNotEqual(procedures.Count, 0);
        }

        [TestMethod]
        public void GetTriggerDefinition()
        {
            IDBSchemaHandler dbSchemaHandler = new DBSchemaHandler(ConnectionString, DBHandlerType.DbHandlerMSSQL);

            var trigger = dbSchemaHandler.GetTriggerDefinition("DataTrigger");

            if(trigger != null)
                Assert.AreEqual(trigger.TriggerName, "DataTrigger");
        }

        [TestMethod]
        public void GetTriggerDefinitionListing()
        {
            IDBSchemaHandler dbSchemaHandler = new DBSchemaHandler(ConnectionString, DBHandlerType.DbHandlerMSSQL);

            var triggers = dbSchemaHandler.GetTriggerDefinitionListing();

            Assert.AreNotEqual(triggers.Count, 0);
        }
    }
}
