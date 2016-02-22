using System.Collections;
using System.Collections.Generic;

namespace Framework.DataAccessGateway.Schema
{

    /// <summary>
    /// Class DBUserDefinedTableTypeCollection.
    /// </summary>
    public class DBUserDefinedTableTypeCollection : IList<DBUserDefinedTableTypeDefinition>
    {
        /// <summary>
        /// The database user defined table type definition list
        /// </summary>
        private readonly List<DBUserDefinedTableTypeDefinition> dbUserDefinedTableTypeDefinitionList = new List<DBUserDefinedTableTypeDefinition>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DBUserDefinedTableTypeCollection"/> class.
        /// </summary>
        public DBUserDefinedTableTypeCollection()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DBUserDefinedTableTypeCollection"/> class.
        /// </summary>
        /// <param name="userDefinedTableTypeDefinition">The user defined table type definition.</param>
        public DBUserDefinedTableTypeCollection(List<DBUserDefinedTableTypeDefinition> userDefinedTableTypeDefinition)
        {
            dbUserDefinedTableTypeDefinitionList = userDefinedTableTypeDefinition;
        }


        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<DBUserDefinedTableTypeDefinition> GetEnumerator()
        {
            return dbUserDefinedTableTypeDefinitionList.GetEnumerator();
        }


        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return dbUserDefinedTableTypeDefinitionList.GetEnumerator();
        }


        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.Int32.</returns>
        public int IndexOf(DBUserDefinedTableTypeDefinition item)
        {
            return dbUserDefinedTableTypeDefinitionList.IndexOf(item);
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        public void Insert(int index, DBUserDefinedTableTypeDefinition item)
        {
            dbUserDefinedTableTypeDefinitionList.Insert(index, item);
        }

        /// <summary>
        /// Removes at.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveAt(int index)
        {
            dbUserDefinedTableTypeDefinitionList.RemoveAt(index);
        }


        /// <summary>
        /// Gets or sets the <see cref="DBUserDefinedTableTypeDefinition"/> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>DBUserDefinedTableTypeDefinition.</returns>
        public DBUserDefinedTableTypeDefinition this[int index]
        {
            get { return dbUserDefinedTableTypeDefinitionList[index]; }
            set { dbUserDefinedTableTypeDefinitionList[index] = value; }
        }


        /// <summary>
        /// Gets or sets the <see cref="DBUserDefinedTableTypeDefinition"/> with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>DBUserDefinedTableTypeDefinition.</returns>
        public DBUserDefinedTableTypeDefinition this[string name]
        {
            get
            {
                foreach (var dbUserDefinedTableTypeDefinition in dbUserDefinedTableTypeDefinitionList)
                {
                    if (dbUserDefinedTableTypeDefinition.Name == name)
                    {
                        return dbUserDefinedTableTypeDefinition;
                    }
                }

                return null;
            }
            set
            {
                DBUserDefinedTableTypeDefinition tempDBUserDefinedTableTypeDefinition = null;
                foreach (var dbUserDefinedTableTypeDefinition in dbUserDefinedTableTypeDefinitionList)
                {
                    if (dbUserDefinedTableTypeDefinition.Name == name)
                    {
                        tempDBUserDefinedTableTypeDefinition = dbUserDefinedTableTypeDefinition;
                    }
                }
                tempDBUserDefinedTableTypeDefinition = value;
            }
        }


        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(DBUserDefinedTableTypeDefinition item)
        {
            dbUserDefinedTableTypeDefinitionList.Add(item);
        }


        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear()
        {
            dbUserDefinedTableTypeDefinitionList.Clear();
        }


        /// <summary>
        /// Determines whether [contains] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.</returns>
        public bool Contains(DBUserDefinedTableTypeDefinition item)
        {
            return dbUserDefinedTableTypeDefinitionList.Contains(item);
        }


        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(DBUserDefinedTableTypeDefinition[] array, int arrayIndex)
        {
            dbUserDefinedTableTypeDefinitionList.CopyTo(array, arrayIndex);
        }


        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return dbUserDefinedTableTypeDefinitionList.Count; }
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
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Remove(DBUserDefinedTableTypeDefinition item)
        {
            return dbUserDefinedTableTypeDefinitionList.Remove(item);
        }        
    }
}