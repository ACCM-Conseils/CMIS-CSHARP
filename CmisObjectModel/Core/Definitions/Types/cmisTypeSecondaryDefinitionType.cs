using System.Collections.Generic;

namespace CmisObjectModel.Core.Definitions.Types
{
    [Attributes.CmisTypeInfo(CMISTypeName, TargetTypeName, DefaultElementName)]
    public partial class cmisTypeSecondaryDefinitionType
    {

        public cmisTypeSecondaryDefinitionType(string id, string localName, string displayName, string queryName, params Properties.cmisPropertyDefinitionType[] propertyDefinitions) : base(id, localName, displayName, queryName, propertyDefinitions)
        {
        }
        public cmisTypeSecondaryDefinitionType(string id, string localName, string displayName, string queryName, string parentId, params Properties.cmisPropertyDefinitionType[] propertyDefinitions) : base(id, localName, displayName, queryName, parentId, propertyDefinitions)
        {
        }

        #region Constants
        public new const string CMISTypeName = "cmis:cmisTypeSecondaryDefinitionType";
        public const string TargetTypeName = "cmis:secondary";
        public const string DefaultElementName = "typeSecondaryDefinition";
        #endregion

        protected override enumBaseObjectTypeIds _baseId
        {
            get
            {
                return enumBaseObjectTypeIds.cmisSecondary;
            }
        }

        /// <summary>
      /// Returns the defaultProperties of a TypeSecondaryDefinition-instance
      /// </summary>
      /// <param name="localNamespace"></param>
      /// <param name="isBaseType"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static List<Properties.cmisPropertyDefinitionType> GetDefaultProperties(string localNamespace, bool isBaseType)
        {
            return new List<Properties.cmisPropertyDefinitionType>();
        }

        protected override string GetCmisTypeName()
        {
            return CMISTypeName;
        }

        protected override void InitClass()
        {
            MyBaseInitClass();
            _controllableACL = false;
            _controllablePolicy = false;
            _creatable = false;
            _fileable = false;
            _id = TargetTypeName;
            _queryName = TargetTypeName;
        }

    }
}