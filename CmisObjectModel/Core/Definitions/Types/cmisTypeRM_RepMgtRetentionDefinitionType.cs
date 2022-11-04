using System.Collections.Generic;

namespace CmisObjectModel.Core.Definitions.Types
{
    [Attributes.CmisTypeInfo(CMISTypeName, TargetTypeName, DefaultElementName)]
    public partial class cmisTypeRM_RepMgtRetentionDefinitionType
    {

        public cmisTypeRM_RepMgtRetentionDefinitionType(string id, string localName, string displayName, string queryName, params Properties.cmisPropertyDefinitionType[] propertyDefinitions) : base(id, localName, displayName, queryName, cmisTypeSecondaryDefinitionType.TargetTypeName, propertyDefinitions)
        {
        }
        public cmisTypeRM_RepMgtRetentionDefinitionType(string id, string localName, string displayName, string queryName, string parentId, params Properties.cmisPropertyDefinitionType[] propertyDefinitions) : base(id, localName, displayName, queryName, parentId, propertyDefinitions)
        {
        }

        #region Constants
        public new const string CMISTypeName = "cmis:cmisTypeRM_RepMgtRetentionDefinitionType";
        public new const string TargetTypeName = "cmis:rm_repMgtRetention";
        public new const string DefaultElementName = "typeRM_RepMgtRetentionDefinition";
        #endregion

        /// <summary>
      /// Returns the defaultProperties of a TypeRM_HoldDefinition-instance
      /// </summary>
        public static new List<Properties.cmisPropertyDefinitionType> GetDefaultProperties(string localNamespace, bool isBaseType)
        {
            return cmisTypeSecondaryDefinitionType.GetDefaultProperties(localNamespace, false);
        }

        protected override string GetCmisTypeName()
        {
            return CMISTypeName;
        }

        protected override void InitClass()
        {
            base.InitClass();
            _parentId = cmisTypeSecondaryDefinitionType.TargetTypeName;
            _queryable = true;
        }

    }
}