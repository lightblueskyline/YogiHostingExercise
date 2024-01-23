using System.Runtime.CompilerServices;

namespace DependencyInjection.Models
{
    public class Repository : IRepository
    {
        //private Dictionary<string, Product> products;
        private IStorage storage;

        public Repository(IStorage storage)
        {
            //products = new Dictionary<string, Product>();
            this.storage = storage;
            new List<Product> {
                new Product { Name = "Women Shoes", Price = 99M },
                new Product { Name = "Skirts", Price = 29.99M },
                new Product { Name = "Pants", Price = 40.5M }
            }.ForEach(p => AddProduct(p));
        }

        //public IEnumerable<Product> Products => products.Values;
        public IEnumerable<Product> Products => storage.Items;

        //public Product this[string name] => products[name];
        public Product this[string name] => storage[name];

        //public void AddProduct(Product product) => products[product.Name] = product;
        public void AddProduct(Product product) => storage[product.Name] = product;

        //public void DeleteProduct(Product product) => products.Remove(product.Name);
        public void DeleteProduct(Product product) => storage.RemoveItem(product.Name);

        private string guid = System.Guid.NewGuid().ToString();
        public override string ToString()
        {
            return guid;
        }
    }
}
