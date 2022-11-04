using System;

namespace CmisObjectModel.ServiceModel.AtomPub
{
    /// <summary>
   /// Opens and closes servicehosts for baseAdresses
   /// </summary>
   /// <remarks></remarks>
    public class ServiceManager : Base.ServiceManager
    {

        protected override Type GetImplementedContractType()
        {
            return typeof(Contracts.IAtomPubBinding);
        }

        protected override Type GetServiceType()
        {
            return typeof(CmisService);
        }

        protected override bool SupportsClientCredentialType()
        {
            return true;
        }

    }
}