using System;
using System.Collections.Generic;
using System.Linq;
using sx = System.Xml;
using CmisObjectModel.Core.Properties;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Core.Collections
{
    /// <summary>
   /// </summary>
   /// <remarks>
   /// see cmisPropertiesType
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Core.xsd
   /// </remarks>
    [System.CodeDom.Compiler.GeneratedCode("CmisXsdConverter", "1.0.0.0")]
    public partial class cmisPropertiesType : Serialization.XmlSerializable
    {

        public cmisPropertiesType()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisPropertiesType(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisPropertiesType, string>> _setter = new Dictionary<string, Action<cmisPropertiesType, string>>() { }; // _setter

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
            _properties = new HashSet<cmisProperty>(ReadArray(reader, attributeOverrides, null, cmisProperty.CreateInstance), new cmisPropertyEqualityComparer());
            _extensions = ReadArray(reader, attributeOverrides, null, CmisObjectModel.Extensions.Extension.CreateInstance);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteArray(writer, attributeOverrides, null, Constants.Namespaces.cmis, _properties.ToArray());
            WriteArray(writer, attributeOverrides, null, Constants.Namespaces.cmis, _extensions);
        }
        #endregion

        protected Extensions.Extension[] _extensions;
        public virtual Extensions.Extension[] Extensions
        {
            get
            {
                return _extensions;
            }
            set
            {
                if (!ReferenceEquals(value, _extensions))
                {
                    var oldValue = _extensions;
                    _extensions = value;
                    OnPropertyChanged("Extensions", value, oldValue);
                }
            }
        } // Extensions
        private static cmisPropertyEqualityComparer _equalityComparer = new cmisPropertyEqualityComparer();
        static HashSet<cmisProperty> initial_properties() => new HashSet<cmisProperty>(_equalityComparer);

        protected static HashSet<cmisProperty> _properties;
        public virtual cmisProperty[] Properties
        {
            get
            {
                return _properties.ToArray();
            }
            set
            {
                if (value is null)
                {
                    var oldValue = _properties.ToArray();
                    _properties.Clear();
                    OnPropertyChanged("Properties", _properties.ToArray(), oldValue);
                }
                if (_properties.Count != value.Count() || !_properties.All(e => value.Contains(e)))
                {
                    var oldValue = _properties.ToArray();
                    _properties = new HashSet<cmisProperty>(value, _equalityComparer);
                    OnPropertyChanged("Properties", _properties.ToArray(), oldValue);
                }
            }
        } // Properties

        public bool AddProperty(cmisProperty p)
        {
            if (p is null)
                return false;
            var oldValue = _properties.ToArray();
            if (_properties.Add(p))
            {
                OnPropertyChanged("Properties", _properties.ToArray(), oldValue);
                return true;
            }
            return false;
        }

        public bool RemoveProperty(cmisProperty p)
        {
            if (p is null)
                return false;
            var oldValue = _properties.ToArray();
            if (_properties.Remove(p))
            {
                OnPropertyChanged("Properties", _properties.ToArray(), oldValue);
                return true;
            }
            return false;
        }

        public bool Contains(cmisProperty p)
        {
            return _properties.Contains(p);
        }
        private static cmisProperty _defaultProperty = new cmisPropertyString();
        public cmisProperty GetByPropertyDefinitionId(string propertyDefinitionId)
        {
            lock (_defaultProperty)
            {
                _defaultProperty.PropertyDefinitionId = propertyDefinitionId;
                cmisProperty p = null;
                _properties.TryGetValue(_defaultProperty, out p);
                return p;
            }
        }
        public void Clear()
        {
            var oldValue = _properties.ToArray();
            _properties.Clear();
            OnPropertyChanged("Properties", _properties.ToArray(), oldValue);
        }


        #region helper classes
        private class cmisPropertyEqualityComparer : IEqualityComparer<cmisProperty>
        {

            public new bool Equals(cmisProperty x, cmisProperty y)
            {
                return x is not null && y is not null && x.PropertyDefinitionId.Equals(y.PropertyDefinitionId);
            }

            public new int GetHashCode(cmisProperty obj)
            {
                return (int)obj?.PropertyDefinitionId.GetHashCode();
            }
        }
        #endregion
    }
}