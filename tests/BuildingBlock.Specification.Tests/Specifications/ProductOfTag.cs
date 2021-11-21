using BuildingBlock.Specification.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.Specification.Tests.Specifications
{
	public class ProductOfTag : ISpecification<Product, IProductSpecificationVisitor>
	{
		public ProductOfTag(string tag)
		{
			Tag = tag;
		}

		public string Tag{ get; }

		public bool IsSatisfiedBy(Product item) => item.TagNames.Any(dl => dl.Equals(Tag));

		public void Accept(IProductSpecificationVisitor visitor) => visitor.Visit(this);
	}


	
}
