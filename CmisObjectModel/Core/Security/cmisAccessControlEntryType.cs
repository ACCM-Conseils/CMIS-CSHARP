using sxs = System.Xml.Serialization;

namespace CmisObjectModel.Core.Security
{
    [sxs.XmlRoot("permission", Namespace = Constants.Namespaces.cmis)]
    public partial class cmisAccessControlEntryType
    {

        /// <summary>
      /// Same as property Direct; using the BrowserBinding the Exact-parameter is called IsDirect
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public bool IsDirect
        {
            get
            {
                return _direct;
            }
            set
            {
                Direct = value;
            }
        }

    }
}