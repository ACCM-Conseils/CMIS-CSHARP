using System;
using ccg = CmisObjectModel.Collections.Generic;
using ccdp = CmisObjectModel.Core.Definitions.Properties;
using ccdt = CmisObjectModel.Core.Definitions.Types;
using cmr = CmisObjectModel.Messaging.Requests;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Client
{
    /// <summary>
   /// Simplifies requests to a cmis TypeDefinition
   /// </summary>
   /// <remarks></remarks>
    public class CmisType : CmisDataModelObject
    {

        #region Constructors
        private CmisType(ccdt.cmisTypeDefinitionType type, Contracts.ICmisClient client, Core.cmisRepositoryInfoType repositoryInfo) : base(client, repositoryInfo)
        {
            _type = type;
        }
        #endregion

        #region Helper classes
        /// <summary>
      /// Creates the CmisType-Instance
      /// </summary>
      /// <remarks></remarks>
        public class PreStage
        {
            public PreStage(Contracts.ICmisClient client, ccdt.cmisTypeDefinitionType type)
            {
                _client = client;
                _type = type;
            }

            private Contracts.ICmisClient _client;
            private ccdt.cmisTypeDefinitionType _type;

            public static CmisType operator +(PreStage arg1, Core.cmisRepositoryInfoType arg2)
            {
                return new CmisType(arg1._type, arg1._client, arg2);
            }
        }
        #endregion

        #region Repository
        /// <summary>
      /// Deletes the current type from the repository
      /// </summary>
      /// <remarks></remarks>
        public void Delete()
        {
            _lastException = _client.DeleteType(new cmr.deleteType() { RepositoryId = _repositoryInfo.RepositoryId, TypeId = _type.Id }).Exception;
        }

        /// <summary>
      /// Returns the list of object-types defined for the repository that are children of the current type
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public Generic.ItemList<CmisType> GetChildren(bool includePropertyDefinitions = false, long? maxItems = default, long? skipCount = default)
        {
            return GetTypeChildren(_type.Id, includePropertyDefinitions, maxItems, skipCount);
        }

        /// <summary>
      /// Returns the set of the descendant object-types defined for the Repository under the current type
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public Generic.ItemContainer<CmisType>[] GetDescendants(long? depth = default, bool includePropertyDefinitions = false)
        {
            return GetTypeDescendants(_type.Id, depth, includePropertyDefinitions);
        }

        /// <summary>
      /// Updates the current type definition and returns True in case of success
      /// </summary>
      /// <param name="type"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public bool Update(ccdt.cmisTypeDefinitionType type)
        {
            {
                var withBlock = _client.UpdateType(new cmr.updateType() { RepositoryId = _repositoryInfo.RepositoryId, Type = type });
                _lastException = withBlock.Exception;
                if (_lastException is null)
                {
                    type = withBlock.Response.Type;
                    if (type is not null)
                    {
                        _type = type;
                        return true;
                    }
                }

                return false;
            }
        }
        #endregion

        #region Browser Binding support
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("No succinct representation defined for CmisType", true)]
        public override void BeginSuccinct(bool succinct)
        {
        }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("No succinct representation defined for CmisType", true)]
        public override bool CurrentSuccinct
        {
            get
            {
                return false;
            }
        }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("No succinct representation defined for CmisType", true)]
        public override bool EndSuccinct()
        {
            return false;
        }
        #endregion

        /// <summary>
      /// Access to PropertyDefinitions via index or Id
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccg.ArrayMapper<ccdt.cmisTypeDefinitionType, ccdp.cmisPropertyDefinitionType> PropertyDefinitionsAsReadOnly
        {
            get
            {
                return _type.PropertyDefinitionsAsReadOnly;
            }
        }

        protected ccdt.cmisTypeDefinitionType _type;
        public ccdt.cmisTypeDefinitionType Type
        {
            get
            {
                return _type;
            }
        }

    }
}