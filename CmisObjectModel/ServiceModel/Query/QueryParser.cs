using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
using cm = CmisObjectModel.Messaging;

namespace CmisObjectModel.ServiceModel.Query
{
    /// <summary>
   /// Simple parser to evaluate a query expression
   /// </summary>
   /// <remarks>see chapter 2.1.14.2.1 BNF Grammar of the cmis documentation
   /// Chapter 2.1.14 Query of the cmis documentation defines the grammar of sql supported in cmis.
   /// The defined grammar is a subset of sql grammar supported by this parser. The parser detects
   /// functions in general, not only CONTAINS(), SCORE(), IN_FOLDER() and IN_TREE(). It is the job
   /// of the server implementation to determine where an extension of the cmis standard is useful.
   /// String expressions within the CONTAINS() function MAY contain \' instead of '' to define a
   /// single quote. Outside the CONTAINS() function a single quote within a string expression MUST
   /// defined through ''.
   /// Supported literals are: strings, numerics, datetimes and booleans. Boolean expressions are
   /// complete case insensitive (for example tRUe is a valid notation). DateTime expressions MAY
   /// define only the date without any time information. The separator between year, month and
   /// day can be choosen free with the only limitation that it has to be exact one sign. If a time
   /// is defined the separator between hour, minute and second must be :. The definition of second
   /// and millisecond is optional (see const _dateTimeLiteral). Note: if the optional offset isn't
   /// defined the given time is interpreted as local time.
   /// </remarks>
    public class QueryParser
    {

        #region Constants
        private const string _dateTimeLiteral = "<datetime literal> ::= TIMESTAMP[<space>]<quote>YYYY<char>MM<char>DD[Thh:mm[:ss[.sss]][{Z|{+|-}hh:mm}]]<quote>";

        // processing rank
        private const int _signRank = 1 << 30;
        private const int _mathOperatorRank = _signRank >> 1;
        private const int _stringOperatorRank = _mathOperatorRank >> 1;
        private const int _compareOperatorRank = _stringOperatorRank >> 1;
        private const int _betweenOperatorRank = _compareOperatorRank >> 1;
        private const int _negationRank = _betweenOperatorRank >> 1;
        private const int _logicalOperatorRank = _negationRank >> 1;
        private const int _methodRank = _logicalOperatorRank >> 1;
        private const int _parenthesisRank = _methodRank >> 1;
        private const int _selectRank = _parenthesisRank >> 1;
        private const int _joinRank = _selectRank >> 1;
        private const int _conditionRank = _joinRank >> 1;
        private const int _orderByRank = _conditionRank >> 1;

        private const string shortAliasPattern = @"(\)\s*|\s+)(?<Alias>(((?<AliasName>""(""""|[^""])+""))|(?<AliasName>" + forbiddenIdentifierCharPattern + "+)))";
        private const string blankOrCloseParenthesisPrefixPattern = @"(?<=[\s\)])";
        private const string blankOrOpenParenthesisSuffixPattern = @"(?=(\z|[\s\(]))";
        private const string identifierStartCharPattern = "[A-Z_]";
        private const string forbiddenIdentifierCharPattern = @"[^\s\.,\<\>=\(\)\-\+]";
        private const string dateTimeLiteralPattern = @"(?<!\w)TIMESTAMP\s*'(?<DateConstant>(?<Year>\d{4}4{5}).(?<Month>\d{4}1,2{5}).(?<Day>\d{4}1,2{5})(T(?<Hour>\d{4}1,2{5}):(?<Minute>\d{4}1,2{5})(:(?<Second>\d{4}1,2{5})(\.(?<Millisecond>\d{4}1,3{5})\d*)?)?(?<Offset>((?<OffsetUtc>Z)|(?<OffsetSign>[\+\-])(?<OffsetHour>\d{4}2{5}):(?<OffsetMinute>\d{4}2{5})))?)?)\s*'";
        public static string sqlPattern = string.Format("(" + @"(?<=(\A|[\s,\(]))(?<Select>Select){2}" + "|" + @"(?<=[\s\)""])(?<SqlMainPart>(From|Where)){2}" + "|" + @"(?<=([+\-*\/\(\<\>=]\s*|\s+(and|in|is|like|on|or|select|where)\s+|\Aselect\s+))(?<Sign>[+\-])" + "|" + @"(?<=(select\s+|,\s*))(?<Identifier>((?<Prefix>{0}{1}*)\.)*\*)" + "|" + @"(?<=[\s,\(\<\>=]({0}{1}*\.)*{0}{1}*\s*\(\s*)(?<Identifier>\*)(?=\s*\))" + "|" + @"(?<MathOperator>([+\-*\/]|\<\<|\>\>))" + "|" + @"((?<CompareOperator>([\<\>]=|\<\>|[\=\<\>]))|(?<=\s)((?<Negation>not)\s+)?(?<CompareOperator>(like|in)){2}|(?<=\s)(?<CompareOperator>is)(\s+(?<Negation>not))?(?=(\z|\s)))" + "|" + @"(?<=\s)((?<Negation>not)\s+)?(?<BetweenOperator>between){2}" + "|" + "{3}(?<LogicalOperator>(or|and)){2}" + "|" + @"(?<=[\s\(])(?<Negation>not){2}" + "|" + @"{3}(?<Alias>As((\s*(?<AliasName>""(""""|[^""])+""))|\s+(?<AliasName>{1}+)))" + "|" + "(?<Separator>,)" + "|" + @"(?<StringOperator>\|\|)" + "|" + @"(?<CloseParenthesis>[\)])" + "|" + @"(?<OpenParenthesis>[\(])" + "|" + @"(?<=[\s,\(\<\>=](?<IsContainsOperation>)contains\s*\(\s*)(?<Constant>'(?<StringConstant>(\\\\|''|\\'|[^'])*)')" + "|" + @"(?<Constant>('(?<StringConstant>(''|[^'])*)'|(?<NumberConstant>\d+(\.\d+)?)|(?<=\s)null(?=(\s|\z)))|" + dateTimeLiteralPattern + @"|(?<!\w)(?<BooleanConstant>(true|false))(?!\w))" + "|" + @"{3}(?<Join>((inner|(left|right|full)(\s+outer)?)\s+)?join){2}" + "|" + @"{3}(?<Condition>(on|having|start\s+with|connect\s+by)){2}" + "|" + @"{3}(?<OrderBy>order(\s+siblings)?\s+by){2}" + "|" + @"{3}(?<OrderDirection>((?<Direction>(Asc|Desc)(ending)?)(\s+(?<Nulls>Nulls\s+(First|Last)))?|(?<Nulls>Nulls\s+(First|Last))))(?=(\z|[\s,]))" + "|" + @"(?<=[\s,\(\<\>=])(?<Method>((?<Prefix>{0}{1}*)\.)*(?<MethodName>{0}{1}*))(?=\s*\()" + "|" + @"(?<=[\s,\(\<\>=])((?<Any>any)\s+)?(?<Identifier>((?<Prefix>(""(""""|[^""])*""|{0}{1}*))\.)*(""(""""|[^""])*""|{0}{1}*))" + ")", identifierStartCharPattern, forbiddenIdentifierCharPattern, blankOrOpenParenthesisSuffixPattern, blankOrCloseParenthesisPrefixPattern, "{", "}");
        private static Dictionary<string, Func<str.Match, int, Expression>> _expressionFactories = new Dictionary<string, Func<str.Match, int, Expression>>() { { "Alias", CreateAliasExpression }, { "AliasName", null }, { "BetweenOperator", CreateBetweenOperatorExpression }, { "CloseParenthesis", CreateParenthesisExpression }, { "CompareOperator", CreateCompareOperatorExpression }, { "Condition", CreateConditionExpression }, { "Constant", CreateConstantExpression }, { "Direction", null }, { "Identifier", CreateIdentifierExpression }, { "IsContainsOperation", null }, { "Join", CreateJoinExpression }, { "LogicalOperator", CreateLogicalOperatorExpression }, { "MathOperator", CreateMathOperatorExpression }, { "Method", CreateMethodExpression }, { "Negation", CreateNegationExpression }, { "Nulls", null }, { "NumberConstant", null }, { "OpenParenthesis", CreateParenthesisExpression }, { "OrderBy", CreateOrderByExpression }, { "OrderDirection", CreateOrderDirectionExpression }, { "Prefix", null }, { "Select", CreateSelectExpression }, { "Separator", CreateSeparatorExpression }, { "Sign", CreateSignExpression }, { "SqlMainPart", CreateSqlMainPartExpression }, { "StringConstant", null }, { "StringOperator", CreateStringOperatorExpression } };
        #endregion

        #region Constructors
        public static ccg.Result<SelectExpression> CreateInstance(string q)
        {
            if (string.IsNullOrEmpty(q))
            {
                return cm.cmisFaultType.CreateInvalidArgumentException("q");
            }
            else
            {
                // parse the query expression
                var expressions = GetExpressions(q);

                // failure
                if (expressions is null)
                    return cm.cmisFaultType.CreateInvalidArgumentException("q");

                // create expression tree
                var groups = (from expression in expressions
                              where expression.Rank > 0
                              orderby expression.Index
                              group expression by expression.Rank into Group
                              let rank = Group.Key
                              orderby rank descending
                              select Group).ToArray();
                foreach (IEnumerable<Expression> group in groups)
                {
                    var groupExpressions = group is Expression[]? (Expression[])group : group.ToArray();

                    for (int groupExpressionsIndex = groupExpressions.Length - 1; groupExpressionsIndex >= 0; groupExpressionsIndex -= 1)
                        groupExpressions[groupExpressionsIndex].Seal(expressions);
                }

                // search for result
                var retVal = GetSelectExpression(expressions);

                // search for the first error if any
                if (retVal is null)
                {
                    // query doesn't start with '[\(]*Select'-Pattern
                    var httpStatusCode = Common.CommonFunctions.ToHttpStatusCode(cm.enumServiceException.invalidArgument);
                    var fault = new cm.cmisFaultType(httpStatusCode, cm.enumServiceException.invalidArgument, "Unexpected expression in parameter 'q'");
                    return fault.ToFaultException();
                }
                else
                {
                    for (int expressionIndex = 0, loopTo = expressions.Count - 1; expressionIndex <= loopTo; expressionIndex++)
                    {
                        var expression = expressions[expressionIndex];
                        var failureIndex = expression.Seal(expressions) ?? (ReferenceEquals(expression, retVal) || expression.Parent is not null ? default(int?) : expression.Match.Index);
                        if (failureIndex.HasValue)
                        {
                            var httpStatusCode = Common.CommonFunctions.ToHttpStatusCode(cm.enumServiceException.invalidArgument);
                            var fault = new cm.cmisFaultType(httpStatusCode, cm.enumServiceException.invalidArgument, "Unexpected expression in parameter 'q' at position " + failureIndex.Value);
                            return fault.ToFaultException();
                        }
                    }
                }

                return retVal;
            }
        }

        /// <summary>
      /// Splits the query term in expressions
      /// </summary>
      /// <param name="q"></param>
      /// <returns>A list of expressions or nothing if a found match cannot be converted into an expression-instance</returns>
      /// <remarks></remarks>
        internal static List<Expression> GetExpressions(string q)
        {
            var regEx = new str.Regex(sqlPattern, str.RegexOptions.ExplicitCapture | str.RegexOptions.IgnoreCase | str.RegexOptions.Multiline);
            var shortAliasRegEx = new str.Regex(shortAliasPattern, str.RegexOptions.ExplicitCapture | str.RegexOptions.IgnoreCase | str.RegexOptions.Multiline);
            var retVal = new List<Expression>();
            int index = 0;
            Expression expression = null;

            // parse the query expression
            foreach (str.Match match in regEx.Matches(q))
            {
                expression = CreateExpression(match, index, expression, q, shortAliasRegEx);

                if (expression is null)
                {
                    return null;
                }
                else
                {
                    retVal.Add(expression);
                    index += 1;
                }
            }

            return retVal;
        }
        #endregion

        #region Expression-factories
        private static Expression CreateExpression(str.Match match, int index, Expression lastExpression, string q, str.Regex shortAliasRegEx)
        {
            foreach (KeyValuePair<string, Func<str.Match, int, Expression>> de in _expressionFactories)
            {
                if (de.Value is not null)
                {
                    var group = match.Groups[de.Key];
                    if (group is not null && group.Success)
                    {
                        if (string.Compare(de.Key, "Identifier") == 0 && (lastExpression is IdentifierExpression || lastExpression is ParenthesisExpression && lastExpression.Match.Value == ")" || lastExpression is ConstantExpression))
                        {
                            return CreateExpression(shortAliasRegEx.Match(q, match.Index - 1), index, lastExpression, q, shortAliasRegEx);
                        }
                        else
                        {
                            return de.Value(match, index);
                        }
                    }
                }
            }

            return null;
        }

        private static Expression CreateAliasExpression(str.Match match, int index)
        {
            return new AliasExpression(match, "Alias", 0, index);
        }

        private static Expression CreateBetweenOperatorExpression(str.Match match, int index)
        {
            return new BetweenExpression(match, "BetweenOperator", _betweenOperatorRank, index);
        }

        private static Expression CreateCompareOperatorExpression(str.Match match, int index)
        {
            return new OperatorExpression(match, "CompareOperator", _compareOperatorRank, index, true);
        }

        private static Expression CreateConditionExpression(str.Match match, int index)
        {
            return new WhereExpression(match, "Condition", _conditionRank, index);
        }

        private static Expression CreateConstantExpression(str.Match match, int index)
        {
            return new ConstantExpression(match, "Constant", 0, index);
        }

        private static Expression CreateIdentifierExpression(str.Match match, int index)
        {
            return new IdentifierExpression(match, "Identifier", 0, index);
        }

        private static Expression CreateJoinExpression(str.Match match, int index)
        {
            return new JoinExpression(match, "Join", _joinRank, index);
        }

        private static Expression CreateLogicalOperatorExpression(str.Match match, int index)
        {
            return new OperatorExpression(match, "LogicalOperator", _logicalOperatorRank + (string.Compare(match.Value, "or", true) == 0 ? 0 : 1), index, true);
        }

        private static Expression CreateMathOperatorExpression(str.Match match, int index)
        {
            int offset;
            switch (match.Value ?? "")
            {
                case "-":
                case "+":
                    {
                        offset = 0;
                        break;
                    }
                case "*":
                case "/":
                    {
                        offset = 1;
                        break;
                    }

                default:
                    {
                        offset = 2;
                        break;
                    }
            }
            return new OperatorExpression(match, "MathOperator", _mathOperatorRank + offset, index, true);
        }

        private static Expression CreateMethodExpression(str.Match match, int index)
        {
            return new MethodExpression(match, "Method", _methodRank, index);
        }

        private static Expression CreateNegationExpression(str.Match match, int index)
        {
            return new OperatorExpression(match, "Negation", _negationRank, index, false);
        }

        private static Expression CreateOrderByExpression(str.Match match, int index)
        {
            return new OrderByExpression(match, "OrderBy", _orderByRank, index);
        }

        private static Expression CreateOrderDirectionExpression(str.Match match, int index)
        {
            return new OrderDirectionExpression(match, "OrderDirection", 0, index);
        }

        private static Expression CreateParenthesisExpression(str.Match match, int index)
        {
            int rank = match.Value == "(" ? _parenthesisRank : 0;
            return new ParenthesisExpression(match, (rank == 0 ? "Close" : "Open") + "Parenthesis", rank, index);
        }

        private static Expression CreateSelectExpression(str.Match match, int index)
        {
            return new SelectExpression(match, "Select", _selectRank, index);
        }

        private static Expression CreateSeparatorExpression(str.Match match, int index)
        {
            return new Expression(match, "Separator", 0, index);
        }

        private static Expression CreateSignExpression(str.Match match, int index)
        {
            return new SignExpression(match, _signRank, index);
        }

        private static Expression CreateSqlMainPartExpression(str.Match match, int index)
        {
            switch (match.Value.ToLowerInvariant() ?? "")
            {
                case "from":
                    {
                        return new FromExpression(match, "SqlMainPart", 0, index);
                    }
                case "where":
                    {
                        return new WhereExpression(match, "SqlMainPart", 0, index);
                    }

                default:
                    {
                        return null;
                    }
            }
        }

        private static Expression CreateStringOperatorExpression(str.Match match, int index)
        {
            return new OperatorExpression(match, "StringOperator", _stringOperatorRank, index, true);
        }
        #endregion

        /// <summary>
      /// Returns the main select expression of the query or null
      /// </summary>
      /// <param name="expressions"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        private static SelectExpression GetSelectExpression(List<Expression> expressions)
        {
            if (expressions.Count == 0)
            {
                return null;
            }
            else
            {
                var root = expressions[0].Root;

                // skip embedding parenthesis
                while (root is ParenthesisExpression)
                {
                    {
                        var withBlock = (ParenthesisExpression)root;
                        var children = withBlock.GetChildren(expression => true);
                        root = children is null || children.Length != 1 ? null : children[0];
                    }
                }
                return root as SelectExpression;
            }
        }

    }
}