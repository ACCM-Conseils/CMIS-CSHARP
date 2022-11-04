using System;
using System.Collections.Generic;
using sx = System.Xml;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Core.Choices
{
    /// <summary>
   /// Implementation of choices containing double-values
   /// </summary>
   /// <remarks>In CMIS specification there is no cmisChoiceDouble defined, but some CMIS servers
   /// (i.e. alfresco) send double values instead of decimal values</remarks>
    public class cmisChoiceDouble : Generic.cmisChoice<double, cmisChoiceDouble>
    {

        public cmisChoiceDouble()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisChoiceDouble(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisChoiceDouble, string>> _setter = new Dictionary<string, Action<cmisChoiceDouble, string>>() { }; // _setter

        /// <summary>
      /// Deserialization of all properties stored in subnodes
      /// </summary>
      /// <param name="reader"></param>
      /// <remarks></remarks>
        protected override void ReadXmlCore(sx.XmlReader reader, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            base.ReadXmlCore(reader, attributeOverrides);
            _values = ReadArray<double>(reader, attributeOverrides, "value", Constants.Namespaces.cmis);
            _choices = ReadArray(reader, attributeOverrides, "choice", Constants.Namespaces.cmis, GenericXmlSerializableFactory<cmisChoiceDouble>);
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
            WriteArray(writer, attributeOverrides, "choice", Constants.Namespaces.cmis, _choices);
        }
        #endregion

    }
}