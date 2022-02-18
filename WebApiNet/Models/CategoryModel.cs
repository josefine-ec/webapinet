namespace WebApiNet.Models
{
    public class CategoryModel
    {
        public CategoryModel(string category)
        {
            Category = category;
        }

        public string Category { get; set; }
    }
}
