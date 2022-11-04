using System;
using System.Collections.Generic;
using str = System.Text.RegularExpressions;
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
using ccg = CmisObjectModel.Common.Generic;

namespace CmisObjectModel.ServiceModel.Query
{
    /// <summary>
   /// Base class for a match in a query string
   /// </summary>
   /// <remarks></remarks>
    public class Expression
    {

        #region Constants
        public const string NotSetValue = "499280f8f2e94625bf5abd29fd2fcc56";
        #endregion

        #region Constructors
        public Expression(str.Match match, string groupName, int rank, int index)
        {
            Match = match;
            GroupName = groupName;
            Index = index;
            Rank = rank;
        }
        #endregion

        /// <summary>
      /// Returns True if the Value-property can be set to a custom defined value, otherwise False
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public virtual bool CanSetValue()
        {
            return true;
        }

        /// <summary>
      /// Searches for an ancestor of TParent-type
      /// </summary>
      /// <typeparam name="TParent"></typeparam>
      /// <returns></returns>
      /// <remarks></remarks>
        public TParent GetAncestor<TParent>() where TParent : CompositeExpression
        {
            var parent = Parent;

            while (parent is not null)
            {
                if (parent is TParent)
                {
                    return (TParent)parent;
                }
                else
                {
                    parent = parent.Parent;
                }
            }

            return null;
        }
        /// <summary>
      /// Searches for an ancestor of a type defined in ancestorTypes
      /// </summary>
      /// <param name="ancestorTypes"></param>
      /// <returns></returns>
      /// <remarks>Respects the inheritance of types</remarks>
        public CompositeExpression GetAncestor(params Type[] ancestorTypes)
        {
            if (ancestorTypes is null)
            {
                return Parent;
            }
            else
            {
                var parent = Parent;

                while (parent is not null)
                {
                    var parentType = parent.GetType();

                    foreach (Type ancestorType in ancestorTypes)
                    {
                        if (ancestorType is not null && ancestorType.IsAssignableFrom(parentType))
                        {
                            return parent;
                        }
                    }
                    parent = parent.Parent;
                }
            }

            return null;
        }

        /// <summary>
      /// Returns the next expression on the left side which doesn't belong to the same root
      /// </summary>
      /// <param name="expressions"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected internal Expression GetLeftExpression(List<Expression> expressions)
        {
            //A remplacer
            /*var root = Root;

            for (var index = index - 1; index >= 0; index -= 1)
            {
                var retVal = expressions[index];
                if (!ReferenceEquals(retVal.Root, root))
                    return retVal;
            }*/

            return null;
        }

        /// <summary>
      /// Returns the next expression on the right side which doesn't belong to the same root
      /// </summary>
      /// <param name="expressions"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected internal Expression GetRightExpression(List<Expression> expressions)
        {
            var root = Root;

            for (int index = Index + 1, loopTo = expressions.Count - 1; index <= loopTo; index++)
            {
                var retVal = expressions[index];
                if (!ReferenceEquals(retVal.Root, root))
                    return retVal;
            }

            return null;
        }

        /// <summary>
      /// Default implementation returns Match.Value
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        protected virtual string GetValue(Type executingType)
        {
            return Match.Value;
        }

        public readonly string GroupName;
        public readonly int Index;
        public readonly str.Match Match;

        protected CompositeExpression _parent;
        public CompositeExpression Parent
        {
            get
            {
                return _parent;
            }
        }
        protected void SetParent(CompositeExpression parent)
        {
            if (!ReferenceEquals(_parent, parent))
            {
                _parent = parent;
                ParentChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        protected static void SetParent(Expression instance, CompositeExpression parent)
        {
            if (instance is not null)
                instance.SetParent(parent);
        }
        public event EventHandler ParentChanged;

        public readonly int Rank;

        /// <summary>
      /// Returns the top level expression the current instance belongs to
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public Expression Root
        {
            get
            {
                return _parent is null ? this : _parent.Root;
            }
        }

        protected int? _sealResult;
        /// <summary>
      /// Returns null if the expression is accepted in the parsed query, otherwise the position of the match
      /// </summary>
      /// <param name="expressions"></param>
      /// <returns>Null: success, otherwise position of unexpected expression</returns>
      /// <remarks></remarks>
        public virtual int? Seal(List<Expression> expressions)
        {
            _sealed = true;
            return _sealResult;
        }

        protected bool _sealed = false;
        public bool Sealed
        {
            get
            {
                return _sealed;
            }
        }

        public sealed override string ToString()
        {
            return Value;
        }

        private ccg.Nullable<string> _value;
        public string Value
        {
            get
            {
                return _value.HasValue ? _value.Value : GetValue(GetType());
            }
            set
            {
                if (CanSetValue())
                {
                    if ((value ?? "") == NotSetValue)
                    {
                        _value = default;
                    }
                    else
                    {
                        _value = value;
                    }
                }
            }
        }

    }
}