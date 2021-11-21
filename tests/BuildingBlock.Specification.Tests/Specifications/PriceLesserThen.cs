using BuildingBlock.Specification.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.Specification.Tests.Specifications
{
	public class PriceLesserThen : ISpecification<Product, IProductSpecificationVisitor>
	{
		public PriceLesserThen(double limit)
		{
			Limit = limit;
		}

		public double Limit { get; }

		public bool IsSatisfiedBy(Product item) => item.Price < Limit;

		public void Accept(IProductSpecificationVisitor visitor) => visitor.Visit(this);
	}
}
