namespace BuildingBlock.Specification.Tests
{
    using BuildingBlock.Specification;
    using BuildingBlock.Specification.Extensions;
    using BuildingBlock.Specification.Tests.Models;
    using BuildingBlock.Specification.Tests.Specifications;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class EFExpressionVisitorSpecificationTests
    {
        [TestMethod]
        public void SimpleEFImplementation()
        {
            //assign
            var repo = new ProductRepository();

            var spec = new PriceGreaterThen(10);

            //act
            var result = repo.GetProducts(spec).ToList();

            Assert.IsTrue(result.All(p => spec.IsSatisfiedBy(p)), $"Not all products were matched category or price by the specification");
        }

        [TestMethod]
        public void TestAndImplementation()
        {
            //assign
            var repo = new ProductRepository();

            var spec = new PriceGreaterThen(10)
                .And(new PriceLesserThen(20));

            //act
            var result = repo.GetProducts(spec).ToList();

            Assert.IsTrue(result.All(p => spec.IsSatisfiedBy(p)), $"Not all products were matched category or price by the specification");
        }

        [TestMethod]
        public void TestDeepEFImplementation()
        {

            //assign
            var repo = new ProductRepository();

            var spec = new ProductOfTag("Test-1");


            //act
            var result = repo.GetProducts(spec).ToList();

            Assert.IsTrue(result.All(p => spec.IsSatisfiedBy(p)), $"Not all products were matched category or price by the specification");
        }

        [TestMethod]
        public void TestDeepOrEFImplementation()
        {
            //assign
            var repo = new ProductRepository();

            var spec = new ProductOfTag("Test-1")
             .Or(new ProductOfTag("Test-2"));

            //act
            var result = repo.GetProducts(spec).ToList();

            Assert.IsTrue(result.All(p => spec.IsSatisfiedBy(p)), $"Not all products were matched category or price by the specification");
        }

        [TestMethod]
        public void TestEFHelperImplementation()
        {
            //assign
            var repo = new ProductRepository();

            //  ISpecification< Product, IProductSpecificationVisitor> spec = null;/* new PriceGreaterThen(10)
            //    .And(new PriceLesserThen(20))
            //.Or(new ProductOfCategory("Electronics"));*/

            var helper = new Helpers.EnumerableSpecificationHelper<Product, IProductSpecificationVisitor>();
            helper.Include(new ProductOfTag("Test-1"));
            helper.Include(new ProductOfTag("Test-2"));
            helper.Apply();

            helper.Include(new ProductOfCategory("Electronics"));
            helper.Apply();

            var spec = helper.GetSpecification();
            //act
            var result = repo.GetProducts(spec).ToList();

            Assert.IsTrue(result.All(p => spec.IsSatisfiedBy(p)), $"Not all products were matched category or price by the specification");
        }

        [TestMethod]
        public void TestEFImplementation()
        {
            //assign
            var repo = new ProductRepository();

            var spec = new PriceGreaterThen(10)
                .And(new PriceLesserThen(20))
            .Or(new ProductOfCategory("Electronics"));

            //act
            var result = repo.GetProducts(spec).ToList();

            Assert.IsTrue(result.All(p => spec.IsSatisfiedBy(p)), $"Not all products were matched category or price by the specification");
        }

        [TestMethod]
        public void TestNotImplementation()
        {
            //assign
            var repo = new ProductRepository();

            var spec = new PriceGreaterThen(10).Not();

            //act
            var result = repo.GetProducts(spec).ToList();

            Assert.IsTrue(result.All(p => spec.IsSatisfiedBy(p)), $"Not all products were matched category or price by the specification");
        }

        [TestMethod]
        public void TestOrImplementation()
        {
            //assign
            var repo = new ProductRepository();

            var spec = new PriceGreaterThen(10)
                .Or(new PriceLesserThen(20));

            //act
            var result = repo.GetProducts(spec).ToList();

            Assert.IsTrue(result.All(p => spec.IsSatisfiedBy(p)), $"Not all products were matched category or price by the specification");
        }
    }

    public class ProductRepository
    {
        static ProductRepository()
        {
            Repo = new List<EFProduct>();
            Repo.Add(new EFProduct(Guid.NewGuid(), 5, "Electronics", new List<string>() { "Test-1", "Test-2", "Test-3" }));
            Repo.Add(new EFProduct(Guid.NewGuid(), 500, "Electronics", new List<string>() { "Test-1" }));
            Repo.Add(new EFProduct(Guid.NewGuid(), 10, "Electronics", new List<string>() { "Test-1", "Test-2", "Test-3" }));
            Repo.Add(new EFProduct(Guid.NewGuid(), 15, "Electronics", new List<string>() { "Test-1", "Test-2", "Test-3" }));
            Repo.Add(new EFProduct(Guid.NewGuid(), 20, "Electronics", new List<string>() { "Test-1", "Test-2", "Test-3" }));
            Repo.Add(new EFProduct(Guid.NewGuid(), 10, "Games", new List<string>() { "Test-1", "Test-2", "Test-3" }));
            Repo.Add(new EFProduct(Guid.NewGuid(), 15, "Games", new List<string>() { "Test-1", "Test-2", "Test-3" }));
            Repo.Add(new EFProduct(Guid.NewGuid(), 20, "Games", new List<string>() { "Test-1", "Test-2", "Test-3" }));
        }

        public static List<EFProduct> Repo { get; set; }

        public IEnumerable<Product> GetProducts(ISpecification<Product, IProductSpecificationVisitor> spec)
        {
            var visitor = new ExpressionQueryProductSpecVisitor();
            spec.Accept(visitor);
            var expression = visitor.Expr;


            foreach (var efProduct in Repo.AsQueryable().Where(expression).ToList())
            {
                var product = ConvertPersistenceToDomain(efProduct);
                yield return product;

            }
        }

        private Product ConvertPersistenceToDomain(EFProduct entity)
        {
            return new Product(entity.Price, entity.Category, entity.Tags.Select(dl => dl.Name).ToList());
        }
    }
}
