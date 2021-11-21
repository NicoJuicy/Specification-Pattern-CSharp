using BuildingBlock.Specification.Tests.Models;
using BuildingBlock.Specification.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.Specification.Tests.Specifications
{
    /// <summary>
    /// This is an implementation when the Specification has to be implemented with Expressions
    /// </summary>
    public class ExpressionQueryProductSpecVisitor : EFExpressionVisitor<EFProduct, IProductSpecificationVisitor, Product>, IProductSpecificationVisitor
    {
        public override Expression<Func<EFProduct, bool>> ConvertSpecToExpression(ISpecification<Product, IProductSpecificationVisitor> spec)
        {
            var visitor = new ExpressionQueryProductSpecVisitor();
            spec.Accept(visitor);
            return visitor.Expr;
        }

        public void Visit(PriceGreaterThen spec) => Expr = expr => expr.Price >= spec.Limit;
       

        public void Visit(PriceLesserThen spec) => Expr = expr => expr.Price < spec.Limit;
      
        public void Visit(ProductOfCategory spec)
        {
            string category = spec.Category;
            Expr = expr => expr.Category.Equals(category);
        }

        public void Visit(ProductOfTag spec)
        {
            string tag = spec.Tag;
            Expr = expr => expr.Tags.Any(dl => dl.Name.Equals(tag));
        }


    }
}
