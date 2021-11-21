using BuildingBlock.Specification.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.Specification.Tests.Specifications
{
	public class ProductOfCategory : ISpecification<Product, IProductSpecificationVisitor>
	{
		public ProductOfCategory(string category)
		{
			Category = category;
		}

		public string Category { get; }
		public bool IsSatisfiedBy(Product item) => item.Category.Equals(Category);

		public void Accept(IProductSpecificationVisitor visitor) => visitor.Visit(this);
	}
}
