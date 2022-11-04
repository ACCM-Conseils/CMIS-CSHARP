
namespace CmisObjectModel.Messaging.Responses
{
    public abstract class ObjectManipulationServiceResponse : Serialization.XmlSerializable
    {

        public ObjectManipulationServiceResponse()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected ObjectManipulationServiceResponse(bool? initClassSupported) : base(initClassSupported)
        {
        }

        protected Core.cmisObjectType _object;
        public virtual Core.cmisObjectType Object
        {
            get
            {
                return _object;
            }
            set
            {
                if (!ReferenceEquals(value, _object))
                {
                    var oldValue = _object;
                    _object = value;
                    OnPropertyChanged("Object", value, oldValue);
                    ObjectId = value is null ? default : value.ObjectId;
                }
            }
        } // Object

        protected string _objectId;
        public virtual string ObjectId
        {
            get
            {
                return _objectId;
            }
            set
            {
                if ((_objectId ?? "") != (value ?? ""))
                {
                    string oldValue = _objectId;
                    _objectId = value;
                    OnPropertyChanged("ObjectId", value, oldValue);
                }
            }
        } // ObjectId


    }
}