using sx = System.Xml;
using sxs = System.Xml.Serialization;
using cac = CmisObjectModel.Attributes.CmisTypeInfoAttribute;
using CmisObjectModel.Constants;

namespace CmisObjectModel.Extensions.Data
{
    [sxs.XmlRoot("converterDefinitionItem", Namespace = Namespaces.com)]
    [cac("com:converterDefinitionItem", null, "converterDefinitionItem")]
    public class ConverterDefinitionItem : Serialization.XmlSerializable
    {

        public ConverterDefinitionItem()
        {
        }
        public ConverterDefinitionItem(string key, string value)
        {
            _key = key;
            _value = value;
        }

        #region IXmlSerializable
        protected override void ReadAttributes(sx.XmlReader reader)
        {
        }

        protected override void ReadXmlCore(sx.XmlReader reader, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            _key = Read(reader, attributeOverrides, "key", null);
            _value = Read(reader, attributeOverrides, "value", null);
        }

        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            if (!string.IsNullOrEmpty(_key))
                WriteElement(writer, attributeOverrides, "key", Namespaces.com, _key);
            if (!string.IsNullOrEmpty(_value))
                WriteElement(writer, attributeOverrides, "value", Namespaces.com, _value);
        }
        #endregion

        private string _key;
        public string Key
        {
            get
            {
                return _key;
            }
            set
            {
                if ((_key ?? "") != (value ?? ""))
                {
                    string oldValue = _key;
                    _key = value;
                    OnPropertyChanged("Key", value, oldValue);
                }
            }
        } // Key

        private string _value;
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                if ((_value ?? "") != (value ?? ""))
                {
                    string oldValue = _value;
                    _value = value;
                    OnPropertyChanged("Value", value, oldValue);
                }
            }
        } // Value

    }
}