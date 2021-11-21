using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.Specification
{
    //Generic interfaces definitions
    //http://share.linqpad.net/rtex3r.linq
    public interface ISpecification<in T, in TVisitor> where TVisitor : ISpecificationVisitor<TVisitor, T>
    {
        bool IsSatisfiedBy(T item);
        void Accept(TVisitor visitor);
    }

    public interface ISpecificationVisitor<TVisitor, T> where TVisitor : ISpecificationVisitor<TVisitor, T>
    {

        void Visit(AndSpecification<T, TVisitor> spec);
        void Visit(OrSpecification<T, TVisitor> spec);
        void Visit(NotSpecification<T, TVisitor> spec);
    }
}
