﻿using System;
using System.Linq.Expressions;

namespace AutoQueryable.Core.CriteriaFilters
{
    public class QueryableTypeFilter : IQueryableTypeFilter
    {
        public QueryableTypeFilter(IQueryableFilter queryableFilter, Func<Expression, Expression, Expression> filter)
        {
            this.QueryableFilter = queryableFilter;
            this.Filter = filter;
        }
       
        public Func<Expression, Expression, Expression> Filter { get; set; }
        public IQueryableFilter QueryableFilter { get; set; }
    }
}