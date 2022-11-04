using System;
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
   /// see getContentChangesResponse
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Messaging.xsd
   /// </remarks>
    public partial class getContentChangesResponse : Serialization.XmlSerializable
    {

        public getContentChangesResponse()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected getContentChangesResponse(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<getContentChangesResponse, string>> _setter = new Dictionary<string, Action<getContentChangesResponse, string>>() { }; // _setter

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
            _objects = Read(reader, attributeOverrides, "objects", Constants.Namespaces.cmism, GenericXmlSerializableFactory<cmisObjectListType>);
            _changeLogToken = Read(reader, attributeOverrides, "changeLogToken", Constants.Namespaces.cmism, _changeLogToken);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteElement(writer, attributeOverrides, "objects", Constants.Namespaces.cmism, _objects);
            WriteElement(writer, attributeOverrides, "changeLogToken", Constants.Namespaces.cmism, _changeLogToken);
        }
        #endregion

        protected string _changeLogToken;
        public virtual string ChangeLogToken
        {
            get
            {
                return _changeLogToken;
            }
            set
            {
                if ((_changeLogToken ?? "") != (value ?? ""))
                {
                    string oldValue = _changeLogToken;
                    _changeLogToken = value;
                    OnPropertyChanged("ChangeLogToken", value, oldValue);
                }
            }
        } // ChangeLogToken

        protected cmisObjectListType _objects;
        public virtual cmisObjectListType Objects
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