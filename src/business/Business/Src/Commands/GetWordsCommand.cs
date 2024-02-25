using Business.Src.Objects;
using MediatR;
using Objects.Src.Primitives;
using System.Linq.Expressions;
using System;
using System.Linq;
using Objects.Src.Dto;

namespace Business.Src.Commands
{
    public class GetWordsCommand : IRequest<SelectResult<WordDto>>
    {
        public GetWordsCommand(WordCategory? category, WordType? type, LanguageType? from, LanguageType? to, WordLevel? level) {
            BuildExpression(category, type, from, to, level); 
        }

        public Expression<Func<WordDto, bool>> Filter { get; private set; }

        private void BuildExpression(WordCategory? category, WordType? type, LanguageType? from, LanguageType? to, WordLevel? level)
        {
            Filter = x => true;

            if (category.HasValue) Filter = CombineExpressions(Filter, dto => dto.Category == category.Value);

            if (type.HasValue) Filter = CombineExpressions(Filter, dto => dto.Type == type.Value);

            if (from.HasValue) Filter = CombineExpressions(Filter, dto => dto.LanguageFrom == from.Value);

            if (to.HasValue) Filter = CombineExpressions(Filter, dto => dto.LanguageTo == to.Value);

            if(level.HasValue) Filter = CombineExpressions(Filter, dto => dto.Level == level.Value);
        }

        static Expression<Func<T, bool>> CombineExpressions<T>(
        Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }
}
