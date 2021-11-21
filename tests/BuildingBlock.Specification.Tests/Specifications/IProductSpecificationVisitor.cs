namespace BuildingBlock.Specification.Tests.Specifications
{
    using BuildingBlock.Specification.Tests.Models;

    public interface IProductSpecificationVisitor : ISpecificationVisitor<IProductSpecificationVisitor, Product>
    {
        void Visit(PriceGreaterThen spec);

        void Visit(PriceLesserThen spec);

        void Visit(ProductOfCategory spec);

        void Visit(ProductOfTag spec);
    }
}
