namespace SilverlightDemo
{
    public class CategoryItem
    {
        public string Code { set; get; }
        public string Explain { set; get; }
        public string ID { set; get; }
        public string Source { set; get; }
        public string XAML { set; get; }

        public CategoryItem()
        {
        }

        public CategoryItem(string name, string xaml)
        {
            this.ID = name;
            this.XAML = xaml;
        }
    }
}
