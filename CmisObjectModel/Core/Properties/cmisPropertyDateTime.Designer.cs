﻿using System;
using System.Collections.Generic;
using sx = System.Xml;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Core.Properties
{
    /// <summary>
   /// </summary>
   /// <remarks>
   /// see cmisPropertyDateTime
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Core.xsd
   /// </remarks>
    public partial class cmisPropertyDateTime : Generic.cmisProperty<DateTimeOffset>
    {

        public cmisPropertyDateTime()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisPropertyDateTime(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisPropertyDateTime, string>> _setter = new Dictionary<string, Action<cmisPropertyDateTime, string>>() { }; // _setter

        /// <summary>
      /// Deserialization of all properties stored in subnodes
      /// </summary>
      /// <param name="reader"></param>
      /// <remarks></remarks>
        protected override void ReadXmlCore(sx.XmlReader reader, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            base.ReadXmlCore(reader, attributeOverrides);
            _values = ReadArray<DateTimeOffset>(reader, attributeOverrides, "value", Constants.Namespaces.cmis);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            base.WriteXmlCore(writer, attributeOverrides);
            WriteArray(writer, attributeOverrides, "value", Constants.Namespaces.cmis, _values);
        }
        #endregion

    }
}