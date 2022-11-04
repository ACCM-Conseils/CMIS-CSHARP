using System;
using System.Collections.Generic;
using sx = System.Xml;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Core.Choices
{
    /// <summary>
   /// </summary>
   /// <remarks>
   /// see cmisChoiceDateTime
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Core.xsd
   /// </remarks>
    public partial class cmisChoiceDateTime : Generic.cmisChoice<DateTimeOffset, cmisChoiceDateTime>
    {

        public cmisChoiceDateTime()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisChoiceDateTime(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisChoiceDateTime, string>> _setter = new Dictionary<string, Action<cmisChoiceDateTime, string>>() { }; // _setter

        /// <summary>
      /// Deserialization of all properties stored in subnodes
      /// </summary>
      /// <param name="reader"></param>
      /// <remarks></remarks>
        protected override void ReadXmlCore(sx.XmlReader reader, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            base.ReadXmlCore(reader, attributeOverrides);
            _values = ReadArray<DateTimeOffset>(reader, attributeOverrides, "value", Constants.Namespaces.cmis);
            _choices = ReadArray(reader, attributeOverrides, "choice", Constants.Namespaces.cmis, GenericXmlSerializableFactory<cmisChoiceDateTime>);
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