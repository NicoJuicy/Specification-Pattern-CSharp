namespace BuildingBlock.Specification
{
    public class NotSpecification<T, TVisitor> : ISpecification<T, TVisitor> where TVisitor : ISpecificationVisitor<TVisitor, T>
    {
        public NotSpecification(ISpecification<T, TVisitor> specification)
        {
            Specification = specification;
        }

        public ISpecification<T, TVisitor> Specification { get; }

        public void Accept(TVisitor visitor) => visitor.Visit(this);

        public bool IsSatisfiedBy(T obj) => !Specification.IsSatisfiedBy(obj);
    }
}
