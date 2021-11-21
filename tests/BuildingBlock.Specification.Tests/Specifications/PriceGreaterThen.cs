namespace BuildingBlock.Specification.Tests.Specifications
{
    using BuildingBlock.Specification.Tests.Models;

    public class PriceGreaterThen : ISpecification<Product, IProductSpecificationVisitor>
    {
        public PriceGreaterThen(double limit)
        {
            Limit = limit;
        }

        public double Limit { get; }

        public void Accept(IProductSpecificationVisitor visitor) => visitor.Visit(this);

        public bool IsSatisfiedBy(Product item) => item.Price >= Limit;
    }
}
