﻿using FreeSql.Internal.CommonProvider;
using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ZhonTai.Admin.Core.Dto;

namespace ZhonTai.Admin.Core.Db
{
    public class TestMemberExpressionVisitor : ExpressionVisitor
    {
        public string MemberExpString;
        public bool Result { get; private set; }

        public static bool IsExists(Expression selector, Expression memberExp)
        {
            var visitor = new TestMemberExpressionVisitor { MemberExpString = memberExp.ToString() };
            visitor.Visit(selector);
            return visitor.Result;
        }
        protected override Expression VisitMember(MemberExpression node)
        {
            if (!Result && node.ToString() == MemberExpString) Result = true;
            return node;
        }
    }

    public static class FreeSqlExt
    {
        /// <summary>
        /// 执行忽略列查询
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="that"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static List<T1> ToListIgnore<T1>(this ISelect<T1> that, Expression<Func<T1, object>> selector)
        {
            if (selector == null) return that.ToList();
            var s0p = that as Select0Provider;
            var tb = s0p._tables[0];
            var parmExp = tb.Parameter ?? Expression.Parameter(tb.Table.Type, tb.Alias);
            var initExps = tb.Table.Columns.Values
                .Where(a => a.Attribute.IsIgnore == false)
                .Select(a => new
                {
                    exp = Expression.Bind(tb.Table.Properties[a.CsName], Expression.MakeMemberAccess(parmExp, tb.Table.Properties[a.CsName])),
                    ignored = TestMemberExpressionVisitor.IsExists(selector, Expression.MakeMemberAccess(parmExp, tb.Table.Properties[a.CsName]))
                })
                .Where(a => a.ignored == false)
                .Select(a => a.exp)
                .ToArray();
            var lambda = Expression.Lambda<Func<T1, T1>>(
                Expression.MemberInit(
                    Expression.New(tb.Table.Type),
                    initExps
                ),
                parmExp
            );
            return that.ToList(lambda);
        }

        /// <summary>
        /// 多列排序
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="that"></param>
        /// <param name="sortList"></param>
        /// <returns></returns>
        public static ISelect<T1> SortList<T1>(this ISelect<T1> that, List<SortInput>? sortList)
        {
            if (sortList != null && sortList.Count > 0)
            {
                sortList.ForEach(sort =>
                {
                    that = that.OrderByPropertyNameIf(sort.Order.HasValue, sort.PropName, sort.IsAscending.Value);
                });
            }

            return that;
        }
    }
}
