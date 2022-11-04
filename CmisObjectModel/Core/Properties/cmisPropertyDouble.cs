using System;
using System.Collections.Generic;
using sx = System.Xml;
using sxs = System.Xml.Serialization;
using cac = CmisObjectModel.Attributes.CmisTypeInfoAttribute;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Core.Properties
{
    /// <summary>
   /// Implementation of properties containing double-values
   /// </summary>
   /// <remarks>In CMIS specification there is no cmisPropertyDouble defined, but some CMIS servers
   /// (i.e. alfresco) send double values instead of decimal values</remarks>
    [sxs.XmlRoot(cmisPropertyDecimal.DefaultElementName, Namespace = Constants.Namespaces.cmis)]
    [cac(CmisTypeName, TargetTypeName, DefaultElementName)]
    public class cmisPropertyDouble : Generic.cmisProperty<double>
    {

        public cmisPropertyDouble()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisPropertyDouble(bool? initClassSupported) : base(initClassSupported)
        {
        }
        public cmisPropertyDouble(string propertyDefinitionId, string localName, string displayName, string queryName, params double[] values) : base(propertyDefinitionId, localName, displayName, queryName, values)
        {
        }

        #region Constants
        public const string CmisTypeName = "cmis:cmisPropertyDouble";
        public const string TargetTypeName = "double";
        public const string DefaultElementName = "propertyDouble";
        #endregion

        #region IComparable
        protected override int CompareTo(params double[] other)
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
                    double first = _values[index];
                    double second = other[index];
                    if (first < second)
                    {
                        return -1;
                    }
                    else if (first > second)
                    {
                        return 1;
                    }
                }
                return length == otherLength ? 0 : length > otherLength ? 1 : -1;
            }
        }
        #endregion

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisPropertyDecimal, string>> _setter = new Dictionary<string, Action<cmisPropertyDecimal, string>>() { }; // _setter

        /// <summary>
      /// Deserialization of all properties stored in subnodes
      /// </summary>
      /// <param name="reader"></param>
      /// <remarks></remarks>
        protected override void ReadXmlCore(sx.XmlReader reader, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            base.ReadXmlCore(reader, attributeOverrides);
            _values = ReadArray<double>(reader, attributeOverrides, "value", Constants.Namespaces.cmis);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            base.WriteXmlCore(writer, attributeOverrides);
            WriteArray(writer, attributeOverrides, "value", Constants.Namespaces.cmis, _values);
        }
        #endregion

        public override enumPropertyType Type
        {
            get
            {
                return enumPropertyType.@decimal;
            }
        }

    }
}