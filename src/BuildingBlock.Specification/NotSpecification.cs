using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.Specification
{
    public class NotSpecification<T, TVisitor> : ISpecification<T, TVisitor> where TVisitor : ISpecificationVisitor<TVisitor, T>
    {
        public ISpecification<T, TVisitor> Specification { get; }

        public NotSpecification(ISpecification<T, TVisitor> specification)
        {
            Specification = specification;
        }

        public void Accept(TVisitor visitor) => visitor.Visit(this);
        public bool IsSatisfiedBy(T obj) => !Specification.IsSatisfiedBy(obj);
    }
}
