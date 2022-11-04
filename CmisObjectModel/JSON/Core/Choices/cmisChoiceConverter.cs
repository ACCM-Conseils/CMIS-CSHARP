
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
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.JSON.Core.Choices
{
    namespace Generic
    {
        /// <summary>
      /// A Converter for all cmisChoice-types
      /// </summary>
      /// <typeparam name="TProperty"></typeparam>
      /// <typeparam name="TChoice"></typeparam>
      /// <remarks></remarks>
        public class cmisChoiceConverter<TProperty, TChoice> : Serialization.Generic.JavaScriptConverter<TChoice> where TChoice : CmisObjectModel.Core.Choices.Generic.cmisChoice<TProperty, TChoice>, new()
        {

            #region Constructors
            public cmisChoiceConverter() : base(new Serialization.Generic.DefaultObjectResolver<TChoice>())
            {
            }
            public cmisChoiceConverter(Serialization.Generic.ObjectResolver<TChoice> objectResolver) : base(objectResolver)
            {
            }
            #endregion

            protected override void Deserialize(SerializationContext context)
            {
                context.Object.DisplayName = Read(context.Dictionary, "displayName", context.Object.DisplayName);
                context.Object.Values = ReadArray(context.Dictionary, "value", context.Object.Values);
                context.Object.Choices = ReadArray(context, "choice", context.Object.Choices);
            }

            protected override void Serialize(SerializationContext context)
            {
                context.Add("displayName", context.Object.DisplayName);
                if (context.Object.Values is not null)
                {
                    if (context.Object.Values.Length == 1)
                    {
                        context.Add("value", context.Object.Values[0]);
                    }
                    else
                    {
                        context.Add("value", context.Object.Values);
                    }
                }
                if (context.Object.Choices is not null)
                {
                    WriteArray(context, context.Object.Choices, "choice");
                }
            }
        }
    }
}