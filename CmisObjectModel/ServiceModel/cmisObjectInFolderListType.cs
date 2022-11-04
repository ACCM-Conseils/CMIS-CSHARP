using System.Collections;
using System.Data;
using System.Linq;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.ServiceModel
{
    public class cmisObjectInFolderListType : Messaging.cmisObjectInFolderListType, Contracts.IServiceModelObjectEnumerable
    {

        #region IServiceModelObjectEnumerable
        protected override IEnumerator IEnumerable_GetEnumerator()
        {
            return (_objects ?? (new cmisObjectInFolderType[] { })).GetEnumerator();
        }

        public bool ContainsObjects
        {
            get
            {
                return _objects is not null;
            }
        }

        private bool IServiceModelObjectEnumerable_HasMoreItems
        {
            get
            {
                return _hasMoreItems;
            }
        }

        bool Contracts.IServiceModelObjectEnumerable.HasMoreItems { get => IServiceModelObjectEnumerable_HasMoreItems; }

        private long? IServiceModelObjectEnumerable_NumItems
        {
            get
            {
                return _numItems;
            }
        }

        long? Contracts.IServiceModelObjectEnumerable.NumItems { get => IServiceModelObjectEnumerable_NumItems; }
        #endregion

        public new cmisObjectInFolderType[] Objects
        {
            get
            {
                if (_objects is null)
                {
                    return null;
                }
                else
                {
                    return (from cmisObject in _objects
                            where cmisObject is null || cmisObject is cmisObjectInFolderType
                            select ((cmisObjectInFolderType)cmisObject)).ToArray();
                }
            }
            set
            {
                if (value is null)
                {
                    _objects = null;
                }
                else
                {
                    base.Objects = (from item in value
                                    select ((Messaging.cmisObjectInFolderType)item)).ToArray();
                }
            }
        }

        public static implicit operator cmisObjectListType(cmisObjectInFolderListType value)
        {
            if (value is null)
            {
                return null;
            }
            else
            {
                return new cmisObjectListType()
                {
                    HasMoreItems = value._hasMoreItems,
                    NumItems = value._numItems,
                    Objects = value._objects is null ? null : (from cmisObjectInFolder in value._objects
                                                               let cmisObject = cmisObjectInFolder is null || !(cmisObjectInFolder is cmisObjectInFolderType) ? null : ((cmisObjectInFolderType)cmisObjectInFolder).Object
                                                               where cmisObject is not null || cmisObjectInFolder is null
                                                               select cmisObject).ToArray()
                };
            }
        }
    }
}