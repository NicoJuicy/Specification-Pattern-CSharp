using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.Specification.Extensions
{
    public static class SpecificationExtensions
    {
        public static ISpecification<T, TVisitor> And<T, TVisitor>(this ISpecification<T, TVisitor> left, ISpecification<T, TVisitor> right) where TVisitor : ISpecificationVisitor<TVisitor, T>
            => new AndSpecification<T, TVisitor>(left, right);
        public static ISpecification<T, TVisitor> Or<T, TVisitor>(this ISpecification<T, TVisitor> left, ISpecification<T, TVisitor> right) where TVisitor : ISpecificationVisitor<TVisitor, T>
            => new OrSpecification<T, TVisitor>(left, right);
        public static ISpecification<T, TVisitor> Not<T, TVisitor>(this ISpecification<T, TVisitor> spec) where TVisitor : ISpecificationVisitor<TVisitor, T>
            => new NotSpecification<T, TVisitor>(spec);

        
    }
}
