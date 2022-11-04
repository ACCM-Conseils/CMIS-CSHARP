using System;
using System.Collections.Generic;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.JSON.Serialization
{
    /// <summary>
   /// Customization of JavaScriptSerialization
   /// </summary>
   /// <remarks></remarks>
    public class JSONAttributeOverrides
    {

        #region Helper classes
        /// <summary>
      /// Customization of JavaScriptSerialization for an element of a type
      /// </summary>
      /// <remarks></remarks>
        public class JSONElementAttribute
        {
            public JSONElementAttribute(string aliasName, JavaScriptConverter elementConverter)
            {
                AliasName = aliasName ?? string.Empty;
                ElementConverter = elementConverter;
            }

            public readonly string AliasName;
            public readonly JavaScriptConverter ElementConverter;
        }

        /// <summary>
      /// Customization of JavaScriptSerialization for a type and/or elements of a type
      /// </summary>
      /// <remarks></remarks>
        public class JSONTypeAttribute
        {
            public readonly Dictionary<string, JSONElementAttribute> ElementAttributes = new Dictionary<string, JSONElementAttribute>();
            public JavaScriptConverter TypeConverter;
        }
        #endregion

        private readonly Dictionary<Type, JSONTypeAttribute> _attributes = new Dictionary<Type, JSONTypeAttribute>();

        /// <summary>
      /// Gets or sets the serialization overrides for an element of a type
      /// </summary>
        public JSONElementAttribute get_ElementAttribute(Type type, string elementName)
        {
            var jsonAttribute = _attributes.ContainsKey(type) ? _attributes[type] : null;
            return jsonAttribute is null || !jsonAttribute.ElementAttributes.ContainsKey(elementName) ? null : jsonAttribute.ElementAttributes[elementName];
        }

        public void set_ElementAttribute(Type type, string elementName, JSONElementAttribute value)
        {
            var jsonAttribute = GetOrCreateAttribute(type);
            if (value is null)
            {
                jsonAttribute.ElementAttributes.Remove(elementName);
            }
            else if (jsonAttribute.ElementAttributes.ContainsKey(elementName))
            {
                jsonAttribute.ElementAttributes[elementName] = value;
            }
            else
            {
                jsonAttribute.ElementAttributes.Add(elementName, value);
            }
        }

        /// <summary>
      /// Creates a new ElementConverter for type, elementName and aliasName
      /// </summary>
        public void set_ElementConverter(Type type, string elementName, string aliasName, JavaScriptConverter value)
        {
            if (!(type is null || string.IsNullOrEmpty(elementName) || string.IsNullOrEmpty(aliasName)))
            {
                set_ElementAttribute(type, elementName, new JSONElementAttribute(aliasName, value));
            }
        }

        /// <summary>
      /// Returns a valid JSONAttribute for given type
      /// </summary>
      /// <param name="type"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        private JSONTypeAttribute GetOrCreateAttribute(Type type)
        {
            if (_attributes.ContainsKey(type))
            {
                return _attributes[type];
            }
            else
            {
                var retVal = new JSONTypeAttribute();
                _attributes.Add(type, retVal);
                return retVal;
            }
        }

        /// <summary>
      /// Gets or sets the serialization overrides for a type and its elements
      /// </summary>
      /// <param name="type"></param>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public JSONTypeAttribute get_TypeAttribute(Type type)
        {
            return _attributes.ContainsKey(type) ? _attributes[type] : null;
        }

        public void set_TypeAttribute(Type type, JSONTypeAttribute value)
        {
            if (value is null)
            {
                _attributes.Remove(type);
            }
            else if (_attributes.ContainsKey(type))
            {
                _attributes[type] = value;
            }
            else
            {
                _attributes.Add(type, value);
            }
        }

        /// <summary>
      /// Gets or sets the serialization overrides for a type
      /// </summary>
        public JavaScriptConverter get_TypeConverter(Type type)
        {
            return _attributes.ContainsKey(type) ? _attributes[type].TypeConverter : null;
        }

        public void set_TypeConverter(Type type, JavaScriptConverter value)
        {
            GetOrCreateAttribute(type).TypeConverter = value;
        }

    }
}