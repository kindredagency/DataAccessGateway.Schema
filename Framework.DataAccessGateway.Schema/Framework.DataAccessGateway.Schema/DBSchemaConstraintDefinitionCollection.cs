using System.Collections;
using System.Collections.Generic;

namespace Framework.DataAccessGateway.Schema
{
    /// <summary>
    /// Class DBSchemaConstraintDefinitionCollection.
    /// </summary>
    public class DBSchemaConstraintDefinitionCollection : IList<DBSchemaConstraintDefinition>
    {   
        private readonly List<DBSchemaConstraintDefinition> dbSchemaConstraintDefinitionList = new List<DBSchemaConstraintDefinition>();

        public DBSchemaConstraintDefinitionCollection()
        {

        }

        public DBSchemaConstraintDefinitionCollection(List<DBSchemaConstraintDefinition> schemaConstraintDefinitionList)
        {
            dbSchemaConstraintDefinitionList = schemaConstraintDefinitionList;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<DBSchemaConstraintDefinition> GetEnumerator()
        {
            return dbSchemaConstraintDefinitionList.GetEnumerator();
        }        

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return dbSchemaConstraintDefinitionList.GetEnumerator();
        }     

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.Int32.</returns>
        public int IndexOf(DBSchemaConstraintDefinition item)
        {
            return dbSchemaConstraintDefinitionList.IndexOf(item);
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        public void Insert(int index, DBSchemaConstraintDefinition item)
        {
            dbSchemaConstraintDefinitionList.Insert(index, item);
        }

        /// <summary>
        /// Removes at.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveAt(int index)
        {
            dbSchemaConstraintDefinitionList.RemoveAt(index);
        }

        /// <summary>
        /// Gets or sets the <see cref="DBSchemaConstraintDefinition"/> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>DBSchemaConstraintDefinition.</returns>
        public DBSchemaConstraintDefinition this[int index]
        {
            get { return dbSchemaConstraintDefinitionList[index]; }
            set { dbSchemaConstraintDefinitionList[index] = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="DBSchemaConstraintDefinition"/> with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>DBSchemaConstraintDefinition.</returns>
        public DBSchemaConstraintDefinition this[string name]
        {
            get
            {
                foreach (var dbSchemaConstraintDefinition in dbSchemaConstraintDefinitionList)
                {
                    if (dbSchemaConstraintDefinition.ConstraintName == name)
                    {
                        return dbSchemaConstraintDefinition;
                    }
                }

                return null;
            }
            set
            {
                DBSchemaConstraintDefinition tempDBSchemaConstraintDefinition = null;
                foreach (var dbSchemaConstraintDefinition in dbSchemaConstraintDefinitionList)
                {
                    if (dbSchemaConstraintDefinition.ConstraintName == name)
                    {
                        tempDBSchemaConstraintDefinition = dbSchemaConstraintDefinition;
                    }
                }
                tempDBSchemaConstraintDefinition = value;
            }
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(DBSchemaConstraintDefinition item)
        {
            dbSchemaConstraintDefinitionList.Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear()
        {
            dbSchemaConstraintDefinitionList.Clear();
        }

        /// <summary>
        /// Determines whether /[contains] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.</returns>
        public bool Contains(DBSchemaConstraintDefinition item)
        {
            return dbSchemaConstraintDefinitionList.Contains(item);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(DBSchemaConstraintDefinition[] array, int arrayIndex)
        {
            dbSchemaConstraintDefinitionList.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return dbSchemaConstraintDefinitionList.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if item cannot be found then, <c>false</c> otherwise.</returns>
        public bool Remove(DBSchemaConstraintDefinition item)
        {
            return dbSchemaConstraintDefinitionList.Remove(item);
        }        
    }
}