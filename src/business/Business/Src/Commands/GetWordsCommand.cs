using Business.Src.Objects;
using MediatR;
using Objects.Src;
using Objects.Src.Primitives;
using System.Linq.Expressions;
using System;
using System.Linq;

namespace Business.Src.Commands
{
    public class GetWordsCommand : IRequest<SelectResult<WordDto>>
    {
        public GetWordsCommand(WordCategory? category, WordType? type, LanguageType? from, LanguageType? to, WordLevel? level) {
            BuildExpression(category, type, from, to, level); 
        }

        public Expression<Func<WordDto, bool>> filter { get; private set; }

        private void BuildExpression(WordCategory? category, WordType? type, LanguageType? from, LanguageType? to, WordLevel? level)
        {
            filter = x => true;

            if (category.HasValue) filter = CombineExpressions(filter, dto => dto.Category == category.Value);

            if (type.HasValue) filter = CombineExpressions(filter, dto => dto.Type == type.Value);

            if (from.HasValue) filter = CombineExpressions(filter, dto => dto.LanguageFrom == from.Value);

            if (to.HasValue) filter = CombineExpressions(filter, dto => dto.LanguageTo == to.Value);

            if(level.HasValue) filter = CombineExpressions(filter, dto => dto.Level == level.Value);
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
