using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CmisObjectModel.Messaging
{
    public partial class cmisObjectInFolderContainerType : IEnumerable<Core.cmisObjectType>
    {

        #region IEnumerable
        public IEnumerator<Core.cmisObjectType> GetEnumerator()
        {
            var stack = new Stack<cmisObjectInFolderContainerType>();
            var objects = new List<Core.cmisObjectType>();
            var verify = new HashSet<cmisObjectInFolderContainerType>();

            // collect cmisObjects
            stack.Push(this);
            verify.Add(this);
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                var cmisObject = current.ObjectInFolder is null ? null : current.ObjectInFolder.Object;

                if (cmisObject is not null)
                    objects.Add(cmisObject);
                if (current.Children is not null)
                {
                    foreach (cmisObjectInFolderContainerType child in current.Children)
                    {
                        if (child is not null && verify.Add(child))
                            stack.Push(child);
                    }
                }
            }

            return objects.GetEnumerator();
        }

        protected virtual IEnumerator IEnumerable_GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => IEnumerable_GetEnumerator();
        #endregion

        /// <summary>
      /// Converts AtomEntry
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static implicit operator cmisObjectInFolderContainerType(AtomPub.AtomEntry value)
        {
            return Convert(value);
        }

        /// <summary>
      /// Converts AtomEntry
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      /// <remarks>Method implemented to avoid warning within the Operator CType(AtomEntry): recursion!</remarks>
        public static cmisObjectInFolderContainerType Convert(AtomPub.AtomEntry value)
        {
            cmisObjectInFolderType objectInFolder = value;

            if (objectInFolder is null)
            {
                return null;
            }
            else
            {
                var childrenFeed = value.Children;
                var children = (from entry in (childrenFeed is null ? null : childrenFeed.Entries) ?? new List<AtomPub.AtomEntry>()
                                let child = Convert(entry)
                                where child is not null
                                select child).ToList();
                return new cmisObjectInFolderContainerType() { _children = children.Count == 0 ? null : children.ToArray(), _objectInFolder = objectInFolder };
            }
        }

    }
}