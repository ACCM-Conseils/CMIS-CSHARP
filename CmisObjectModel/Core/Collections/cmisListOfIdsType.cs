using sxs = System.Xml.Serialization;

namespace CmisObjectModel.Core.Collections
{
    [sxs.XmlRoot("ids", Namespace = Constants.Namespaces.cmis)]
    public partial class cmisListOfIdsType
    {

        public cmisListOfIdsType(params string[] ids)
        {
            _ids = ids;
        }

        public static implicit operator cmisListOfIdsType(string[] value)
        {
            return value is null || value.Length == 0 ? null : new cmisListOfIdsType(value);
        }

        public static implicit operator string[](cmisListOfIdsType value)
        {
            return value is null ? null : value.Ids;
        }

    }
}