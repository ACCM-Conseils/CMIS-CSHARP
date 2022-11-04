using System.Collections;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.ServiceModel
{
    public class getContentChanges : Messaging.Responses.getContentChangesResponse, Contracts.IServiceModelObjectEnumerable
    {

        #region IServiceModelObjectEnumerable
        protected override IEnumerator IEnumerable_GetEnumerator()
        {
            return (Objects ?? new cmisObjectListType()).GetEnumerator();
        }

        public bool ContainsObjects
        {
            get
            {
                var objects = Objects;
                return objects is not null && objects.ContainsObjects;
            }
        }

        public bool HasMoreItems
        {
            get
            {
                var objects = Objects;
                return objects is not null && objects.HasMoreItems;
            }
        }

        public long? NumItems
        {
            get
            {
                var objects = Objects;
                if (objects is null)
                {
                    return default;
                }
                else
                {
                    return objects.NumItems;
                }
            }
        }
        #endregion

        public new cmisObjectListType Objects
        {
            get
            {
                return _objects as cmisObjectListType;
            }
            set
            {
                base.Objects = value;
            }
        }
    }
}