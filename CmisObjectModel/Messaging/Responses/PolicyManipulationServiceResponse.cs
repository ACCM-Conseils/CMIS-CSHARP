
namespace CmisObjectModel.Messaging.Responses
{
    public abstract class PolicyManipulationServiceResponse : Serialization.XmlSerializable
    {

        public PolicyManipulationServiceResponse()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected PolicyManipulationServiceResponse(bool? initClassSupported) : base(initClassSupported)
        {
        }

        protected Core.cmisObjectType _object;
        /// <summary>
      /// AtomPub binding and browser binding return a cmisObject. In the AddObjectToFolder() specification
      /// (see chapter 2.2.5.1 addObjectToFolder) there is no output defined.
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public virtual Core.cmisObjectType Object
        {
            get
            {
                return _object;
            }
            set
            {
                if (!ReferenceEquals(_object, value))
                {
                    var oldValue = _object;
                    _object = value;
                    OnPropertyChanged("Object", value, oldValue);
                }
            }
        } // Object

    }
}