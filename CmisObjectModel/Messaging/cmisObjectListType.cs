using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CmisObjectModel.Messaging
{
    public partial class cmisObjectListType : IEnumerable<Core.cmisObjectType>
    {

        #region IEnumerable
        public IEnumerator<Core.cmisObjectType> GetEnumerator()
        {
            return (from cmisObject in _objects ?? (new Core.cmisObjectType[] { })
                    select cmisObject).GetEnumerator();
        }

        protected virtual IEnumerator IEnumerable_GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => IEnumerable_GetEnumerator();
        #endregion

        /// <summary>
      /// Converts AtomFeed of objects
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static implicit operator cmisObjectListType(AtomPub.AtomFeed value)
        {
            if (value is null)
            {
                return null;
            }
            else
            {
                var objects = (from entry in value.Entries ?? new List<AtomPub.AtomEntry>()
                               let @object = entry is null ? null : entry.Object
                               where @object is not null
                               select @object).ToList();
                return new cmisObjectListType()
                {
                    _hasMoreItems = value.HasMoreItems,
                    _numItems = value.NumItems,
                    _objects = objects.Count == 0 ? null : objects.ToArray()
                };
            }
        }

    }
}