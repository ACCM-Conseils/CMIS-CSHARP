using System;
using System.Collections.Generic;
using sx = System.Xml;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Core.Definitions.Properties
{
    /// <summary>
   /// </summary>
   /// <remarks>
   /// see cmisPropertyBooleanDefinitionType
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Core.xsd
   /// </remarks>
    public partial class cmisPropertyBooleanDefinitionType : Generic.cmisPropertyDefinitionType<bool, Choices.cmisChoiceBoolean, Core.Properties.cmisPropertyBoolean>
    {

        public cmisPropertyBooleanDefinitionType()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisPropertyBooleanDefinitionType(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisPropertyBooleanDefinitionType, string>> _setter = new Dictionary<string, Action<cmisPropertyBooleanDefinitionType, string>>() { }; // _setter

        /// <summary>
      /// Deserialization of all properties stored in attributes
      /// </summary>
      /// <param name="reader"></param>
      /// <remarks></remarks>
        protected override void ReadAttributes(sx.XmlReader reader)
        {
            // at least one property is serialized in an attribute-value
            if (_setter.Count > 0)
            {
                for (int attributeIndex = 0, loopTo = reader.AttributeCount - 1; attributeIndex <= loopTo; attributeIndex++)
                {
                    reader.MoveToAttribute(attributeIndex);
                    // attribute name
                    string key = reader.Name.ToLowerInvariant();
                    if (_setter.ContainsKey(key))
                        _setter[key].Invoke(this, reader.GetAttribute(attributeIndex));
                }
            }
        }

        /// <summary>
      /// Deserialization of all properties stored in subnodes
      /// </summary>
      /// <param name="reader"></param>
      /// <remarks></remarks>
        protected override void ReadXmlCore(sx.XmlReader reader, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            base.ReadXmlCore(reader, attributeOverrides);
            _defaultValue = Read(reader, attributeOverrides, "defaultValue", Constants.Namespaces.cmis, GenericXmlSerializableFactory<Core.Properties.cmisPropertyBoolean>);
            _choices = ReadArray(reader, attributeOverrides, "choice", Constants.Namespaces.cmis, GenericXmlSerializableFactory<Choices.cmisChoiceBoolean>);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            base.WriteXmlCore(writer, attributeOverrides);
            WriteElement(writer, attributeOverrides, "defaultValue", Constants.Namespaces.cmis, _defaultValue);
            WriteArray(writer, attributeOverrides, "choice", Constants.Namespaces.cmis, _choices);
        }
        #endregion

    }
}