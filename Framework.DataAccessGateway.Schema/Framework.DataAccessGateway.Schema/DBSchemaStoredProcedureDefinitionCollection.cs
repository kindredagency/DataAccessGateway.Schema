using System.Collections;
using System.Collections.Generic;

namespace Framework.DataAccessGateway.Schema
{
    /// <summary>
    /// Class DBSchemaStoredProcedureDefinitionCollection.
    /// </summary>
    public class DBSchemaStoredProcedureDefinitionCollection : IList<DBSchemaStoredProcedureDefinition>
    {    
        private readonly List<DBSchemaStoredProcedureDefinition> procedures = new List<DBSchemaStoredProcedureDefinition>();     

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<DBSchemaStoredProcedureDefinition> GetEnumerator()
        {
            return procedures.GetEnumerator();
        }     

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return procedures.GetEnumerator();
        }        

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.Int32.</returns>
        public int IndexOf(DBSchemaStoredProcedureDefinition item)
        {
            return procedures.IndexOf(item);
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        public void Insert(int index, DBSchemaStoredProcedureDefinition item)
        {
            procedures.Insert(index, item);
        }

        /// <summary>
        /// Removes at.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveAt(int index)
        {
            procedures.RemoveAt(index);
        }

        /// <summary>
        /// Gets or sets the <see cref="DBSchemaStoredProcedureDefinition"/> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>DBSchemaStoredProcedureDefinition.</returns>
        public DBSchemaStoredProcedureDefinition this[int index]
        {
            get { return procedures[index]; }
            set { procedures[index] = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="DBSchemaStoredProcedureDefinition"/> with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>DBSchemaStoredProcedureDefinition.</returns>
        public DBSchemaStoredProcedureDefinition this[string name]
        {
            get
            {
                foreach (var dbSchemaStoredProcedureDefinition in procedures)
                {
                    if (dbSchemaStoredProcedureDefinition.Name == name)
                    {
                        return dbSchemaStoredProcedureDefinition;
                    }
                }

                return null;
            }
            set
            {
                DBSchemaStoredProcedureDefinition tempDBSchemaStoredProcedureDefinition = null;
                foreach (var dbSchemaStoredProcedureDefinition in procedures)
                {
                    if (dbSchemaStoredProcedureDefinition.Name == name)
                    {
                        tempDBSchemaStoredProcedureDefinition = dbSchemaStoredProcedureDefinition;
                    }
                }
                tempDBSchemaStoredProcedureDefinition = value;
            }
        }       

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(DBSchemaStoredProcedureDefinition item)
        {
            procedures.Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear()
        {
            procedures.Clear();
        }

        /// <summary>
        /// Determines whether [contains] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.</returns>
        public bool Contains(DBSchemaStoredProcedureDefinition item)
        {
            return procedures.Contains(item);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(DBSchemaStoredProcedureDefinition[] array, int arrayIndex)
        {
            procedures.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return procedures.Count; }
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
        public bool Remove(DBSchemaStoredProcedureDefinition item)
        {
            return procedures.Remove(item);
        }        
    }
}