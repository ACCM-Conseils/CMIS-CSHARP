using sxs = System.Xml.Serialization;

namespace CmisObjectModel.Core.Definitions.Properties
{
    [sxs.XmlRoot(DefaultElementName, Namespace = Constants.Namespaces.cmis)]
    [Attributes.CmisTypeInfo(CmisTypeName, TargetTypeName, DefaultElementName)]
    public partial class cmisPropertyStringDefinitionType
    {

        public cmisPropertyStringDefinitionType(string id, string localName, string localNamespace, string displayName, string queryName, bool required, bool inherited, bool queryable, bool orderable, enumCardinality cardinality, enumUpdatability updatability, params Choices.cmisChoiceString[] choices) : base(id, localName, localNamespace, displayName, queryName, required, inherited, queryable, orderable, cardinality, updatability, choices)
        {
        }
        public cmisPropertyStringDefinitionType(string id, string localName, string localNamespace, string displayName, string queryName, bool required, bool inherited, bool queryable, bool orderable, enumCardinality cardinality, enumUpdatability updatability, Core.Properties.cmisPropertyString defaultValue, params Choices.cmisChoiceString[] choices) : base(id, localName, localNamespace, displayName, queryName, required, inherited, queryable, orderable, cardinality, updatability, defaultValue, choices)
        {
        }

        #region Constants
        public const string CmisTypeName = "cmis:cmisPropertyStringDefinitionType";
        public const string TargetTypeName = Core.Properties.cmisPropertyString.CmisTypeName;
        public const string DefaultElementName = "propertyStringDefinition";
        #endregion

        protected override enumPropertyType _propertyType
        {
            get
            {
                return enumPropertyType.@string;
            }
        }

    }
}