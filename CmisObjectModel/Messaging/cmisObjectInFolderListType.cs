using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CmisObjectModel.Messaging
{
    public partial class cmisObjectInFolderListType : IEnumerable<Core.cmisObjectType>
    {

        #region IEnumerable
        public IEnumerator<Core.cmisObjectType> GetEnumerator()
        {
            return (from cmisObjectObjectInFolder in _objects ?? (new cmisObjectInFolderType[] { })
                    let cmisObject = cmisObjectObjectInFolder is null ? null : cmisObjectObjectInFolder.Object
                    where cmisObject is not null
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
        public static implicit operator cmisObjectInFolderListType(AtomPub.AtomFeed value)
        {
            if (value is null)
            {
                return null;
            }
            else
            {
                var objects = (from entry in value.Entries ?? new List<AtomPub.AtomEntry>()
                               let @object = entry
                               where @object is not null
                               select @object).ToList();
                return new cmisObjectInFolderListType()
                {
                    _hasMoreItems = value.HasMoreItems,
                    _numItems = value.NumItems,
                    _objects = objects.Count == 0 ? null : objects.Cast<cmisObjectInFolderType>().ToArray()
                };
            }
        }

    }
}