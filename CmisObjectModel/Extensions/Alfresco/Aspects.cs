using System.Collections.Generic;
using sx = System.Xml;
using sxs = System.Xml.Serialization;
using cac = CmisObjectModel.Attributes.CmisTypeInfoAttribute;
using CmisObjectModel.Constants;

namespace CmisObjectModel.Extensions.Alfresco
{
    /// <summary>
   /// Support for Alfresco Aspect extensions (in properties-collection)
   /// </summary>
   /// <remarks></remarks>
    [sxs.XmlRoot("aspects", Namespace = Namespaces.alf)]
    [cac("alf:aspects", null, "aspects")]
    [Attributes.JavaScriptConverter(typeof(JSON.Extensions.Alfresco.AspectsConverter))]
    public class Aspects : Extension
    {

        #region Constructors
        public Aspects()
        {
        }

        public Aspects(params Aspect[] appliedAspects)
        {
            _appliedAspects = appliedAspects;
        }
        #endregion

        #region Helper classes
        public class Aspect
        {

            public Aspect(string aspectName)
            {
                AspectName = aspectName;
            }
            public Aspect(string aspectName, Core.Collections.cmisPropertiesType properties)
            {
                AspectName = aspectName;
                Properties = properties;
                Seal();
            }

            public readonly string AspectName;
            public Core.Collections.cmisPropertiesType Properties;

            /// <summary>
         /// Sets the declaringType-ExtendedProperty of all properties within this instance
         /// </summary>
         /// <remarks></remarks>
            public void Seal()
            {
                if (!string.IsNullOrEmpty(AspectName))
                {
                    var properties = Properties is null ? null : Properties.Properties;

                    if (properties is not null)
                    {
                        foreach (Core.Properties.cmisProperty property in properties)
                        {
                            if (property is not null)
                            {
                                {
                                    var withBlock = property.get_ExtendedProperties();
                                    if (withBlock.ContainsKey(ExtendedProperties.DeclaringType))
                                    {
                                        withBlock[ExtendedProperties.DeclaringType] = AspectName;
                                    }
                                    else
                                    {
                                        withBlock.Add(ExtendedProperties.DeclaringType, AspectName);
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }
        #endregion

        #region IXmlSerializable
        protected override void ReadAttributes(sx.XmlReader reader)
        {
        }

        protected override void ReadXmlCore(sx.XmlReader reader, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            Aspect currentAspect = null;
            var appliedAspects = new List<Aspect>();

            reader.MoveToContent();
            while (reader.IsStartElement())
            {
                if (string.Compare(reader.NamespaceURI, Namespaces.alf, true) == 0)
                {
                    if (string.Compare(reader.LocalName, "appliedAspects", true) == 0)
                    {
                        // seal the current aspect
                        if (currentAspect is not null)
                            appliedAspects.Add(currentAspect);
                        // start the next aspect
                        currentAspect = new Aspect(reader.ReadElementString());
                    }
                    else if (string.Compare(reader.LocalName, "properties", true) == 0)
                    {
                        // ensure aspect
                        if (currentAspect is null)
                            currentAspect = new Aspect(null);
                        // append aspects - properties
                        currentAspect.Properties = new Core.Collections.cmisPropertiesType();
                        currentAspect.Properties.ReadXml(reader);
                        appliedAspects.Add(currentAspect);
                        currentAspect.Seal();
                        currentAspect = null;
                    }
                }
            }
            // maybe there is an aspect-Item left
            if (currentAspect is not null)
                appliedAspects.Add(currentAspect);
            _appliedAspects = appliedAspects.Count == 0 ? null : appliedAspects.ToArray();
        }

        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            if (_appliedAspects is not null)
            {
                foreach (Aspect item in _appliedAspects)
                {
                    if (item is not null)
                    {
                        if (!string.IsNullOrEmpty(item.AspectName))
                        {
                            WriteElement(writer, attributeOverrides, "appliedAspects", Namespaces.alf, item.AspectName);
                        }
                        if (item.Properties is not null)
                        {
                            WriteElement(writer, attributeOverrides, "properties", Namespaces.alf, item.Properties);
                        }
                    }
                }
            }
        }
        #endregion

        private Aspect[] _appliedAspects;
        public virtual Aspect[] AppliedAspects
        {
            get
            {
                return _appliedAspects;
            }
            set
            {
                if (!ReferenceEquals(_appliedAspects, value))
                {
                    var oldValue = _appliedAspects;
                    _appliedAspects = value;
                    OnPropertyChanged("AppliedAspects", value, oldValue);
                }
            }
        } // AppliedAspects
    }
}