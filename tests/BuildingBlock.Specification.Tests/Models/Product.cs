using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.Specification.Tests.Models
{
	public class Product
	{
		public Product()
		{

		}

		public Product(double price, string category, List<string> tags)
		{
			this.Price = price;
			this.Category = category;
			TagNames = tags;
		}

		public double Price { get; }
		public string Category { get; }
		public List<string> TagNames { get; }
	}

	public class EFProduct 
	{

		public EFProduct(Guid id, double price, string category, List<string> tags) 
		{
			this.Id = id;
			this.Price = price;
			this.Category = category;
			
			Tags = tags.Select(dl => new Tag() { Name = dl }).ToList();
		}

	public double Price { get; }
	public string Category { get; }


	public Guid Id { get; set; }
		public List<Tag> Tags { get; set; }
	}

	public class Tag
	{
		public string Name { get; set; }
	}
}
