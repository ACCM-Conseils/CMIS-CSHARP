
namespace CmisObjectModel.Messaging
{
    public partial class cmisObjectParentsType
    {

        /// <summary>
      /// Converts AtomEntry of object
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static implicit operator cmisObjectParentsType(AtomPub.AtomEntry value)
        {
            var cmisObject = value is null ? null : value.Object;

            return cmisObject is null ? null : new cmisObjectParentsType() { Object = cmisObject, RelativePathSegment = value.RelativePathSegment };
        }

    }
}