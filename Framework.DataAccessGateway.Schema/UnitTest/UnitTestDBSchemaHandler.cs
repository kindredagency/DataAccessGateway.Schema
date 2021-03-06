﻿using Framework.DataAccessGateway.Core;
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
        public void Properties()
        {
            IDBSchemaHandler dbSchemaHandler = new DBSchemaHandler(ConnectionString, DBHandlerType.DbHandlerMSSQL);

            Assert.AreNotEqual(dbSchemaHandler.ConnectionString, null);
            Assert.AreNotEqual(dbSchemaHandler.DataBaseName, null);
            Assert.AreNotEqual(dbSchemaHandler.ServerName, null);
        }   

        [TestMethod]
        public void GetDB()
        {            
            IDBSchemaHandler dbSchemaHandler = new DBSchemaHandler(ConnectionString, DBHandlerType.DbHandlerMSSQL);

            var db = dbSchemaHandler.GetDataBaseDefinition();          

            Assert.AreNotEqual(db.Procs.Count, 0);
            Assert.AreNotEqual(db.Tables.Count, 0);
            Assert.AreNotEqual(db.Triggers.Count, 0);
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

        [TestMethod]
        public void GetView()
        {
            IDBSchemaHandler dbSchemaHandler = new DBSchemaHandler(ConnectionString, DBHandlerType.DbHandlerMSSQL);

            var view = dbSchemaHandler.GetView("SampleView");

            if (view != null)
                Assert.AreEqual(view.Name, "SampleView");
        }

        [TestMethod]
        public void GetViewListing()
        {
            IDBSchemaHandler dbSchemaHandler = new DBSchemaHandler(ConnectionString, DBHandlerType.DbHandlerMSSQL);

            var views = dbSchemaHandler.GetViewListing();

            Assert.AreNotEqual(views.Count, 0);
        }

        [TestMethod]
        public void GetConstraintDefinitionCollection()
        {
            IDBSchemaHandler dbSchemaHandler = new DBSchemaHandler(ConnectionString, DBHandlerType.DbHandlerMSSQL);

            var views = dbSchemaHandler.GetConstraintDefinitionCollection();

            Assert.AreNotEqual(views.Count, 0);
        }

        [TestMethod]
        public void GetUserDefinedTableType()
        {
            IDBSchemaHandler dbSchemaHandler = new DBSchemaHandler(ConnectionString, DBHandlerType.DbHandlerMSSQL);

            var view = dbSchemaHandler.GetUserDefinedTableType("_Data");

            if (view != null)
                Assert.AreEqual(view.Name, "_Data");
        }

        [TestMethod]
        public void GetUserDefinedTableTypes()
        {
            IDBSchemaHandler dbSchemaHandler = new DBSchemaHandler(ConnectionString, DBHandlerType.DbHandlerMSSQL);

            var views = dbSchemaHandler.GetUserDefinedTableTypes();

            Assert.AreNotEqual(views.Count, 0);
        }

    }
}
