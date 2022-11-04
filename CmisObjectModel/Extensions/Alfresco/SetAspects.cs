using System.Collections.Generic;
using sx = System.Xml;
using sxs = System.Xml.Serialization;
using cac = CmisObjectModel.Attributes.CmisTypeInfoAttribute;
using CmisObjectModel.Constants;

namespace CmisObjectModel.Extensions.Alfresco
{
    /// <summary>
   /// Support for Alfresco Aspect CRUD (in properties-collection)
   /// </summary>
   /// <remarks></remarks>
    [sxs.XmlRoot("setAspects", Namespace = Namespaces.alf)]
    [cac("alf:setAspects", null, "setAspects")]
    [Attributes.JavaScriptConverter(typeof(JSON.Extensions.Alfresco.SetAspectsConverter))]
    public class SetAspects : Extension
    {

        #region Constructors
        public SetAspects()
        {
        }

        public SetAspects(params Aspect[] aspects)
        {
            _aspects = aspects;
        }
        #endregion

        #region Helper classes
        public enum enumSetAspectsAction : int
        {
            aspectsToAdd,
            aspectsToRemove
        }

        /// <summary>
      /// SetAspects-Entry
      /// </summary>
      /// <remarks></remarks>
        public class Aspect : Aspects.Aspect
        {

            public Aspect(enumSetAspectsAction action, string aspectName) : base(aspectName)
            {
                Action = action;
            }
            public Aspect(enumSetAspectsAction action, string aspectName, Core.Properties.cmisProperty[] properties) : this(action, aspectName)
            {
                if (properties is not null)
                    Properties = new Core.Collections.cmisPropertiesType(properties);
            }
            public Aspect(enumSetAspectsAction action, string aspectName, Core.Collections.cmisPropertiesType properties) : base(aspectName, properties)
            {
                Action = action;
            }

            public readonly enumSetAspectsAction Action;
        }
        #endregion

        #region IXmlSerializable
        protected override void ReadAttributes(sx.XmlReader reader)
        {
        }

        protected override void ReadXmlCore(sx.XmlReader reader, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            Aspect currentAspect = null;
            var aspects = new List<Aspect>();

            reader.MoveToContent();
            while (reader.IsStartElement())
            {
                if (string.Compare(reader.NamespaceURI, Namespaces.alf, true) == 0)
                {
                    if (string.Compare(reader.LocalName, "aspectsToAdd", true) == 0)
                    {
                        // seal the current aspect
                        if (currentAspect is not null)
                            aspects.Add(currentAspect);
                        // start the next aspect
                        currentAspect = new Aspect(enumSetAspectsAction.aspectsToAdd, reader.ReadElementString());
                    }
                    else if (string.Compare(reader.LocalName, "aspectsToRemove", true) == 0)
                    {
                        // seal the current aspect
                        if (currentAspect is not null)
                            aspects.Add(currentAspect);
                        // write the aspectsToRemove-entry
                        aspects.Add(new Aspect(enumSetAspectsAction.aspectsToRemove, reader.ReadElementString()));
                        currentAspect = null;
                    }
                    else if (string.Compare(reader.LocalName, "properties", true) == 0)
                    {
                        // ensure aspect
                        if (currentAspect is null)
                            currentAspect = new Aspect(enumSetAspectsAction.aspectsToAdd, null);
                        // append aspects - properties
                        currentAspect.Properties = new Core.Collections.cmisPropertiesType();
                        currentAspect.Properties.ReadXml(reader);
                        aspects.Add(currentAspect);
                        currentAspect.Seal();
                        currentAspect = null;
                    }
                }
            }
            // maybe there is an aspectsToAdd-Item left
            if (currentAspect is not null)
                aspects.Add(currentAspect);
            _aspects = aspects.Count == 0 ? null : aspects.ToArray();
        }

        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            if (_aspects is not null)
            {
                foreach (Aspect setAspect in _aspects)
                {
                    if (setAspect is not null)
                    {
                        if (!string.IsNullOrEmpty(setAspect.AspectName))
                        {
                            switch (setAspect.Action)
                            {
                                case enumSetAspectsAction.aspectsToAdd:
                                    {
                                        WriteElement(writer, attributeOverrides, "aspectsToAdd", Namespaces.alf, setAspect.AspectName);
                                        break;
                                    }
                                case enumSetAspectsAction.aspectsToRemove:
                                    {
                                        WriteElement(writer, attributeOverrides, "aspectsToRemove", Namespaces.alf, setAspect.AspectName);
                                        break;
                                    }
                            }
                        }
                        if (setAspect.Properties is not null && setAspect.Action == enumSetAspectsAction.aspectsToAdd)
                        {
                            WriteElement(writer, attributeOverrides, "properties", Namespaces.alf, setAspect.Properties);
                        }
                    }
                }
            }
        }
        #endregion

        private Aspect[] _aspects;
        public virtual Aspect[] Aspects
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