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
   /// see cmisExtensionFeatureType
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Core.xsd
   /// </remarks>
    public partial class cmisExtensionFeatureType : Serialization.XmlSerializable
    {

        public cmisExtensionFeatureType()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisExtensionFeatureType(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisExtensionFeatureType, string>> _setter = new Dictionary<string, Action<cmisExtensionFeatureType, string>>() { }; // _setter

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
            _url = Read(reader, attributeOverrides, "url", Constants.Namespaces.cmis, _url);
            _commonName = Read(reader, attributeOverrides, "commonName", Constants.Namespaces.cmis, _commonName);
            _versionLabel = Read(reader, attributeOverrides, "versionLabel", Constants.Namespaces.cmis, _versionLabel);
            _description = Read(reader, attributeOverrides, "description", Constants.Namespaces.cmis, _description);
            _featureDatas = ReadArray(reader, attributeOverrides, "featureData", Constants.Namespaces.cmis, GenericXmlSerializableFactory<cmisExtensionFeatureKeyValuePair>);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteElement(writer, attributeOverrides, "id", Constants.Namespaces.cmis, _id);
            if (!string.IsNullOrEmpty(_url))
                WriteElement(writer, attributeOverrides, "url", Constants.Namespaces.cmis, _url);
            if (!string.IsNullOrEmpty(_commonName))
                WriteElement(writer, attributeOverrides, "commonName", Constants.Namespaces.cmis, _commonName);
            if (!string.IsNullOrEmpty(_versionLabel))
                WriteElement(writer, attributeOverrides, "versionLabel", Constants.Namespaces.cmis, _versionLabel);
            if (!string.IsNullOrEmpty(_description))
                WriteElement(writer, attributeOverrides, "description", Constants.Namespaces.cmis, _description);
            WriteArray(writer, attributeOverrides, "featureData", Constants.Namespaces.cmis, _featureDatas);
        }
        #endregion

        protected string _commonName;
        public virtual string CommonName
        {
            get
            {
                return _commonName;
            }
            set
            {
                if ((_commonName ?? "") != (value ?? ""))
                {
                    string oldValue = _commonName;
                    _commonName = value;
                    OnPropertyChanged("CommonName", value, oldValue);
                }
            }
        } // CommonName

        protected string _description;
        public virtual string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if ((_description ?? "") != (value ?? ""))
                {
                    string oldValue = _description;
                    _description = value;
                    OnPropertyChanged("Description", value, oldValue);
                }
            }
        } // Description

        protected cmisExtensionFeatureKeyValuePair[] _featureDatas;
        public virtual cmisExtensionFeatureKeyValuePair[] FeatureDatas
        {
            get
            {
                return _featureDatas;
            }
            set
            {
                if (!ReferenceEquals(value, _featureDatas))
                {
                    var oldValue = _featureDatas;
                    _featureDatas = value;
                    OnPropertyChanged("FeatureDatas", value, oldValue);
                }
            }
        } // FeatureDatas

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

        protected string _url;
        public virtual string Url
        {
            get
            {
                return _url;
            }
            set
            {
                if ((_url ?? "") != (value ?? ""))
                {
                    string oldValue = _url;
                    _url = value;
                    OnPropertyChanged("Url", value, oldValue);
                }
            }
        } // Url

        protected string _versionLabel;
        public virtual string VersionLabel
        {
            get
            {
                return _versionLabel;
            }
            set
            {
                if ((_versionLabel ?? "") != (value ?? ""))
                {
                    string oldValue = _versionLabel;
                    _versionLabel = value;
                    OnPropertyChanged("VersionLabel", value, oldValue);
                }
            }
        } // VersionLabel

    }
}