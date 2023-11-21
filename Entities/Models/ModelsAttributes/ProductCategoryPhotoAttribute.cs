namespace Entities.Models.ModelsAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ProductCategoryPhotoAttribute : Attribute
    {
        public string PhotoUrl { get; set; }

        public ProductCategoryPhotoAttribute(string photoUrl)
        {
            PhotoUrl = photoUrl;
        }
    }
}
