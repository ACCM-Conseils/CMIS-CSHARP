using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using st = System.Text;
using str = System.Text.RegularExpressions;

namespace CmisObjectModel.ServiceModel.Query
{
    /// <summary>
   /// Base class of all expressions represented in more than one matches in the query string
   /// </summary>
   /// <remarks></remarks>
    public abstract class CompositeExpression : Expression
    {

        #region Constructors
        protected CompositeExpression(str.Match match, string groupName, int rank, int index, string childrenSeparator, string childBlockSeparator) : base(match, groupName, rank, index)
        {
            _childrenSeparator = childrenSeparator;
            _childBlockSeparator = childBlockSeparator;
        }
        #endregion

        #region Helper classes
        public delegate bool fnContinueWith(CompositeExpression instance, enumContinueWith continueWith);

        public enum enumContinueWith
        {
            currentInstance,
            children
        }

        /// <summary>
      /// Simple tree of children
      /// </summary>
      /// <remarks></remarks>
        public class GetDescendantsResult
        {
            //A remplacer
            /*public GetDescendantsResult(CompositeExpression currentInstance, Func<Expression, bool> matchSelector, Func<CompositeExpression, CompositeExpression> instanceSelector) : this(currentInstance, matchSelector, instanceSelector, default)
            {
            }*/
            private GetDescendantsResult(CompositeExpression currentInstance, Func<Expression, bool> matchSelector, Func<CompositeExpression, CompositeExpression> instanceSelector, GetDescendantsResult parent)
            {
                var childrenDescendants = new List<GetDescendantsResult>();
                var children = new List<Expression>(currentInstance.GetChildren(matchSelector));

                Instance = instanceSelector(currentInstance);
                Parent = parent;
                foreach (Expression child in currentInstance._children)
                {
                    if (child is CompositeExpression)
                    {
                        var childDescendant = new GetDescendantsResult((CompositeExpression)child, matchSelector, instanceSelector, this);

                        if (ReferenceEquals(childDescendant.Instance, Instance))
                        {
                            // combine results (via instanceSelector the same instance has been chosen)
                            children.AddRange(childDescendant.Children);
                            childrenDescendants.AddRange(childDescendant.ChildrenDescendants);
                        }
                        else if (childDescendant.Children.Length > 0)
                        {
                            // normal
                            childrenDescendants.Add(childDescendant);
                        }
                        else if (childDescendant.ChildrenDescendants.Length > 0)
                        {
                            // skip levels without children
                            childrenDescendants.AddRange(childDescendant.ChildrenDescendants);
                        }
                    }
                }
                Children = children.ToArray();
                ChildrenDescendants = childrenDescendants.ToArray();
            }

            public GetDescendantsResult(CompositeExpression currentInstance, Func<Expression, bool> matchSelector, Func<CompositeExpression, CompositeExpression> instanceSelector, fnContinueWith continueWith) : this(currentInstance, matchSelector, instanceSelector, continueWith, null)
            {
            }
            private GetDescendantsResult(CompositeExpression currentInstance, Func<Expression, bool> matchSelector, Func<CompositeExpression, CompositeExpression> instanceSelector, fnContinueWith continueWith, GetDescendantsResult parent)
            {
                var childrenDescendants = new List<GetDescendantsResult>();
                var children = new List<Expression>(currentInstance.GetChildren(matchSelector));

                Instance = instanceSelector(currentInstance);
                Parent = parent;
                if (continueWith(currentInstance, enumContinueWith.children))
                {
                    foreach (Expression child in currentInstance._children)
                    {
                        if (child is CompositeExpression && continueWith((CompositeExpression)child, enumContinueWith.currentInstance))
                        {
                            var childDescendant = new GetDescendantsResult((CompositeExpression)child, matchSelector, instanceSelector, continueWith, this);

                            if (ReferenceEquals(childDescendant.Instance, Instance))
                            {
                                // combine results (via instanceSelector the same instance has been chosen)
                                children.AddRange(childDescendant.Children);
                                childrenDescendants.AddRange(childDescendant.ChildrenDescendants);
                            }
                            else if (childDescendant.Children.Length > 0)
                            {
                                // normal
                                childrenDescendants.Add(childDescendant);
                            }
                            else if (childDescendant.ChildrenDescendants.Length > 0)
                            {
                                // skip levels without children
                                childrenDescendants.AddRange(childDescendant.ChildrenDescendants);
                            }
                        }
                    }
                }
                Children = children.ToArray();
                ChildrenDescendants = childrenDescendants.ToArray();
            }

            public readonly Expression[] Children;
            public readonly GetDescendantsResult[] ChildrenDescendants;
            public readonly CompositeExpression Instance;
            public readonly GetDescendantsResult Parent;

            public List<Expression> ToList()
            {
                var retVal = new List<Expression>();
                ToList(retVal);
                return retVal;
            }
            private void ToList(List<Expression> list)
            {
                list.AddRange(Children);
                foreach (GetDescendantsResult subList in ChildrenDescendants)
                    subList.ToList(list);
            }
        }

        /// <summary>
      /// Simple generic tree of children
      /// </summary>
      /// <remarks></remarks>
        public class GetDescendantsResult<TInstance, TChild>
              where TInstance : CompositeExpression
              where TChild : Expression
        {
            //A remplacer
            /*public GetDescendantsResult(CompositeExpression currentInstance, Func<TChild, bool> matchSelector, Func<CompositeExpression, TInstance> instanceSelector) : this(currentInstance, matchSelector, instanceSelector, default)
            {
            }*/
            private GetDescendantsResult(CompositeExpression currentInstance, Func<TChild, bool> matchSelector, Func<CompositeExpression, TInstance> instanceSelector, GetDescendantsResult<TInstance, TChild> parent)
            {
                var childrenDescendants = new List<GetDescendantsResult<TInstance, TChild>>();
                var children = new List<TChild>(currentInstance.GetChildren(matchSelector));

                Instance = instanceSelector(currentInstance);
                Parent = parent;
                foreach (Expression child in currentInstance._children)
                {
                    if (child is CompositeExpression)
                    {
                        var childDescendant = new GetDescendantsResult<TInstance, TChild>((CompositeExpression)child, matchSelector, instanceSelector, this);

                        if (ReferenceEquals(childDescendant.Instance, Instance))
                        {
                            // combine results (via instanceSelector the same instance has been chosen)
                            children.AddRange(childDescendant.Children);
                            childrenDescendants.AddRange(childDescendant.ChildrenDescendants);
                        }
                        else if (childDescendant.Children.Length > 0)
                        {
                            // normal
                            childrenDescendants.Add(childDescendant);
                        }
                        else if (childDescendant.ChildrenDescendants.Length > 0)
                        {
                            // skip levels without children
                            childrenDescendants.AddRange(childDescendant.ChildrenDescendants);
                        }
                    }
                }
                Children = children.ToArray();
                ChildrenDescendants = childrenDescendants.ToArray();
            }

            public GetDescendantsResult(CompositeExpression currentInstance, Func<TChild, bool> matchSelector, Func<CompositeExpression, TInstance> instanceSelector, fnContinueWith continueWith) : this(currentInstance, matchSelector, instanceSelector, continueWith, null)
            {
            }
            private GetDescendantsResult(CompositeExpression currentInstance, Func<TChild, bool> matchSelector, Func<CompositeExpression, TInstance> instanceSelector, fnContinueWith continueWith, GetDescendantsResult<TInstance, TChild> parent)
            {
                var childrenDescendants = new List<GetDescendantsResult<TInstance, TChild>>();
                var children = new List<TChild>(currentInstance.GetChildren(matchSelector));

                Instance = instanceSelector(currentInstance);
                Parent = parent;
                if (continueWith(currentInstance, enumContinueWith.children))
                {
                    foreach (Expression child in currentInstance._children)
                    {
                        if (child is CompositeExpression && continueWith((CompositeExpression)child, enumContinueWith.currentInstance))
                        {
                            var childDescendant = new GetDescendantsResult<TInstance, TChild>((CompositeExpression)child, matchSelector, instanceSelector, this);

                            if (ReferenceEquals(childDescendant.Instance, Instance))
                            {
                                // combine results (via instanceSelector the same instance has been chosen)
                                children.AddRange(childDescendant.Children);
                                childrenDescendants.AddRange(childDescendant.ChildrenDescendants);
                            }
                            else if (childDescendant.Children.Length > 0)
                            {
                                // normal
                                childrenDescendants.Add(childDescendant);
                            }
                            else if (childDescendant.ChildrenDescendants.Length > 0)
                            {
                                // skip levels without children
                                childrenDescendants.AddRange(childDescendant.ChildrenDescendants);
                            }
                        }
                    }
                }
                Children = children.ToArray();
                ChildrenDescendants = childrenDescendants.ToArray();
            }

            public readonly TChild[] Children;
            public readonly GetDescendantsResult<TInstance, TChild>[] ChildrenDescendants;
            public readonly TInstance Instance;
            public readonly GetDescendantsResult<TInstance, TChild> Parent;

            public List<TChild> ToList()
            {
                var retVal = new List<TChild>();
                ToList(retVal);
                return retVal;
            }
            private void ToList(List<TChild> list)
            {
                list.AddRange(Children);
                foreach (GetDescendantsResult<TInstance, TChild> subList in ChildrenDescendants)
                    subList.ToList(list);
            }
        }
        #endregion

        protected string _childBlockSeparator;
        protected List<Expression> _children = new List<Expression>();
        protected string _childrenSeparator;

        /// <summary>
      /// Returns the count of children
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public int Count
        {
            get
            {
                return _children.Count;
            }
        }

        /// <summary>
      /// Returns the identifiers of fields contained within the current instance-type
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public GetDescendantsResult FieldIdentifiers
        {
            get
            {
                return GetIdentifiers<FieldExpression>();
            }
        }

        /// <summary>
      /// Returns a list of matching children
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public Expression[] GetChildren(Func<Expression, bool> matchSelector)
        {
            return (from child in _children
                    where matchSelector(child)
                    select child).ToArray();
        }
        /// <summary>
      /// Returns a list of matching children
      /// </summary>
        public TExpression[] GetChildren<TExpression>() where TExpression : Expression
        {
            return (from child in _children
                    where child is TExpression
                    select ((TExpression)child)).ToArray();
        }
        /// <summary>
      /// Returns a list of matching children
      /// </summary>
        public TExpression[] GetChildren<TExpression>(Func<TExpression, bool> matchSelector) where TExpression : Expression
        {
            return (from child in _children
                    where child is TExpression && matchSelector((TExpression)child)
                    select ((TExpression)child)).ToArray();
        }

        /// <summary>
      /// Returns a tree of matching descendants
      /// </summary>
      /// <param name="matchSelector">Decision which child should be returned</param>
      /// <param name="instanceSelector">If set the returned value is set for GetDescendantsResult.Instance property.
      /// If not set the GetDescendantsResult.Instance property is the parent not of all items listet in
      /// GetDescendantsResult.Children property.</param>
      /// <returns></returns>
      /// <remarks></remarks>
        public GetDescendantsResult GetDescendants(Func<Expression, bool> matchSelector, Func<CompositeExpression, CompositeExpression> instanceSelector = null)
        {
            //A remplacer
            return null;
            //return new GetDescendantsResult(this, matchSelector, instanceSelector ?? (instance => instance));
        }

        /// <summary>
      /// Returns a tree of matching descendants
      /// </summary>
      /// <param name="continueWith">Callback to decide when stop building the tree</param>
      /// <param name="matchSelector">Decision which child should be returned</param>
      /// <param name="instanceSelector">If set the returned value is set for GetDescendantsResult.Instance property.
      /// If not set the GetDescendantsResult.Instance property is the parent not of all items listet in
      /// GetDescendantsResult.Children property.</param>
      /// <returns></returns>
      /// <remarks></remarks>
        public GetDescendantsResult GetDescendants(fnContinueWith continueWith, Func<Expression, bool> matchSelector, Func<CompositeExpression, CompositeExpression> instanceSelector = null)
        {
            return new GetDescendantsResult(this, matchSelector, instanceSelector ?? (instance => instance), continueWith);
        }

        /// <summary>
      /// Returns a tree of matching descendants
      /// </summary>
      /// <param name="matchSelector">Decision which child should be returned</param>
      /// <param name="instanceSelector">If set the returned value is set for GetDescendantsResult.Instance property.
      /// If not set the GetDescendantsResult.Instance property is the parent not of all items listet in
      /// GetDescendantsResult.Children property.</param>
      /// <returns></returns>
      /// <remarks></remarks>
        public GetDescendantsResult<TInstance, TChild> GetDescendants<TInstance, TChild>(Func<TChild, bool> matchSelector, Func<CompositeExpression, TInstance> instanceSelector = null)
where TInstance : CompositeExpression
where TChild : Expression
        {
            //A remplacer
            //return new GetDescendantsResult<TInstance, TChild>(this, matchSelector, instanceSelector ?? new Func<CompositeExpression, TInstance>(instance => { if (instance is TInstance) { return (TInstance)instance; } else { return instance.GetAncestor<TInstance>(); } }));
            return null;
        }

        /// <summary>
      /// Returns a tree of matching descendants
      /// </summary>
      /// <param name="continueWith">Callback to decide when stop building the tree</param>
      /// <param name="matchSelector">Decision which child should be returned</param>
      /// <param name="instanceSelector">If set the returned value is set for GetDescendantsResult.Instance property.
      /// If not set the GetDescendantsResult.Instance property is the parent not of all items listet in
      /// GetDescendantsResult.Children property.</param>
      /// <returns></returns>
      /// <remarks></remarks>
        public GetDescendantsResult<TInstance, TChild> GetDescendants<TInstance, TChild>(fnContinueWith continueWith, Func<TChild, bool> matchSelector, Func<CompositeExpression, TInstance> instanceSelector = null)
where TInstance : CompositeExpression
where TChild : Expression
        {
            return new GetDescendantsResult<TInstance, TChild>(this, matchSelector, instanceSelector ?? new Func<CompositeExpression, TInstance>(instance => { if (instance is TInstance) { return (TInstance)instance; } else { return instance.GetAncestor<TInstance>(); } }), continueWith);
        }

        /// <summary>
      /// Returns the identifiers of specified DatabaseObjectExpression-type within the current instance-type
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <returns></returns>
      /// <remarks></remarks>
        public GetDescendantsResult GetIdentifiers<T>() where T : DatabaseObjectExpression
        {
            return GetDescendants(expression => expression is IdentifierExpression && expression.GetAncestor<DatabaseObjectExpression>() is T, expression => GetType().IsAssignableFrom(expression.GetType()) ? expression : expression.GetAncestor(GetType()));
        }

        /// <summary>
      /// Default implementation of GetValue() supports a children block separated by _childrenSeparator embedded in
      /// the value of the current instance followed by _childBlockSeparator as prefix and _childBlockSeparator
      /// followed by _termination.ToString() as suffix. The suffix is only present if _termination is not null.
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        protected override string GetValue(Type executingType)
        {
            string myBaseResult = base.GetValue(executingType);

            if (GetType().IsAssignableFrom(executingType))
            {
                var sb = new st.StringBuilder(myBaseResult);

                if (_children.Count > 0)
                {
                    if (!string.IsNullOrEmpty(_childBlockSeparator) && sb.Length > 0)
                        sb.Append(_childBlockSeparator);
                    sb.Append(string.Join(_childrenSeparator ?? "", (from child in _children
                                                                     let childExpression = child.Value
                                                                     select childExpression).ToArray()));
                }
                if (_termination is not null)
                {
                    if (!string.IsNullOrEmpty(_childBlockSeparator) && sb.Length > 0)
                        sb.Append(_childBlockSeparator);
                    sb.Append(_termination.Value);
                }

                return sb.ToString();
            }
            else
            {
                return myBaseResult;
            }
        }

        /// <summary>
      /// Replaces a child; the method returns true, if the replacement was successful
      /// </summary>
        public virtual bool ReplaceChild(Expression oldChild, Expression newChild)
        {
            for (int index = 0, loopTo = _children.Count - 1; index <= loopTo; index++)
            {
                var child = _children[index];

                if (ReferenceEquals(oldChild, child))
                {
                    if (newChild is null)
                    {
                        _children.RemoveAt(index);
                    }
                    else
                    {
                        _children[index] = newChild;
                        SetParent(newChild, this);
                    }
                    SetParent(oldChild, null);

                    return true;
                }
            }

            return false;
        }

        /// <summary>
      /// Returns the identifiers of tables contained within the current instance-type
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public GetDescendantsResult TableIdentifiers
        {
            get
            {
                return GetIdentifiers<TableExpression>();
            }
        }

        protected Expression _termination;

    }
}