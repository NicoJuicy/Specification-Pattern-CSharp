namespace BuildingBlock.Specification.Tests.Specifications
{
    using BuildingBlock.Specification.Tests.Models;
    using System.Linq;

    public class ProductOfTag : ISpecification<Product, IProductSpecificationVisitor>
    {
        public ProductOfTag(string tag)
        {
            Tag = tag;
        }

        public string Tag { get; }

        public void Accept(IProductSpecificationVisitor visitor) => visitor.Visit(this);

        public bool IsSatisfiedBy(Product item) => item.TagNames.Any(dl => dl.Equals(Tag));
    }
}
