namespace BuildingBlock.Specification.Tests.Specifications
{
    using BuildingBlock.Specification.Tests.Models;

    public class ProductOfCategory : ISpecification<Product, IProductSpecificationVisitor>
    {
        public ProductOfCategory(string category)
        {
            Category = category;
        }

        public string Category { get; }

        public void Accept(IProductSpecificationVisitor visitor) => visitor.Visit(this);

        public bool IsSatisfiedBy(Product item) => item.Category.Equals(Category);
    }
}
