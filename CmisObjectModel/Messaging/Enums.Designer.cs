
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Messaging
{
    public enum enumServiceException : int
    {
        constraint,
        nameConstraintViolation,
        contentAlreadyExists,
        filterNotValid,
        invalidArgument,
        notSupported,
        objectNotFound,
        permissionDenied,
        runtime,
        storage,
        streamNotSupported,
        updateConflict,
        versioning
    }
}