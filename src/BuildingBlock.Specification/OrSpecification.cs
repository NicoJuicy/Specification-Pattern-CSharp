namespace BuildingBlock.Specification
{
    public class OrSpecification<T, TVisitor> : ISpecification<T, TVisitor> where TVisitor : ISpecificationVisitor<TVisitor, T>
    {
        public OrSpecification(ISpecification<T, TVisitor> left, ISpecification<T, TVisitor> right)
        {
            this.Left = left;
            this.Right = right;
        }

        public ISpecification<T, TVisitor> Left { get; }

        public ISpecification<T, TVisitor> Right { get; }

        public void Accept(TVisitor visitor) => visitor.Visit(this);

        public bool IsSatisfiedBy(T obj) => Left.IsSatisfiedBy(obj) || Right.IsSatisfiedBy(obj);
    }
}
