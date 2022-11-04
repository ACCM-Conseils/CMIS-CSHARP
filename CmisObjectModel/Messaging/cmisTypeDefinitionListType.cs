using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CmisObjectModel.Messaging
{
    public partial class cmisTypeDefinitionListType
    {

        /// <summary>
      /// Converts AtomFeed of typedefinitions
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static implicit operator cmisTypeDefinitionListType(AtomPub.AtomFeed value)
        {
            if (value is null)
            {
                return null;
            }
            else
            {
                var types = (from entry in value.Entries ?? new List<AtomPub.AtomEntry>()
                             let type = entry is null ? null : entry.Type
                             where type is not null
                             select type).ToList();
                return new cmisTypeDefinitionListType()
                {
                    _hasMoreItems = value.HasMoreItems,
                    _numItems = value.NumItems,
                    _types = types.Count == 0 ? null : types.ToArray()
                };
            }
        }

    }
}