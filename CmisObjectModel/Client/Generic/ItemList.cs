using System.Collections;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Client.Generic
{
    /// <summary>
   /// Generic list for types and objects in folders supporting HasMoreItems and NumItems
   /// </summary>
   /// <typeparam name="TItem"></typeparam>
   /// <remarks></remarks>
    public class ItemList<TItem> : IEnumerable
    {

        public ItemList(TItem[] items, bool hasMoreItems, long? numItems)
        {
            HasMoreItems = hasMoreItems;
            Items = items;
            NumItems = numItems;
        }

        public readonly bool HasMoreItems;
        public readonly TItem[] Items;
        public readonly long? NumItems;

        public IEnumerator GetEnumerator()
        {
            return (Items ?? (new TItem[] { })).GetEnumerator();
        }
    }
}