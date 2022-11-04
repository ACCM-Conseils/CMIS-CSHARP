using sc = System.ComponentModel;
using sx = System.Xml;

namespace CmisObjectModel.Serialization
{
    /// <summary>
   /// A simple instantiatable XmlSerializable
   /// </summary>
   /// <remarks></remarks>
    public sealed class EmptyXmlSerializable : XmlSerializable
    {

        #region INotifyPropertyChanged
        public override void AddHandler(sc.PropertyChangedEventHandler handler, string propertyName)
        {
        }
        public override void AddHandler(sc.PropertyChangedEventHandler handler, params string[] propertyNames)
        {
        }
        public override void RemoveHandler(sc.PropertyChangedEventHandler handler, string propertyName)
        {
        }
        public override void RemoveHandler(sc.PropertyChangedEventHandler handler, params string[] propertyNames)
        {
        }
        #endregion

        #region IXmlSerializable
        protected override void ReadAttributes(sx.XmlReader reader)
        {
        }

        protected override void ReadXmlCore(sx.XmlReader reader, XmlAttributeOverrides attributeOverrides)
        {
        }

        protected override void WriteXmlCore(sx.XmlWriter writer, XmlAttributeOverrides attributeOverrides)
        {
        }
        #endregion

        public static readonly EmptyXmlSerializable Singleton = new EmptyXmlSerializable();
    }
}