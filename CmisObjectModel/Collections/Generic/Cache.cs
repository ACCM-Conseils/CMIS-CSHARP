using System;
using System.Collections.Generic;

namespace CmisObjectModel.Collections.Generic
{
    /// <summary>
   /// Cache implementation for a defined maximal amount of key-value-pairs
   /// </summary>
   /// <typeparam name="TKey"></typeparam>
   /// <typeparam name="TValue"></typeparam>
   /// <remarks></remarks>
    public class Cache<TKey, TValue>
    {

        #region Constructors
        /// <summary>
      /// Create a new cache
      /// </summary>
      /// <param name="capacity"></param>
      /// <param name="leaseTime">Time in seconds</param>
      /// <param name="autoRenewExpiration">If True every reading access to an entry will update the expiration date of the entry</param>
      /// <remarks></remarks>
        public Cache(int capacity, double leaseTime, bool autoRenewExpiration)
        {
            _capacity = Math.Max(1, capacity);
            _fifoCapacity = _capacity;
            // not necessary to renew leasetime if the leasetime itself is set to Date.MaxValue
            _autoRenewExpiration = autoRenewExpiration && !double.IsPositiveInfinity(leaseTime);
            _leaseTime = leaseTime;
        }
        #endregion

        #region Helper-classes
        /// <summary>
      /// Entry of cache handling expiration and refcounter
      /// </summary>
      /// <remarks></remarks>
        private class CacheEntry
        {

            #region Constructors
            public CacheEntry(Cache<TKey, TValue> owner, TKey[] keys, TValue value)
            {
                _owner = owner;
                Keys = keys;
                _value = value;
                RenewExpiration();
            }
            #endregion

            #region RefCounter
            public int AddRef()
            {
                if (_refCounter > 0)
                {
                    _refCounter += 1;
                    _owner._fifoCapacity += 1;
                }

                return _refCounter;
            }
            public int Release()
            {
                if (_refCounter > 0)
                {
                    _refCounter -= 1;
                    _owner._fifoCapacity -= 1;
                }

                return _refCounter;
            }

            private int _refCounter = 1;
            public int RefCount
            {
                get
                {
                    return _refCounter;
                }
            }
            #endregion

            /// <summary>
         /// Expiration date
         /// </summary>
         /// <remarks></remarks>
            private DateTime _absoluteExpiration;

            /// <summary>
         /// Returns True if the expiration date lies in the past
         /// </summary>
         /// <value></value>
         /// <returns></returns>
         /// <remarks></remarks>
            public bool IsExpired
            {
                get
                {
                    return DateTime.UtcNow > _absoluteExpiration;
                }
            }

            /// <summary>
         /// Returns True if the instance is expired or has more than one reference in the fifo
         /// </summary>
         /// <value></value>
         /// <returns></returns>
         /// <remarks></remarks>
            public bool IsMultiReferencedOrExpired
            {
                get
                {
                    return _refCounter > 1 || IsExpired;
                }
            }

            /// <summary>
         /// Key of current instance in the cache
         /// </summary>
         /// <remarks></remarks>
            public readonly TKey[] Keys;

            private Cache<TKey, TValue> _owner;

            private TValue _value;
            public TValue Value
            {
                get
                {
                    return _value;
                }
                set
                {
                    _value = value;
                }
            }

            /// <summary>
         /// Recalculates the expiration date
         /// </summary>
         /// <remarks></remarks>
            public void RenewExpiration()
            {
                if (double.IsPositiveInfinity(_owner._leaseTime))
                {
                    _absoluteExpiration = DateTime.MaxValue;
                }
                else
                {
                    _absoluteExpiration = DateTime.UtcNow.AddSeconds(_owner._leaseTime);
                }
            }

            /// <summary>
         /// Marks the current CacheEntry as expired
         /// </summary>
         /// <remarks></remarks>
            public void SetIsExpired()
            {
                _absoluteExpiration = DateTime.MinValue;
            }
        }
        #endregion

        private bool _autoRenewExpiration;
        private int _capacity;
        private DictionaryTree<TKey, CacheEntry> _cache = new DictionaryTree<TKey, CacheEntry>();
        private int _fifoCapacity;
        private Queue<CacheEntry> _fifo = new Queue<CacheEntry>();
        private CacheEntry _lastEntry;
        private double _leaseTime;

        /// <summary>
      /// Capacity of the cache
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public int Capacity
        {
            get
            {
                return _capacity;
            }
            set
            {
                if (value > 0 && value != _capacity)
                {
                    lock (_cache)
                    {
                        int fifoCapacityOffset = _fifoCapacity - _capacity;

                        _capacity = value;
                        _fifoCapacity = value + fifoCapacityOffset;
                        Purge();
                    }
                }
            }
        }

        /// <summary>
      /// Removes cacheentries from tree and all of its subtrees.
      /// The function returns TRUE if any entry is removed
      /// </summary>
      /// <param name="tree"></param>
      /// <remarks></remarks>
        private bool Clear(DictionaryTree<TKey, CacheEntry> tree)
        {
            var nullableEntry = tree.GetValue();
            bool retVal = false;

            foreach (KeyValuePair<TKey, DictionaryTree<TKey, CacheEntry>> de in tree.SubTrees)
                retVal = Clear(de.Value) || retVal;
            if (nullableEntry.HasValue)
            {
                nullableEntry.Value.SetIsExpired();
                retVal = true;
            }
            tree.Clear();

            return retVal;
        }

        /// <summary>
      /// Returns the CacheEntry for given key if exists, otherwise null
      /// </summary>
      /// <param name="keys"></param>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        private CacheEntry get_Entry(params TKey[] keys)
        {
            var nullableEntry = _cache.GetValue(keys);

            if (nullableEntry.HasValue)
            {
                var retVal = nullableEntry.Value;

                if (retVal.IsExpired)
                {
                    _cache.Remove(keys);
                    return null;
                }
                else if (!ReferenceEquals(_lastEntry, retVal))
                {
                    // fifo has to be updated
                    _lastEntry = retVal;
                    if (ReferenceEquals(_fifo.Peek(), retVal))
                    {
                        _fifo.Dequeue();
                    }
                    else
                    {
                        retVal.AddRef();
                    }
                    _fifo.Enqueue(retVal);
                }
                if (_autoRenewExpiration)
                    retVal.RenewExpiration();

                return retVal;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
      /// Gets or sets the cached value
      /// </summary>
      /// <param name="keys"></param>
      /// <value></value>
      /// <returns></returns>
      /// <remarks>Use setter for adding or modifying values</remarks>
        public TValue get_Item(params TKey[] keys)
        {
            lock (_cache)
            {
                try
                {
                    var entry = get_Entry(keys);
                    return entry is null ? default : entry.Value;
                }
                finally
                {
                    Purge();
                }
            }
            // change value of entry and recalculate expiration date
        }

        public void set_Item(TKey[] keys, TValue value)
        {
            lock (_cache)
            {
                try
                {
                    var entry = get_Entry(keys);
                    if (entry is null)
                    {
                        entry = new CacheEntry(this, keys, value);
                        _cache.set_Item(keys, entry);
                        _fifo.Enqueue(entry);
                    }
                    else
                    {
                        entry.Value = value;
                        entry.RenewExpiration();
                    }
                }
                finally
                {
                    Purge();
                }
            }
        }

        /// <summary>
      /// Shrinks a bellied fifo-buffer and removes expired entries or entries that go beyond the scope of capacity
      /// </summary>
      /// <remarks>The fifo grows if an item is accessed that is not the current (_fifo.Peek()) in the fifo</remarks>
        private void Purge(bool enforce = false)
        {
            // check capacity and remove the oldest entries if necessary
            while (_cache.Count > _capacity || _fifo.Count > 0 && _fifo.Peek().IsMultiReferencedOrExpired)
            {
                {
                    var withBlock = _fifo.Dequeue();
                    if (withBlock.RefCount == 1)
                    {
                        _cache.Remove(withBlock.Keys);
                    }
                    else
                    {
                        withBlock.Release();
                    }
                }
            }

            // the fifo-buffer has to be cleaned up
            if (enforce || _fifoCapacity >> 1 > _capacity)
            {
                var fifo = new Queue<CacheEntry>();

                while (_fifo.Count > 0)
                {
                    var entry = _fifo.Dequeue();

                    if (entry.RefCount == 1)
                    {
                        if (entry.IsExpired)
                        {
                            _cache.Remove(entry.Keys);
                        }
                        else
                        {
                            fifo.Enqueue(entry);
                        }
                    }
                    else
                    {
                        entry.Release();
                    }
                }
                _fifo = fifo;
            }
        }

        /// <summary>
      /// Removes CacheEntry-instances for all paths starting with keys
      /// </summary>
      /// <param name="keys"></param>
      /// <remarks></remarks>
        public void RemoveAll(params TKey[] keys)
        {
            if (Clear(_cache.get_Tree(keys)))
                Purge(true);
        }

        /// <summary>
      /// Returns the number of keys in the given path defined in this cache-instance
      /// </summary>
      /// <param name="path"></param>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public int get_ValidPathDepth(params TKey[] path)
        {
            return _cache.get_ValidPathDepth(path);
        }

    }
}