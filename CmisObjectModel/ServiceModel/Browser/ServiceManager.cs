using System;

namespace CmisObjectModel.ServiceModel.Browser
{
    /// <summary>
   /// Opens and closes servicehosts for baseAdresses
   /// </summary>
   /// <remarks></remarks>
    public class ServiceManager : Base.ServiceManager
    {

        protected override Type GetImplementedContractType()
        {
            return typeof(Contracts.IBrowserBinding);
        }

        protected override Type GetServiceType()
        {
            return typeof(CmisService);
        }

        protected override bool SupportsClientCredentialType()
        {
            return false;
        }

    }
}