using System.Collections;
using System.Collections.Generic;

namespace Framework.DataAccessGateway.Schema
{

    /// <summary>
    /// Class DBSchemaViewDefinitionCollection.
    /// </summary>
    public class DBSchemaViewDefinitionCollection : IList<DBSchemaViewDefinition>
    {
        /// <summary>
        /// The views
        /// </summary>
        private readonly List<DBSchemaViewDefinition> views = new List<DBSchemaViewDefinition>();

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<DBSchemaViewDefinition> GetEnumerator()
        {
            return views.GetEnumerator();
        }


        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return views.GetEnumerator();
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.Int32.</returns>
        public int IndexOf(DBSchemaViewDefinition item)
        {
            return views.IndexOf(item);
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        public void Insert(int index, DBSchemaViewDefinition item)
        {
            views.Insert(index, item);
        }

        /// <summary>
        /// Removes at.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveAt(int index)
        {
            views.RemoveAt(index);
        }

        /// <summary>
        /// Gets or sets the <see cref="DBSchemaViewDefinition" /> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>DBSchemaViewDefinition.</returns>
        public DBSchemaViewDefinition this[int index]
        {
            get { return views[index]; }
            set { views[index] = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="DBSchemaViewDefinition" /> with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>DBSchemaViewDefinition.</returns>
        public DBSchemaViewDefinition this[string name]
        {
            get
            {
                foreach (var dbSchemaViewDefinition in views)
                {
                    if (dbSchemaViewDefinition.Name == name)
                    {
                        return dbSchemaViewDefinition;
                    }
                }

                return null;
            }
            set
            {
                DBSchemaViewDefinition tempDbSchemaViewDefinition = null;
                foreach (var dbSchemaTriggerDefinition in views)
                {
                    if (dbSchemaTriggerDefinition.Name == name)
                    {
                        tempDbSchemaViewDefinition = dbSchemaTriggerDefinition;
                    }
                }

                tempDbSchemaViewDefinition = value;
            }
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(DBSchemaViewDefinition item)
        {
            views.Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear()
        {
            views.Clear();
        }

        /// <summary>
        /// Determines whether [contains] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.</returns>
        public bool Contains(DBSchemaViewDefinition item)
        {
            return views.Contains(item);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(DBSchemaViewDefinition[] array, int arrayIndex)
        {
            views.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return views.Count; }
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
        public bool Remove(DBSchemaViewDefinition item)
        {
            return views.Remove(item);
        }
       
    }
}