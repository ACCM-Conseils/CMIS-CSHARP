using sxs = System.Xml.Serialization;

namespace CmisObjectModel.Core.Definitions.Properties
{
    [sxs.XmlRoot(DefaultElementName, Namespace = Constants.Namespaces.cmis)]
    [Attributes.CmisTypeInfo(CmisTypeName, TargetTypeName, DefaultElementName)]
    public partial class cmisPropertyUriDefinitionType
    {

        public cmisPropertyUriDefinitionType(string id, string localName, string localNamespace, string displayName, string queryName, bool required, bool inherited, bool queryable, bool orderable, enumCardinality cardinality, enumUpdatability updatability, params Choices.cmisChoiceUri[] choices) : base(id, localName, localNamespace, displayName, queryName, required, inherited, queryable, orderable, cardinality, updatability, choices)
        {
        }
        public cmisPropertyUriDefinitionType(string id, string localName, string localNamespace, string displayName, string queryName, bool required, bool inherited, bool queryable, bool orderable, enumCardinality cardinality, enumUpdatability updatability, Core.Properties.cmisPropertyUri defaultValue, params Choices.cmisChoiceUri[] choices) : base(id, localName, localNamespace, displayName, queryName, required, inherited, queryable, orderable, cardinality, updatability, defaultValue, choices)
        {
        }

        #region Constants
        public const string CmisTypeName = "cmis:cmisPropertyUriDefinitionType";
        public const string TargetTypeName = Core.Properties.cmisPropertyUri.CmisTypeName;
        public const string DefaultElementName = "propertyUriDefinition";
        #endregion

        protected override enumPropertyType _propertyType
        {
            get
            {
                return enumPropertyType.uri;
            }
        }

    }
}