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
   /// see getDescendantsResponse
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Messaging.xsd
   /// </remarks>
    public partial class getDescendantsResponse : Serialization.XmlSerializable
    {

        public getDescendantsResponse()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected getDescendantsResponse(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<getDescendantsResponse, string>> _setter = new Dictionary<string, Action<getDescendantsResponse, string>>() { }; // _setter

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
            _objects = ReadArray(reader, attributeOverrides, "objects", Constants.Namespaces.cmism, GenericXmlSerializableFactory<cmisObjectInFolderContainerType>);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteArray(writer, attributeOverrides, "objects", Constants.Namespaces.cmism, _objects);
        }
        #endregion

        protected cmisObjectInFolderContainerType[] _objects;
        public virtual cmisObjectInFolderContainerType[] Objects
        {
            get
            {
                return _objects;
            }
            set
            {
                if (!ReferenceEquals(value, _objects))
                {
                    var oldValue = _objects;
                    _objects = value;
                    OnPropertyChanged("Objects", value, oldValue);
                }
            }
        } // Objects

    }
}