namespace BuildingBlock.Specification.Tests
{
    using BuildingBlock.Specification.Extensions;
    using BuildingBlock.Specification.Tests.Models;
    using BuildingBlock.Specification.Tests.Specifications;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
	/// This is an implementation when the Specification has to be implemented with SQL
	/// </summary>
    public class SQLQueryProductSpecVisitor : IProductSpecificationVisitor
    {
        public string QueryFilter { get; private set; }

        public static string SpecToQuery(ISpecification<Product, IProductSpecificationVisitor> spec)
            => $"SELECT * FROM PRODUCTS WHERE {SpecToQueryFilter(spec)}";

        public void Visit(AndSpecification<Product, IProductSpecificationVisitor> spec)
            => QueryFilter = $"({SpecToQueryFilter(spec.Left)}) AND ({SpecToQueryFilter(spec.Right)})";

        public void Visit(NotSpecification<Product, IProductSpecificationVisitor> spec)
            => QueryFilter = $"NOT ({SpecToQueryFilter(spec)})";

        public void Visit(OrSpecification<Product, IProductSpecificationVisitor> spec)
            => QueryFilter = $"({SpecToQueryFilter(spec.Left)}) OR ({SpecToQueryFilter(spec.Right)})";

        public void Visit(PriceGreaterThen spec)
            => QueryFilter = $"PRICE >= {spec.Limit}";

        public void Visit(PriceLesserThen spec)
            => QueryFilter = $"PRICE <= {spec.Limit}";

        public void Visit(ProductOfCategory spec)
            => QueryFilter = $"CATEGORY = '{spec.Category}'";

        public void Visit(ProductOfTag spec)
        => QueryFilter = $"TAG LIKE {spec.Tag}";

        private static string SpecToQueryFilter(ISpecification<Product, IProductSpecificationVisitor> spec)
        {
            var visitor = new SQLQueryProductSpecVisitor();
            spec.Accept(visitor);
            return visitor.QueryFilter;
        }
    }

    [TestClass]
    public class SqlVisitorSpecificationTests
    {
        [TestMethod]
        public void TestSqlImplementation()
        {
            //assign
            var spec =
            new PriceGreaterThen(10)
                .And(new PriceLesserThen(20))
            .Or(new ProductOfCategory("Electronics"));

            //act
            var query = SQLQueryProductSpecVisitor.SpecToQuery(spec);

            //assert
            Assert.AreEqual("SELECT * FROM PRODUCTS WHERE ((PRICE >= 10) AND (PRICE <= 20)) OR (CATEGORY = 'Electronics')", query, "The sql between both sql queries should be the same");
        }
    }
}
