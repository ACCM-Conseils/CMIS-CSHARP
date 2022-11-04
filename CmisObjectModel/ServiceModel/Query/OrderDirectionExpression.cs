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
    public class OrderDirectionExpression : Expression
    {

        #region Constructors
        public OrderDirectionExpression(str.Match match, string groupName, int rank, int index) : base(match, groupName, rank, index)
        {
        }
        #endregion

        public ccg.Nullable<string> Direction
        {
            get
            {
                var group = Match.Groups["Direction"];

                if (group is null || !group.Success)
                {
                    return default;
                }
                else
                {
                    return group.Value;
                }
            }
        }

        public ccg.Nullable<string> Nulls
        {
            get
            {
                var group = Match.Groups["Nulls"];

                if (group is null || !group.Success)
                {
                    return default;
                }
                else
                {
                    return group.Value;
                }
            }
        }

        /// <summary>
      /// Test if the instance is bound to a FieldExpression. If not the parsed query expression is not valid.
      /// </summary>
      /// <param name="expressions"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public override int? Seal(List<Expression> expressions)
        {
            if (!_sealed)
            {
                _sealResult = base.Seal(expressions);

                if (!_sealResult.HasValue && !(_parent is FieldExpression))
                    _sealResult = Match.Index;
            }

            return _sealResult;
        }

    }
}