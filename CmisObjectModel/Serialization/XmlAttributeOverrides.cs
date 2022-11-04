using System;
using System.Collections.Generic;
using sx = System.Xml;
using sxs = System.Xml.Serialization;

namespace CmisObjectModel.Serialization
{
    /// <summary>
   /// Customization of the ReadXml() and WriteXml() in XmlSerializable-instances
   /// </summary>
   /// <remarks>Using-block required</remarks>
    public class XmlAttributeOverrides : IDisposable
    {

        #region Constructors
        public XmlAttributeOverrides(sx.XmlReader reader) : this(reader, null)
        {
        }
        public XmlAttributeOverrides(sx.XmlReader reader, sxs.XmlAttributeOverrides attributeOverrides)
        {
            lock (_syncObject)
            {
                if (!(reader is null || _instances.ContainsKey(reader)))
                {
                    _key = reader;
                    _instances.Add(reader, this);
                }
            }
            _innerAttributeOverrides = attributeOverrides ?? new sxs.XmlAttributeOverrides();
        }
        public XmlAttributeOverrides(sx.XmlWriter writer) : this(writer, null)
        {
        }
        public XmlAttributeOverrides(sx.XmlWriter writer, sxs.XmlAttributeOverrides attributeOverrides)
        {
            lock (_syncObject)
            {
                if (!(writer is null || _instances.ContainsKey(writer)))
                {
                    _key = writer;
                    _instances.Add(writer, this);
                }
            }
            _innerAttributeOverrides = attributeOverrides ?? new sxs.XmlAttributeOverrides();
        }

        /// <summary>
        /// Returns the XmlAttributeOverrides-instance assigned to the reader-instance if exists otherwise null
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static XmlAttributeOverrides GetInstance(sx.XmlReader reader)
        {
            return GetInstance((object)reader);
        }
        /// <summary>
        ///       ''' Returns the XmlAttributeOverrides-instance assigned to the writer-instance if exists otherwise null
        ///       ''' </summary>
        ///       ''' <param name="writer"></param>
        ///       ''' <returns></returns>
        ///       ''' <remarks></remarks>
        public static XmlAttributeOverrides GetInstance(sx.XmlWriter writer)
        {
            return GetInstance((object)writer);
        }
        /// <summary>
        ///       ''' Returns the XmlAttributeOverrides-instance assigned to the key-instance if exists otherwise null
        ///       ''' </summary>
        ///       ''' <param name="key"></param>
        ///       ''' <returns></returns>
        ///       ''' <remarks></remarks>
        private static XmlAttributeOverrides GetInstance(object key)
        {
            lock (_syncObject)
                return key != null && _instances.ContainsKey(key) ? _instances[key] : null;
        }
        #endregion

        #region IDisposable Support
        private bool _isDisposed;
        public bool IsDisposed
        {
            get
            {
                return _isDisposed;
            }
        }

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    lock (_syncObject)
                    {
                        if (_key is not null)
                            _instances.Remove(_key);
                        _key = null;
                        _isDisposed = true;
                    }
                }
                else
                {
                    _isDisposed = true;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        private sxs.XmlAttributeOverrides _innerAttributeOverrides;
        private static Dictionary<object, XmlAttributeOverrides> _instances = new Dictionary<object, XmlAttributeOverrides>();
        private object _key;
        private static object _syncObject = new object();

        public sxs.XmlElementAttribute get_XmlElement(Type type, string memberName)
        {
            var attributes = _innerAttributeOverrides[type, memberName];
            var elementAttributes = attributes is null ? null : attributes.XmlElements;
            return elementAttributes is null || elementAttributes.Count == 0 ? null : elementAttributes[0];
        }

        public void set_XmlElement(Type type, string memberName, sxs.XmlElementAttribute value)
        {
            var attributes = _innerAttributeOverrides[type, memberName];
            if (attributes is null)
            {
                attributes = new sxs.XmlAttributes();
                _innerAttributeOverrides.Add(type, memberName, attributes);
            }
            else
            {
                attributes.XmlElements.Clear();
            }

            if (value is not null)
                attributes.XmlElements.Add(value);
        }

        public sxs.XmlRootAttribute get_XmlRoot(Type type)
        {
            var attributes = _innerAttributeOverrides[type];
            return attributes is null ? null : attributes.XmlRoot;
        }

        public void set_XmlRoot(Type type, sxs.XmlRootAttribute value)
        {
            var attributes = _innerAttributeOverrides[type];
            if (attributes is null)
            {
                attributes = new sxs.XmlAttributes();
                attributes.XmlRoot = value;
                _innerAttributeOverrides.Add(type, attributes);
            }
            else
            {
                attributes.XmlRoot = value;
            }
        }

        public static implicit operator sxs.XmlAttributeOverrides(XmlAttributeOverrides value)
        {
            return value is null ? null : value._innerAttributeOverrides;
        }
    }
}