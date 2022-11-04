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
   /// see cmisObjectIdAndChangeTokenType
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Core.xsd
   /// </remarks>
    public partial class cmisObjectIdAndChangeTokenType : Serialization.XmlSerializable
    {

        public cmisObjectIdAndChangeTokenType()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisObjectIdAndChangeTokenType(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisObjectIdAndChangeTokenType, string>> _setter = new Dictionary<string, Action<cmisObjectIdAndChangeTokenType, string>>() { }; // _setter

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
            _id = Read(reader, attributeOverrides, "id", Constants.Namespaces.cmis, _id);
            _newId = Read(reader, attributeOverrides, "newId", Constants.Namespaces.cmis, _newId);
            _changeToken = Read(reader, attributeOverrides, "changeToken", Constants.Namespaces.cmis, _changeToken);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteElement(writer, attributeOverrides, "id", Constants.Namespaces.cmis, _id);
            if (!string.IsNullOrEmpty(_newId))
                WriteElement(writer, attributeOverrides, "newId", Constants.Namespaces.cmis, _newId);
            if (!string.IsNullOrEmpty(_changeToken))
                WriteElement(writer, attributeOverrides, "changeToken", Constants.Namespaces.cmis, _changeToken);
        }
        #endregion

        protected string _changeToken;
        public virtual string ChangeToken
        {
            get
            {
                return _changeToken;
            }
            set
            {
                if ((_changeToken ?? "") != (value ?? ""))
                {
                    string oldValue = _changeToken;
                    _changeToken = value;
                    OnPropertyChanged("ChangeToken", value, oldValue);
                }
            }
        } // ChangeToken

        protected string _id;
        public virtual string Id
        {
            get
            {
                return _id;
            }
            set
            {
                if ((_id ?? "") != (value ?? ""))
                {
                    string oldValue = _id;
                    _id = value;
                    OnPropertyChanged("Id", value, oldValue);
                }
            }
        } // Id

        protected string _newId;
        public virtual string NewId
        {
            get
            {
                return _newId;
            }
            set
            {
                if ((_newId ?? "") != (value ?? ""))
                {
                    string oldValue = _newId;
                    _newId = value;
                    OnPropertyChanged("NewId", value, oldValue);
                }
            }
        } // NewId

    }
}