using sxs = System.Xml.Serialization;

namespace CmisObjectModel.Core.Definitions.Properties
{
    [sxs.XmlRoot(DefaultElementName, Namespace = Constants.Namespaces.cmis)]
    [Attributes.CmisTypeInfo(CmisTypeName, TargetTypeName, DefaultElementName)]
    public partial class cmisPropertyDecimalDefinitionType
    {

        public cmisPropertyDecimalDefinitionType(string id, string localName, string localNamespace, string displayName, string queryName, bool required, bool inherited, bool queryable, bool orderable, enumCardinality cardinality, enumUpdatability updatability, params Choices.cmisChoiceDecimal[] choices) : base(id, localName, localNamespace, displayName, queryName, required, inherited, queryable, orderable, cardinality, updatability, choices)
        {
        }
        public cmisPropertyDecimalDefinitionType(string id, string localName, string localNamespace, string displayName, string queryName, bool required, bool inherited, bool queryable, bool orderable, enumCardinality cardinality, enumUpdatability updatability, Core.Properties.cmisPropertyDecimal defaultValue, params Choices.cmisChoiceDecimal[] choices) : base(id, localName, localNamespace, displayName, queryName, required, inherited, queryable, orderable, cardinality, updatability, defaultValue, choices)
        {
        }

        #region Constants
        public const string CmisTypeName = "cmis:cmisPropertyDecimalDefinitionType";
        public const string TargetTypeName = Core.Properties.cmisPropertyDecimal.CmisTypeName;
        public const string DefaultElementName = "propertyDecimalDefinition";
        #endregion

        protected override enumPropertyType _propertyType
        {
            get
            {
                return enumPropertyType.@decimal;
            }
        }

    }
}