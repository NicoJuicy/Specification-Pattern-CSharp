using BuildingBlock.Specification.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.Specification.Tests.Specifications
{
	public interface IProductSpecificationVisitor : ISpecificationVisitor<IProductSpecificationVisitor, Product>
	{
		void Visit(PriceGreaterThen spec);
		void Visit(PriceLesserThen spec);
		void Visit(ProductOfCategory spec);
		void Visit(ProductOfTag spec);
	}
}
