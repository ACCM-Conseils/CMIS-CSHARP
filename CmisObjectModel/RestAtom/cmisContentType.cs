using System;
using sxs = System.Xml.Serialization;

namespace CmisObjectModel.RestAtom
{
    [sxs.XmlRoot("content", Namespace = Constants.Namespaces.cmisra)]
    public partial class cmisContentType
    {

        public cmisContentType(string base64, string mediaType)
        {
            _base64 = base64;
            _mediatype = mediaType;
        }
        public cmisContentType(byte[] content, string mediaType)
        {
            _base64 = content is null ? null : Convert.ToBase64String(content);
            _mediatype = mediaType;
        }

        public static implicit operator cmisContentType(Messaging.cmisContentStreamType value)
        {
            return value is null ? null : new cmisContentType(value.Stream, value.MimeType);
        }

        /// <summary>
      /// Converts Base64 to stream
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public System.IO.Stream ToStream()
        {
            if (string.IsNullOrEmpty(_base64))
            {
                return null;
            }
            else
            {
                try
                {
                    return new System.IO.MemoryStream(Convert.FromBase64String(_base64)) { Position = 0L };
                }
                catch
                {
                    return null;
                }
            }
        }

    }
}