using System;
using sxs = System.Xml.Serialization;

namespace CmisObjectModel.Core.Properties
{
    [sxs.XmlRoot(DefaultElementName, Namespace = Constants.Namespaces.cmis)]
    [Attributes.CmisTypeInfo(CmisTypeName, TargetTypeName, DefaultElementName)]
    public partial class cmisPropertyUri
    {

        public cmisPropertyUri(string propertyDefinitionId, string localName, string displayName, string queryName, params string[] values) : base(propertyDefinitionId, localName, displayName, queryName, values)
        {
        }

        #region Constants
        public const string CmisTypeName = "cmis:cmisPropertyUri";
        public const string TargetTypeName = "uri";
        public const string DefaultElementName = "propertyUri";
        #endregion

        #region IComparable
        protected override int CompareTo(params string[] other)
        {
            int length = _values is null ? 0 : _values.Length;
            int otherLength = other is null ? 0 : other.Length;
            if (otherLength == 0)
            {
                return length == 0 ? 0 : 1;
            }
            else if (length == 0)
            {
                return -1;
            }
            else
            {
                for (int index = 0, loopTo = Math.Min(length, otherLength) - 1; index <= loopTo; index++)
                {
                    int result = string.Compare(_values[index], other[index]);
                    if (result != 0)
                        return result;
                }
                return length == otherLength ? 0 : length > otherLength ? 1 : -1;
            }
        }
        #endregion

        public override enumPropertyType Type
        {
            get
            {
                return enumPropertyType.uri;
            }
        }

    }
}