using sxs = System.Xml.Serialization;

namespace CmisObjectModel.Core.Definitions.Properties
{
    [sxs.XmlRoot(DefaultElementName, Namespace = Constants.Namespaces.cmis)]
    [Attributes.CmisTypeInfo(CmisTypeName, TargetTypeName, DefaultElementName)]
    public partial class cmisPropertyDateTimeDefinitionType
    {

        public cmisPropertyDateTimeDefinitionType(string id, string localName, string localNamespace, string displayName, string queryName, bool required, bool inherited, bool queryable, bool orderable, enumCardinality cardinality, enumUpdatability updatability, params Choices.cmisChoiceDateTime[] choices) : base(id, localName, localNamespace, displayName, queryName, required, inherited, queryable, orderable, cardinality, updatability, choices)
        {
        }
        public cmisPropertyDateTimeDefinitionType(string id, string localName, string localNamespace, string displayName, string queryName, bool required, bool inherited, bool queryable, bool orderable, enumCardinality cardinality, enumUpdatability updatability, Core.Properties.cmisPropertyDateTime defaultValue, params Choices.cmisChoiceDateTime[] choices) : base(id, localName, localNamespace, displayName, queryName, required, inherited, queryable, orderable, cardinality, updatability, defaultValue, choices)
        {
        }

        #region Constants
        public const string CmisTypeName = "cmis:cmisPropertyDateTimeDefinitionType";
        public const string TargetTypeName = Core.Properties.cmisPropertyDateTime.CmisTypeName;
        public const string DefaultElementName = "propertyDateTimeDefinition";
        #endregion

        protected override enumPropertyType _propertyType
        {
            get
            {
                return enumPropertyType.datetime;
            }
        }

    }
}