using Entities.Models.ModelsAttributes;

namespace Entities.Models.Categories
{
    [ProductCategoryPhoto("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSA7wvkiV199A0JgAMsCNNnmDcb4JxdSmvmqutbelotEk9ayTk7FkZfJQltkFTbrkgdjw4&usqp=CAU")]
    public class Outerwears : Product
    {
        public string Type { get; set; }
        public int Height { get; set; }
        public string Brand { get; set; }
    }
}
