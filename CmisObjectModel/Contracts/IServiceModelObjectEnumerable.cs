using System.Collections;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Contracts
{
    public interface IServiceModelObjectEnumerable : IEnumerable
    {

        bool ContainsObjects { get; }
        bool HasMoreItems { get; }
        long? NumItems { get; }
    }
}