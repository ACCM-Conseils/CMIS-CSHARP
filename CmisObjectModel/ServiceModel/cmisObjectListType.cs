using System.Collections;
using System.Data;
using System.Linq;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.ServiceModel
{
    public class cmisObjectListType : Messaging.cmisObjectListType, Contracts.IServiceModelObjectEnumerable
    {

        #region IServiceModelObjectEnumerable
        protected override IEnumerator IEnumerable_GetEnumerator()
        {
            return (_objects ?? (new cmisObjectType[] { })).GetEnumerator();
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

        public new cmisObjectType[] Objects
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
                            where cmisObject is null || cmisObject is cmisObjectType
                            select ((cmisObjectType)cmisObject)).ToArray();
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
                    base.Objects = (from cmisObject in value
                                    select ((Core.cmisObjectType)cmisObject)).ToArray();
                }
            }
        }

    }
}