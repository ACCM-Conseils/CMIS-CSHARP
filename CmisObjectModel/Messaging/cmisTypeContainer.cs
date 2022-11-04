using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CmisObjectModel.Messaging
{
    public partial class cmisTypeContainer
    {

        /// <summary>
      /// Converts AtomEntry
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static implicit operator cmisTypeContainer(AtomPub.AtomEntry value)
        {
            return Convert(value);
        }

        /// <summary>
      /// Converts AtomEntry
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      /// <remarks>Method implemented to avoid warning within the Operator CType(AtomEntry): recursion!</remarks>
        public static cmisTypeContainer Convert(AtomPub.AtomEntry value)
        {
            var type = value is null ? null : value.Type;

            if (type is null)
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
                return new cmisTypeContainer() { _children = children.Count == 0 ? null : children.ToArray(), _type = type };
            }
        }

    }
}