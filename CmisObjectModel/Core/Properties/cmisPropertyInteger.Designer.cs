using System;
using System.Collections.Generic;
// depends on the chosen interpretation of the xs:integer expression in a xsd-file
/* TODO ERROR: Skipped IfDirectiveTrivia
#If xs_Integer = "Int32" OrElse xs_integer = "Integer" OrElse xs_integer = "Single" Then
*//* TODO ERROR: Skipped DisabledTextTrivia
Imports xs_Integer = System.Int32
*//* TODO ERROR: Skipped ElseDirectiveTrivia
#Else
*/
using xs_Integer = System.Int64;
using sx = System.Xml;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Core.Properties
{
    /// <summary>
   /// </summary>
   /// <remarks>
   /// see cmisPropertyInteger
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Core.xsd
   /// </remarks>
    public partial class cmisPropertyInteger : Generic.cmisProperty<long>
    {

        public cmisPropertyInteger()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisPropertyInteger(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisPropertyInteger, string>> _setter = new Dictionary<string, Action<cmisPropertyInteger, string>>() { }; // _setter

        /// <summary>
      /// Deserialization of all properties stored in subnodes
      /// </summary>
      /// <param name="reader"></param>
      /// <remarks></remarks>
        protected override void ReadXmlCore(sx.XmlReader reader, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            base.ReadXmlCore(reader, attributeOverrides);
            _values = ReadArray<xs_Integer>(reader, attributeOverrides, "value", Constants.Namespaces.cmis);
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

    }
}