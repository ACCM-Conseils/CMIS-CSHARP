using System;
using sxs = System.Xml.Serialization;

namespace CmisObjectModel.RestAtom
{
    [sxs.XmlRoot("uritemplate", Namespace = Constants.Namespaces.cmisra)]
    public partial class cmisUriTemplateType
    {

        public cmisUriTemplateType(string template, string type, string mediaType)
        {
            _template = template;
            _type = type;
            _mediatype = mediaType;
        }
        public cmisUriTemplateType(Uri templateUri, string type, string mediaType) : this(templateUri is null ? null : templateUri.OriginalString, type, mediaType)
        {
        }

    }
}