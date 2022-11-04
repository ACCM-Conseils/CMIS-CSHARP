using System;
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
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace CmisObjectModel.ServiceModel.Query
{
    /// <summary>
   /// Represents a constant expression (string or number)
   /// </summary>
   /// <remarks></remarks>
    public class ConstantExpression : Expression
    {

        #region Constructors
        public ConstantExpression(str.Match match, string groupName, int rank, int index) : base(match, groupName, rank, index)
        {
        }
        #endregion

        /// <summary>
      /// Returns the boolean value if the constant was detected as boolean, otherwise null
      /// </summary>
        public bool? BooleanConstant
        {
            get
            {
                var group = Match.Groups["BooleanConstant"];

                if (group is null || !group.Success)
                {
                    return default;
                }
                else
                {
                    return bool.Parse(group.Value);
                }
            }
        }

        /// <summary>
      /// Returns the date value if the constant was detected as date, otherwise null
      /// </summary>
        public DateTimeOffset? DateConstant
        {
            get
            {
                var group = Match.Groups["DateConstant"];

                if (group is null || !group.Success)
                {
                    return default;
                }
                else
                {
                    var groupHour = Match.Groups["Hour"];

                    if (group is null || !group.Success)
                    {
                        // only a date, no time information
                        return new DateTime(Conversions.ToInteger(Match.Groups["Year"].Value), Conversions.ToInteger(Match.Groups["Month"].Value), Conversions.ToInteger(Match.Groups["Day"].Value));
                    }
                    else
                    {
                        var groupMillisecond = Match.Groups["Millisecond"];
                        var groupOffset = Match.Groups["Offset"];
                        int millisecond = groupMillisecond is null || !groupMillisecond.Success ? 0 : Conversions.ToInteger(groupMillisecond.Value.PadRight(3, '0'));
                        var baseDate = new DateTime(Conversions.ToInteger(Match.Groups["Year"].Value), Conversions.ToInteger(Match.Groups["Month"].Value), Conversions.ToInteger(Match.Groups["Day"].Value), Conversions.ToInteger(Match.Groups["Hour"].Value), Conversions.ToInteger(Match.Groups["Minute"].Value), Conversions.ToInteger(Match.Groups["Second"].Value), millisecond, groupOffset is null || !groupOffset.Success ? DateTimeKind.Local : DateTimeKind.Utc);
                        var groupOffsetUtc = Match.Groups["OffsetUtc"];

                        if (groupOffset is null || !groupOffset.Success || groupOffsetUtc is not null && groupOffsetUtc.Success)
                        {
                            return baseDate;
                        }
                        else
                        {
                            int sign = Match.Groups["OffsetSign"].Value == "-" ? -1 : 1;
                            var offset = new TimeSpan(sign + Conversions.ToInteger(Match.Groups["OffsetHour"].Value), sign * Conversions.ToInteger(Match.Groups["OffsetMinute"].Value), 0);

                            return new DateTimeOffset(baseDate, offset);
                        }
                    }
                }
            }
        }

        /// <summary>
      /// Returns the double value if the constant was detected as number, otherwise null
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public double? NumberConstant
        {
            get
            {
                var group = Match.Groups["NumberConstant"];

                if (group is null || !group.Success)
                {
                    return default;
                }
                else
                {
                    return double.Parse(group.Value);
                }
            }
        }

        /// <summary>
      /// Returns the string value if the constant was detected as string, otherwise null
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccg.Nullable<string> StringConstant
        {
            get
            {
                var group = Match.Groups["StringConstant"];

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
      /// Returns null if the constant was not detected as string, otherwise
      /// following translations were made:
      /// '' => '
      /// In case of a contains() expression following additional translations were made:
      /// \\ => \, \' => ', \r => carriage return, \n => linefeed, \t => tabulator,
      /// \uXXXX => unicode character ChrW(XXXX)
      /// </summary>
        public ccg.Nullable<string> UnescapedStringConstant
        {
            get
            {
                var stringConstant = StringConstant;

                if (string.IsNullOrEmpty(stringConstant.Value))
                {
                    return stringConstant;
                }
                else
                {
                    var group = Match.Groups["IsContainsOperation"];

                    if (group is null || !group.Success)
                    {
                        return stringConstant.Value.Replace("''", "'");
                    }
                    else
                    {
                        System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(@"(\\u(?<Unicode>\d{1,4})|\\(?<EscapedChar>.)|'(?<EscapedChar>'))", System.Text.RegularExpressions.RegexOptions.Singleline);
                        System.Text.RegularExpressions.MatchEvaluator evaluator = match =>
                        {
                            System.Text.RegularExpressions.Group unicodeGroup = match.Groups["Unicode"];

                            if (unicodeGroup == null || !unicodeGroup.Success)
                            {
                                string escapedChar = match.Groups["EscapedChar"].Value;
                                switch (escapedChar)
                                {
                                    case "r":
                                        {
                                            return Microsoft.VisualBasic.Constants.vbCr;
                                        }

                                    case "n":
                                        {
                                            return Microsoft.VisualBasic.Constants.vbLf;
                                        }

                                    case "t":
                                        {
                                            return Microsoft.VisualBasic.Constants.vbTab;
                                        }

                                    default:
                                        {
                                            return escapedChar;
                                        }
                                }
                            }
                            else
                                return Strings.ChrW(System.Convert.ToInt32(unicodeGroup.Value)).ToString();
                        };
                        return regEx.Replace(stringConstant.Value, evaluator);
                    }
                }
            }
        }

    }
}