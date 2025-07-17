// using System.Linq.Expressions;
//
// namespace TalentFlow.Application.Specifications;
//
// public interface ISpecificationBuilder<T>
// {
//     ISpecificationBuilder<T> Where(Expression<Func<T, bool>> expression);
//     ISpecificationBuilder<T> Include(Expression<Func<T, object>> expression);
//     ISpecificationBuilder<T> AsNoTracking();
// }
//
// internal class SpecificationBuilder<T>(Specification<T> specification) : ISpecificationBuilder<T>
// {
//     public ISpecificationBuilder<T> Where(Expression<Func<T, bool>> expression)
//     {
//         specification.AddWhere(expression);
//         return this;
//     }
//
//     public ISpecificationBuilder<T> Include(Expression<Func<T, object>> includeExpression)
//     {
//         specification.AddInclude(includeExpression);
//         return this;
//     }
//
//     public ISpecificationBuilder<T> AsNoTracking()
//     {
//         specification.SetAsNoTracking();
//         return this;
//     }
// }