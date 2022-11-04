using sxs = System.Xml.Serialization;

namespace CmisObjectModel.Core
{
    [sxs.XmlRoot("capabilities", Namespace = Constants.Namespaces.cmis)]
    public partial class cmisRepositoryCapabilitiesType
    {

        protected bool _capabilityBulkUpdatable;
        public bool CapabilityBulkUpdatable
        {
            get
            {
                return _capabilityBulkUpdatable;
            }
            set
            {
                if (_capabilityBulkUpdatable != value)
                {
                    bool oldValue = _capabilityBulkUpdatable;
                    _capabilityBulkUpdatable = value;
                    OnPropertyChanged("CapabilityBulkUpdatable", value, oldValue);
                }
            }
        } // CapabilityBulkUpdatable

    }
}