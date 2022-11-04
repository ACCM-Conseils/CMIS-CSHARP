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
   /// see cmisChoice
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Core.xsd
   /// </remarks>
    public abstract partial class cmisChoice : Serialization.XmlSerializable
    {

        protected cmisChoice()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisChoice(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisChoice, string>> _setter = new Dictionary<string, Action<cmisChoice, string>>() { { "displayname", SetDisplayName } }; // _setter

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
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteAttribute(writer, attributeOverrides, "displayName", null, _displayName);
        }
        #endregion

        protected string _displayName;
        public virtual string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                if ((_displayName ?? "") != (value ?? ""))
                {
                    string oldValue = _displayName;
                    _displayName = value;
                    OnPropertyChanged("DisplayName", value, oldValue);
                }
            }
        } // DisplayName
        private static void SetDisplayName(cmisChoice instance, string value)
        {
            instance.DisplayName = value;
        }

    }
}