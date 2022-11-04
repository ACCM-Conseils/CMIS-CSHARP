using System;
using System.Collections.Generic;
// ***********************************************************************************************************************
// * Project: CmisObjectModelLibrary
// * Copyright (c) 2014, Brügmann Software GmbH, Papenburg, All rights reserved
// *
// * Contact: opensource<at>patorg.de
// * 
// * CmisObjectModelLibrary is a VB.NET implementation of the Content Management Interoperability Services (CMIS) standard
// *
// * This file is part of CmisObjectModelLibrary.
// * 
// * This library is free software; you can redistribute it and/or
// * modify it under the terms of the GNU Lesser General Public
// * License as published by the Free Software Foundation; either
// * version 3.0 of the License, or (at your option) any later version.
// *
// * This library is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// * Lesser General Public License for more details.
// *
// * You should have received a copy of the GNU Lesser General Public
// * License along with this library (lgpl.txt).
// * If not, see <http://www.gnu.org/licenses/lgpl.txt>.
// ***********************************************************************************************************************
using CmisObjectModel.Common;

namespace CmisObjectModel.Collections.Generic
{
    public class DictionaryTree<TKey, TValue>
    {

        public DictionaryTree() : this(null)
        {
        }
        protected DictionaryTree(DictionaryTree<TKey, TValue> parent)
        {
            _parent = parent;
        }
        protected virtual DictionaryTree<TKey, TValue> CreateSubTree(DictionaryTree<TKey, TValue> parent)
        {
            return new DictionaryTree<TKey, TValue>(parent);
        }

        private int _count = 0;
        private DictionaryTree<TKey, TValue> _parent;
        private Dictionary<TKey, DictionaryTree<TKey, TValue>> _subTrees = new Dictionary<TKey, DictionaryTree<TKey, TValue>>();
        private Common.Generic.Nullable<TValue> _value = default;

        /// <summary>
      /// Removes all subtrees and the resets value to unset
      /// </summary>
      /// <remarks></remarks>
        public void Clear()
        {
            foreach (KeyValuePair<TKey, DictionaryTree<TKey, TValue>> de in _subTrees)
            {
                de.Value.Clear();
                de.Value._parent = null;
            }
            _subTrees.Clear();
            UnsetValue();
        }

        /// <summary>
      /// Returns the values stored in this tree and all subtrees
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public int Count
        {
            get
            {
                return _count;
            }
            private set
            {
                if (_parent is not null)
                    _parent.Count += value - _count;
                _count = value;
            }
        }

        /// <summary>
      /// Returns True if the specified keys exist in the tree
      /// </summary>
      /// <param name="keys"></param>
      /// <returns></returns>
      /// <remarks>A null-key is a placeholder for any key</remarks>
        public bool ContainsKeys(params TKey[] keys)
        {
            var tree = this;
            int uBound = keys is null ? -1 : keys.Length - 1;

            for (int index = 0, loopTo = uBound; index <= loopTo; index++)
            {
                var key = keys[index];

                if (key is null)
                {
                    // placeholder
                    if (index == uBound)
                    {
                        // last level
                        return tree._subTrees.Count > 0;
                    }
                    else
                    {
                        // search subkeys in all subtrees
                        keys = keys.Copy(index + 1);
                        foreach (DictionaryTree<TKey, TValue> subTree in tree._subTrees.Values)
                        {
                            if (subTree.ContainsKeys(keys))
                                return true;
                        }
                        return false;
                    }
                }
                else if (tree._subTrees.ContainsKey(key))
                {
                    tree = tree._subTrees[key];
                }
                else
                {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
      /// Returns the value for specified keys
      /// </summary>
      /// <param name="keys"></param>
      /// <returns></returns>
      /// <remarks>A null-key is a placeholder for any key</remarks>
        public Common.Generic.Nullable<TValue> GetValue(params TKey[] keys)
        {
            var tree = this;
            int uBound = keys is null ? -1 : keys.Length - 1;

            for (int index = 0, loopTo = uBound; index <= loopTo; index++)
            {
                var key = keys[index];

                if (key is null)
                {
                    // search in all subtrees
                    keys = keys.Copy(index + 1);
                    foreach (DictionaryTree<TKey, TValue> subTree in tree._subTrees.Values)
                    {
                        var retVal = subTree.GetValue(keys);
                        if (retVal.HasValue)
                            return retVal;
                    }
                    return default;
                }
                else if (tree._subTrees.ContainsKey(key))
                {
                    tree = tree._subTrees[key];
                }
                else
                {
                    return default;
                }
            }

            return tree._value;
        }

        public TValue get_Item(params TKey[] keys)
        {
            return get_Tree(keys)._value;
        }

        public void set_Item(TKey[] keys, TValue value)
        {
            var tree = get_Tree(keys);
            if (!tree._value.HasValue)
                tree.Count += 1;
            tree._value = value;
        }

        /// <summary>
      /// Removes the value for specified keys
      /// </summary>
      /// <param name="keys"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public bool Remove(params TKey[] keys)
        {
            int keysLength = keys is null ? 0 : keys.Length;
            var tree = this;
            var trees = new Stack<Tuple<TKey, DictionaryTree<TKey, TValue>>>();
            bool retVal;

            if (keys is not null)
            {
                foreach (TKey key in keys)
                {
                    if (key is not null && tree._subTrees.ContainsKey(key))
                    {
                        trees.Push(new Tuple<TKey, DictionaryTree<TKey, TValue>>(key, tree));
                        tree = tree._subTrees[key];
                    }
                    else
                    {
                        break;
                    }
                }
            }
            retVal = trees.Count == keysLength;
            // remove the value
            if (retVal && tree._value.HasValue)
            {
                tree._value = default;
                tree.Count -= 1;
            }
            // remove empty tree-instances (unset value, no subtrees)
            while (trees.Count > 0)
            {
                {
                    var withBlock = trees.Pop();
                    if (!tree._value.HasValue && tree._subTrees.Count == 0)
                    {
                        tree._parent = null;
                        tree = withBlock.Item2;
                        tree._subTrees.Remove(withBlock.Item1);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return retVal;
        }

        /// <summary>
      /// Returns the subtrees of the current tree
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public Dictionary<TKey, DictionaryTree<TKey, TValue>> SubTrees
        {
            get
            {
                return _subTrees;
            }
        }

        /// <summary>
      /// Returns the tree following the keys-path
      /// </summary>
      /// <param name="keys"></param>
      /// <value></value>
      /// <returns></returns>
      /// <remarks>A null-key is a placeholder for any key; null-key at the end of the keys-array will be ignored</remarks>
        public DictionaryTree<TKey, TValue> get_Tree(params TKey[] keys)
        {
            var retVal = this;
            int uBound = keys is null ? -1 : keys.Length - 1;

            for (int index = 0, loopTo = uBound; index <= loopTo; index++)
            {
                var key = keys[index];

                if (key is null)
                {
                    if (index < uBound)
                    {
                        keys = keys.Copy(index + 1);
                        // first chance: the specified subpath is defined in a subtree
                        foreach (DictionaryTree<TKey, TValue> subTree in retVal._subTrees.Values)
                        {
                            if (subTree.ContainsKeys(keys))
                                return subTree.get_Tree(keys);
                        }

                        // second chance: best fit
                        int maxPathDepth = -1;
                        foreach (DictionaryTree<TKey, TValue> subTree in retVal._subTrees.Values)
                        {
                            int pathDepth = subTree.get_ValidPathDepth(keys);
                            if (pathDepth > maxPathDepth)
                            {
                                maxPathDepth = pathDepth;
                                retVal = subTree;
                            }
                        }
                    }
                    else
                    {
                        // get the first subtree
                        foreach (DictionaryTree<TKey, TValue> subTree in retVal._subTrees.Values)
                        {
                            retVal = subTree;
                            break;
                        }
                    }
                    break;
                }
                else
                {
                    if (!retVal._subTrees.ContainsKey(key))
                        retVal._subTrees.Add(key, CreateSubTree(retVal));
                    retVal = retVal._subTrees[key];
                }
            }

            return retVal;
        }

        /// <summary>
      /// Returns the depth of path, that is the number of keys defined in this tree
      /// </summary>
      /// <param name="path"></param>
      /// <value></value>
      /// <returns></returns>
      /// <remarks>A null-key is a placeholder for any key</remarks>
        public int get_ValidPathDepth(params TKey[] path)
        {
            int retVal = 0;
            var tree = this;
            int uBound = path is null ? 0 : path.Length - 1;

            for (int index = 0, loopTo = uBound; index <= loopTo; index++)
            {
                var key = path[index];

                if (key is null)
                {
                    if (index < uBound)
                    {
                        int maxValidPathDepth = -1;

                        path = path.Copy(index + 1);
                        foreach (DictionaryTree<TKey, TValue> subTree in tree._subTrees.Values)
                            maxValidPathDepth = Math.Max(maxValidPathDepth, subTree.get_ValidPathDepth(path));
                        return retVal + maxValidPathDepth + 1;
                    }
                    else if (tree._subTrees.Count == 0)
                    {
                        return retVal;
                    }
                    else
                    {
                        return retVal + 1;
                    }
                }
                else if (tree._subTrees.ContainsKey(key))
                {
                    retVal += 1;
                    tree = tree._subTrees[key];
                }
                else
                {
                    break;
                }
            }

            return retVal;
        }

        /// <summary>
      /// Resets value to unset
      /// </summary>
      /// <remarks></remarks>
        public void UnsetValue()
        {
            if (_value.HasValue)
            {
                _value = default;
                Count -= 1;
            }
        }

    }
}