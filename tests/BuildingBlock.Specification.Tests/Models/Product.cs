namespace BuildingBlock.Specification.Tests.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class EFProduct
    {
        public EFProduct(Guid id, double price, string category, List<string> tags)
        {
            this.Id = id;
            this.Price = price;
            this.Category = category;

            Tags = tags.Select(dl => new Tag() { Name = dl }).ToList();
        }

        public string Category { get; }

        public Guid Id { get; set; }

        public double Price { get; }

        public List<Tag> Tags { get; set; }
    }

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

        public string Category { get; }

        public double Price { get; }

        public List<string> TagNames { get; }
    }

    public class Tag
    {
        public string Name { get; set; }
    }
}
