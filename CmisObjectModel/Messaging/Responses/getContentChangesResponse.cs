using System.Collections;
using System.Collections.Generic;

namespace CmisObjectModel.Messaging.Responses
{
    public partial class getContentChangesResponse : IEnumerable<Core.cmisObjectType>
    {

        #region IEnumerable
        public IEnumerator<Core.cmisObjectType> GetEnumerator()
        {
            return (_objects ?? new cmisObjectListType()).GetEnumerator();
        }

        protected virtual IEnumerator IEnumerable_GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => IEnumerable_GetEnumerator();
        #endregion

    }
}