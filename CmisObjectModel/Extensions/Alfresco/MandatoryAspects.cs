using sx = System.Xml;
using sxs = System.Xml.Serialization;
using cac = CmisObjectModel.Attributes.CmisTypeInfoAttribute;
using CmisObjectModel.Constants;

namespace CmisObjectModel.Extensions.Alfresco
{
    /// <summary>
   /// Support for Alfresco MandatoryAspects extensions (in type-definitions)
   /// </summary>
   /// <remarks></remarks>
    [sxs.XmlRoot("mandatoryAspects", Namespace = Namespaces.alf)]
    [cac("alf:mandatoryAspects", null, "mandatoryAspects")]
    public class MandatoryAspects : Extension
    {

        public MandatoryAspects()
        {
        }

        public MandatoryAspects(params string[] aspects)
        {
            _aspects = aspects;
        }

        #region IXmlSerializable
        protected override void ReadAttributes(sx.XmlReader reader)
        {
        }

        /// <summary>
      /// Deserialization of all properties stored in subnodes
      /// </summary>
      /// <param name="reader"></param>
      /// <param name="attributeOverrides"></param>
      /// <remarks></remarks>
        protected override void ReadXmlCore(sx.XmlReader reader, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            _aspects = ReadArray<string>(reader, attributeOverrides, "mandatoryAspect", Namespaces.alf);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <param name="attributeOverrides"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteArray(writer, attributeOverrides, "mandatoryAspect", Namespaces.alf, _aspects);
        }
        #endregion

        private string[] _aspects;
        public virtual string[] Aspects
        {
            get
            {
                return _aspects;
            }
            set
            {
                if (!ReferenceEquals(_aspects, value))
                {
                    var oldValue = _aspects;
                    _aspects = value;
                    OnPropertyChanged("Aspects", value, oldValue);
                }
            }
        } // Aspects

    }
}