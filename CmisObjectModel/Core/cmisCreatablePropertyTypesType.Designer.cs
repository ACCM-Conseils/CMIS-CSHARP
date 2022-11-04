using System;
using System.Collections.Generic;
using sx = System.Xml;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Core
{
    /// <summary>
   /// </summary>
   /// <remarks>
   /// see cmisCreatablePropertyTypesType
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Core.xsd
   /// </remarks>
    public partial class cmisCreatablePropertyTypesType : Serialization.XmlSerializable
    {

        public cmisCreatablePropertyTypesType()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisCreatablePropertyTypesType(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisCreatablePropertyTypesType, string>> _setter = new Dictionary<string, Action<cmisCreatablePropertyTypesType, string>>() { }; // _setter

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
            _canCreates = ReadEnumArray<enumPropertyType>(reader, attributeOverrides, "canCreate", Constants.Namespaces.cmis);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteArray(writer, attributeOverrides, "canCreate", Constants.Namespaces.cmis, _canCreates);
        }
        #endregion

        protected enumPropertyType[] _canCreates;
        public virtual enumPropertyType[] CanCreates
        {
            get
            {
                return _canCreates;
            }
            set
            {
                if (!ReferenceEquals(value, _canCreates))
                {
                    var oldValue = _canCreates;
                    _canCreates = value;
                    OnPropertyChanged("CanCreates", value, oldValue);
                }
            }
        } // CanCreates

    }
}