namespace SilverlightDemo
{
    public class Category
    {
        public string Name { set; get; }
        public CategoryItem[] CategoryItems { set; get; }

        public Category()
        {
        }

        public Category(string name)
        {
            this.Name = name;
        }
    }
}
