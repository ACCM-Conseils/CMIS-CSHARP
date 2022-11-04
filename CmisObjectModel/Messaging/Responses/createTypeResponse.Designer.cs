﻿using System;
using System.Collections.Generic;
using sx = System.Xml;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Messaging.Responses
{
    /// <summary>
   /// </summary>
   /// <remarks>
   /// see createTypeResponse
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Messaging.xsd
   /// </remarks>
    public partial class createTypeResponse : Serialization.XmlSerializable
    {

        public createTypeResponse()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected createTypeResponse(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<createTypeResponse, string>> _setter = new Dictionary<string, Action<createTypeResponse, string>>() { }; // _setter

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
            _type = Read(reader, attributeOverrides, null, Core.Definitions.Types.cmisTypeDefinitionType.CreateInstance);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteElement(writer, attributeOverrides, null, Constants.Namespaces.cmism, _type);
        }
        #endregion

        protected Core.Definitions.Types.cmisTypeDefinitionType _type;
        public virtual Core.Definitions.Types.cmisTypeDefinitionType Type
        {
            get
            {
                return _type;
            }
            set
            {
                if (!ReferenceEquals(value, _type))
                {
                    var oldValue = _type;
                    _type = value;
                    OnPropertyChanged("Type", value, oldValue);
                }
            }
        } // Type

    }
}