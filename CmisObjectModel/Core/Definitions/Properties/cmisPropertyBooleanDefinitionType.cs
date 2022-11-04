using sxs = System.Xml.Serialization;

namespace CmisObjectModel.Core.Definitions.Properties
{
    [sxs.XmlRoot(DefaultElementName, Namespace = Constants.Namespaces.cmis)]
    [Attributes.CmisTypeInfo(CmisTypeName, TargetTypeName, DefaultElementName)]
    public partial class cmisPropertyBooleanDefinitionType
    {

        public cmisPropertyBooleanDefinitionType(string id, string localName, string localNamespace, string displayName, string queryName, bool required, bool inherited, bool queryable, bool orderable, enumCardinality cardinality, enumUpdatability updatability, params Choices.cmisChoiceBoolean[] choices) : base(id, localName, localNamespace, displayName, queryName, required, inherited, queryable, orderable, cardinality, updatability, choices)
        {
        }
        public cmisPropertyBooleanDefinitionType(string id, string localName, string localNamespace, string displayName, string queryName, bool required, bool inherited, bool queryable, bool orderable, enumCardinality cardinality, enumUpdatability updatability, Core.Properties.cmisPropertyBoolean defaultValue, params Choices.cmisChoiceBoolean[] choices) : base(id, localName, localNamespace, displayName, queryName, required, inherited, queryable, orderable, cardinality, updatability, defaultValue, choices)
        {
        }

        #region Constants
        public const string CmisTypeName = "cmis:cmisPropertyBooleanDefinitionType";
        public const string TargetTypeName = Core.Properties.cmisPropertyBoolean.CmisTypeName;
        public const string DefaultElementName = "propertyBooleanDefinition";
        #endregion

        protected override enumPropertyType _propertyType
        {
            get
            {
                return enumPropertyType.boolean;
            }
        }

    }
}