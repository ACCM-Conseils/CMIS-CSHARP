using sx = System.Xml;
using sxs = System.Xml.Serialization;
using cac = CmisObjectModel.Attributes.CmisTypeInfoAttribute;
using CmisObjectModel.Constants;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Extensions.Common
{
    /// <summary>
   /// Properties extension to return the newId in a bulkUpdateProperties-call
   /// </summary>
   /// <remarks></remarks>
    [sxs.XmlRoot("newId", Namespace = Namespaces.com)]
    [cac("com:newId", null, "newId")]
    public class NewIdExtension : Extension
    {

        #region IXmlSerializable
        protected override void ReadAttributes(sx.XmlReader reader)
        {
        }

        protected override void ReadXmlCore(sx.XmlReader reader, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            _newId = Read(reader, attributeOverrides, "newId", Namespaces.com, _newId);
        }

        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            if (!string.IsNullOrEmpty(_newId))
                WriteElement(writer, attributeOverrides, "newId", Namespaces.com, _newId);
        }
        #endregion

        private string _newId;
        /// <summary>
      /// The RowState of this instance
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public string NewId
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