namespace DependencyInjection.Models
{
    public class Storage : IStorage
    {
        private Dictionary<string, Product> items = new Dictionary<string, Product>();

        public IEnumerable<Product> Items => items.Values;
        public Product this[string key]
        {
            get { return items[key]; }
            set { items[key] = value; }
        }
        public bool ContainsKey(string key) => items.ContainsKey(key);
        public void RemoveItem(string key) => items.Remove(key);
    }
}
