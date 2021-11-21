namespace BuildingBlock.Specification
{
    public interface ISpecification<in T, in TVisitor> where TVisitor : ISpecificationVisitor<TVisitor, T>
    {
        void Accept(TVisitor visitor);

        bool IsSatisfiedBy(T item);
    }

    public interface ISpecificationVisitor<TVisitor, T> where TVisitor : ISpecificationVisitor<TVisitor, T>
    {
        void Visit(AndSpecification<T, TVisitor> spec);

        void Visit(NotSpecification<T, TVisitor> spec);

        void Visit(OrSpecification<T, TVisitor> spec);
    }
}
