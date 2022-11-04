using Microsoft.VisualBasic.CompilerServices;
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
   /// allows nothing as a valid value
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <remarks></remarks>
    public struct Nullable<T>
    {

        public Nullable(T value)
        {
            Value = value;
            HasValue = true;
        }

        public bool HasValue;
        public T Value;

        public override bool Equals(object obj)
        {
            if (obj is T || obj is null)
            {
                return Equals(Conversions.ToGenericParameter<T>(obj));
            }
            else if (obj is Nullable<T>)
            {
                return Equals((Nullable<T>)obj);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(T obj)
        {
            if (!HasValue)
            {
                return false;
            }
            else if (Value is null)
            {
                return obj is null || obj.Equals(Value);
            }
            else
            {
                return Value.Equals(obj);
            }
        }

        public bool Equals(Nullable<T> obj)
        {
            if (HasValue != obj.HasValue)
            {
                return false;
            }
            else
            {
                return !HasValue || Equals(obj.Value);
            }
        }

        public override string ToString()
        {
            if (!HasValue)
            {
                return GetType().Name + ": value not set";
            }
            else
            {
                return Value is null ? null : Value.ToString();
            }
        }

        public static implicit operator Nullable<T>(T value)
        {
            return new Nullable<T>(value);
        }
        public static implicit operator T(Nullable<T> value)
        {
            return value.Value;
        }
    }
}