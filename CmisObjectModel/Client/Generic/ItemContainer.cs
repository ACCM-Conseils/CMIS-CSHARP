using System.Collections;
using System.Collections.Generic;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Client.Generic
{
    /// <summary>
   /// Generic container structure for types and objects in folders
   /// </summary>
   /// <typeparam name="TItem"></typeparam>
   /// <remarks></remarks>
    public class ItemContainer<TItem> : IEnumerable<ItemContainer<TItem>>
    {

        public ItemContainer(TItem item)
        {
            _item = item;
        }

        private List<ItemContainer<TItem>> _children = new List<ItemContainer<TItem>>();
        public List<ItemContainer<TItem>> Children
        {
            get
            {
                return _children;
            }
        }

        /// <summary>
      /// Returns all items without any informations about the logical hierarchy of the items.
      /// </summary>
        public List<TItem> GetAllItems()
        {
            var retVal = new List<TItem>();

            GetAllItems(retVal);
            return retVal;
        }
        private void GetAllItems(List<TItem> values)
        {
            values.Add(_item);
            foreach (ItemContainer<TItem> child in _children)
                child.GetAllItems(values);
        }
        public static List<TItem> GetAllItems(IEnumerable<ItemContainer<TItem>> itemContainers)
        {
            var retVal = new List<TItem>();

            if (itemContainers is not null)
            {
                foreach (ItemContainer<TItem> itemContainer in itemContainers)
                {
                    if (itemContainer is not null)
                        itemContainer.GetAllItems(retVal);
                }
            }

            return retVal;
        }

        private TItem _item;
        public TItem Item
        {
            get
            {
                return _item;
            }
        }

        public IEnumerator<ItemContainer<TItem>> GetEnumerator()
        {
            return _children.GetEnumerator();
        }

        private IEnumerator IEnumerator_GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => IEnumerator_GetEnumerator();
    }
}