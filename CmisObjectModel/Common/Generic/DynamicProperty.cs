using System;
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

namespace CmisObjectModel.Common.Generic
{
    /// <summary>
   /// Encapsulation of a single property defined by getter and/or setter method
   /// </summary>
   /// <typeparam name="TProperty"></typeparam>
   /// <remarks></remarks>
    public class DynamicProperty<TProperty>
    {

        protected DynamicProperty(string propertyName)
        {
        }
        public DynamicProperty(Func<TProperty> getter, string propertyName) : this(getter, null, propertyName)
        {
        }
        public DynamicProperty(Action<TProperty> setter, string propertyName) : this(null, setter, propertyName)
        {
        }
        public DynamicProperty(Func<TProperty> getter, Action<TProperty> setter, string propertyName)
        {
            _getter = getter;
            _setter = setter;
            PropertyName = propertyName;
        }

        private readonly Func<TProperty> _getter;

        /// <summary>
      /// Returns True if a getter is defined
      /// </summary>
        public virtual bool CanRead
        {
            get
            {
                return _getter is not null;
            }
        }

        /// <summary>
      /// Returns True if a setter is defined
      /// </summary>
        public virtual bool CanWrite
        {
            get
            {
                return _setter is not null;
            }
        }

        private readonly Action<TProperty> _setter;
        public readonly string PropertyName;

        /// <summary>
      /// Property-representation of getter and setter methods
      /// </summary>
        public virtual TProperty Value
        {
            get
            {
                if (_getter is null)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    return _getter.Invoke();
                }
            }
            set
            {
                if (_setter is null)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    _setter.Invoke(value);
                }
            }
        }

    }

    /// <summary>
   /// Encapsulation of a single property with one indexparameter defined by getter and/or setter method
   /// </summary>
   /// <typeparam name="TProperty"></typeparam>
   /// <remarks></remarks>
    public class DynamicProperty<TArg1, TProperty>
    {

        public DynamicProperty(Func<TArg1, TProperty> getter, string propertyName) : this(getter, null, propertyName)
        {
        }
        public DynamicProperty(Action<TArg1, TProperty> setter, string propertyName) : this(null, setter, propertyName)
        {
        }
        public DynamicProperty(Func<TArg1, TProperty> getter, Action<TArg1, TProperty> setter, string propertyName)
        {
            _getter = getter;
            _setter = setter;
            PropertyName = propertyName;
        }

        private readonly Func<TArg1, TProperty> _getter;

        /// <summary>
      /// Returns True if a getter is defined
      /// </summary>
        public bool CanRead
        {
            get
            {
                return _getter is not null;
            }
        }

        /// <summary>
      /// Returns True if a setter is defined
      /// </summary>
        public bool CanWrite
        {
            get
            {
                return _setter is not null;
            }
        }

        private readonly Action<TArg1, TProperty> _setter;
        public readonly string PropertyName;

        /// <summary>
      /// Property-representation of getter and setter methods
      /// </summary>
        public TProperty get_Value(TArg1 arg1)
        {
            if (_getter is null)
            {
                throw new NotImplementedException();
            }
            else
            {
                return _getter.Invoke(arg1);
            }
        }

        public void set_Value(TArg1 arg1, TProperty value)
        {
            if (_setter is null)
            {
                throw new NotImplementedException();
            }
            else
            {
                _setter.Invoke(arg1, value);
            }
        }

    }
}