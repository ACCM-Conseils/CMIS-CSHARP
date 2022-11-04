using System;
using System.Collections.Generic;

namespace CmisObjectModel.Collections.Generic
{
    public class TreeType<TValue>
    {

        public TreeType() : this(null)
        {
        }
        protected TreeType(TreeType<TValue> parent)
        {
            _parent = parent;
        }
        protected virtual TreeType<TValue> CreateSubTree(TreeType<TValue> parent)
        {
            return new TreeType<TValue>(parent);
        }

        private int _count = 0;
        private TreeType<TValue> _parent;
        private List<TreeType<TValue>> _subTrees = new List<TreeType<TValue>>();
        private Common.Generic.Nullable<TValue> _value = default;

        /// <summary>
      /// Removes all subtrees and the resets value to unset
      /// </summary>
      /// <remarks></remarks>
        public void Clear()
        {
            foreach (TreeType<TValue> tree in _subTrees)
            {
                tree.Clear();
                tree._parent = null;
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
      /// Returns the value for specified path
      /// </summary>
      /// <param name="path"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public Common.Generic.Nullable<TValue> GetValue(params int[] path)
        {
            var tree = this;
            int uBound = path is null ? -1 : path.Length - 1;

            for (int pathIndex = 0, loopTo = uBound; pathIndex <= loopTo; pathIndex++)
            {
                int index = Math.Max(0, path[pathIndex]);
                if (tree._subTrees.Count <= index)
                {
                    return default;
                }
                else
                {
                    tree = tree._subTrees[index];
                }
            }

            return tree._value;
        }

        public TValue get_Item(params int[] path)
        {
            return get_Tree(path)._value;
        }

        public void set_Item(int[] path, TValue value)
        {
            var tree = get_Tree(path);
            if (!tree._value.HasValue)
                tree.Count += 1;
            tree._value = value;
        }

        /// <summary>
      /// Removes the value for specified path
      /// </summary>
      /// <param name="path"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public bool Remove(params int[] path)
        {
            int pathLength = path is null ? 0 : path.Length;
            var tree = this;
            var trees = new Stack<Tuple<int, TreeType<TValue>>>();
            bool retVal;

            if (path is not null)
            {
                for (int pathIndex = 0, loopTo = pathLength - 1; pathIndex <= loopTo; pathIndex++)
                {
                    int index = Math.Max(0, path[pathIndex]);
                    if (tree._subTrees.Count > index)
                    {
                        trees.Push(new Tuple<int, TreeType<TValue>>(index, tree));
                        tree = tree._subTrees[index];
                    }
                }
            }
            retVal = trees.Count == pathLength;
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
                        tree._subTrees.RemoveAt(withBlock.Item1);
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
        public List<TreeType<TValue>> SubTrees
        {
            get
            {
                return _subTrees;
            }
        }

        /// <summary>
      /// Returns a list of values of this instance and its subtrees
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public List<TValue> ToList()
        {
            var retVal = new List<TValue>();
            ToList(retVal);
            return retVal;
        }
        private void ToList(List<TValue> list)
        {
            if (_value.HasValue)
                list.Add(_value.Value);
            foreach (TreeType<TValue> tree in _subTrees)
                tree.ToList(list);
        }

        /// <summary>
      /// Returns the tree following the index-path
      /// </summary>
      /// <param name="path"></param>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public TreeType<TValue> get_Tree(params int[] path)
        {
            var retVal = this;
            int uBound = path is null ? -1 : path.Length - 1;

            for (int pathIndex = 0, loopTo = uBound; pathIndex <= loopTo; pathIndex++)
            {
                int index = Math.Max(0, path[pathIndex]);
                // ensure instances
                while (retVal._subTrees.Count <= index)
                    retVal._subTrees.Add(CreateSubTree(retVal));
                retVal = retVal._subTrees[index];
            }

            return retVal;
        }

        /// <summary>
      /// Returns the depth of path, that is the number of indexes defined in this tree
      /// </summary>
      /// <param name="path"></param>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public int get_ValidPathDepth(params int[] path)
        {
            int retVal = 0;
            var tree = this;
            int uBound = path is null ? 0 : path.Length - 1;

            for (int pathIndex = 0, loopTo = uBound; pathIndex <= loopTo; pathIndex++)
            {
                int index = Math.Max(0, path[pathIndex]);
                if (tree._subTrees.Count <= index)
                {
                    return retVal;
                }
                else
                {
                    retVal += 1;
                    tree = tree._subTrees[index];
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