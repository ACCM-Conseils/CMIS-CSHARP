using System.Collections;
using System.Data;
using System.Linq;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.ServiceModel
{
    public class cmisObjectInFolderContainerType : Messaging.cmisObjectInFolderContainerType, Contracts.IServiceModelObjectEnumerable, Contracts.IServiceModelObject
    {

        #region IServiceModelObjectEnumerable
        protected override IEnumerator IEnumerable_GetEnumerator()
        {
            return (_children ?? (new cmisObjectInFolderContainerType[] { })).GetEnumerator();
        }

        public bool ContainsObjects
        {
            get
            {
                return _children is not null;
            }
        }

        private bool IServiceModelObjectEnumerable_HasMoreItems
        {
            get
            {
                return false;
            }
        }

        bool Contracts.IServiceModelObjectEnumerable.HasMoreItems { get => IServiceModelObjectEnumerable_HasMoreItems; }

        private long? IServiceModelObjectEnumerable_NumItems
        {
            get
            {
                /* TODO ERROR: Skipped IfDirectiveTrivia
                #If xs_Integer = "Int32" OrElse xs_Integer = "Integer" OrElse xs_Integer = "Single" Then
                *//* TODO ERROR: Skipped DisabledTextTrivia
                            Return If(_children is Nothing, 0, _children.Length)
                *//* TODO ERROR: Skipped ElseDirectiveTrivia
                #Else
                */
                return _children is null ? 0L : _children.LongLength;
                /* TODO ERROR: Skipped EndIfDirectiveTrivia
                #End If
                */
            }
        }

        long? Contracts.IServiceModelObjectEnumerable.NumItems { get => IServiceModelObjectEnumerable_NumItems; }
        #endregion

        #region IServiceModelObject
        public cmisObjectType Object
        {
            get
            {
                return ObjectInFolder;
            }
        }

        public string PathSegment
        {
            get
            {
                var objectInFolder = ObjectInFolder;
                return objectInFolder is null ? null : objectInFolder.PathSegment;
            }
        }

        public string RelativePathSegment
        {
            get
            {
                var objectInFolder = ObjectInFolder;
                return objectInFolder is null ? null : objectInFolder.RelativePathSegment;
            }
        }

        public cmisObjectType.ServiceModelExtension ServiceModel
        {
            get
            {
                var cmisObject = Object;
                return (cmisObject is null ? null : cmisObject.ServiceModel) ?? new cmisObjectType.ServiceModelExtension(cmisObject);
            }
        }
        #endregion

        public new cmisObjectInFolderContainerType[] Children
        {
            get
            {
                if (_children is null)
                {
                    return null;
                }
                else
                {
                    return (from container in _children
                            where container is null || container is cmisObjectInFolderContainerType
                            select ((cmisObjectInFolderContainerType)container)).ToArray();
                }
            }
            set
            {
                if (value is null)
                {
                    _children = null;
                }
                else
                {
                    base.Children = (from container in value
                                     select ((Messaging.cmisObjectInFolderContainerType)container)).ToArray();
                }
            }
        }

        public new cmisObjectInFolderType ObjectInFolder
        {
            get
            {
                return _objectInFolder as cmisObjectInFolderType;
            }
            set
            {
                base.ObjectInFolder = value;
            }
        }

    }
}