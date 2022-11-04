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
   /// Implementation of properties containing object-values
   /// </summary>
   /// <remarks>In CMIS specification there is no cmisPropertyObject defined, but in the browser binding
   /// the properties of a cmisObjectType may be requested as succinctProperties. In this case it is
   /// impossible to detect the correct cmisPropertyType, if the sent value for the property is null.</remarks>
    [sxs.XmlRoot(cmisPropertyString.DefaultElementName, Namespace = Constants.Namespaces.cmis)]
    [cac(CmisTypeName, TargetTypeName, DefaultElementName)]
    public class cmisPropertyObject : Generic.cmisProperty<object>
    {

        public cmisPropertyObject()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisPropertyObject(bool? initClassSupported) : base(initClassSupported)
        {
        }
        public cmisPropertyObject(string propertyDefinitionId, string localName, string displayName, string queryName, params object[] values) : base(propertyDefinitionId, localName, displayName, queryName, values)
        {
        }

        #region Constants
        public const string CmisTypeName = "cmis:cmisPropertyObject";
        public const string TargetTypeName = "object";
        public const string DefaultElementName = "propertyObject";
        #endregion

        #region IComparable
        protected override int CompareTo(params object[] other)
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
                    var first = _values[index];
                    var second = other[index];
                    if (!ReferenceEquals(first, second))
                    {
                        if (first is null)
                        {
                            return -1;
                        }
                        else if (second is null)
                        {
                            return 1;
                        }
                        else if (first is IComparable)
                        {
                            try
                            {
                                int retVal = ((IComparable)first).CompareTo(second);
                                if (retVal != 0)
                                    return retVal;
                            }
                            catch (Exception ex)
                            {
                                // not to compare
                                return 1;
                            }
                        }
                        else if (second is IComparable)
                        {
                            try
                            {
                                int retVal = ((IComparable)second).CompareTo(first);
                                if (retVal != 0)
                                    return -retVal;
                            }
                            catch (Exception ex)
                            {
                                // not to compare
                                return 1;
                            }
                        }
                        else
                        {
                            // not to compare
                            return 1;
                        }
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
            _values = ReadArray<object>(reader, attributeOverrides, "value", Constants.Namespaces.cmis);
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
                // not specified
                return (enumPropertyType)(-1);
            }
        }

    }
}