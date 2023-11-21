﻿
using Entities.Models.ModelsAttributes;

namespace Entities.Models.Categories
{
    [ProductCategoryPhoto("data:image/jpeg;base64,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")]
    public class Shoes : Product
    {
        public string Type { get; set; }
        public string Brand { get; set; }
        public int Size { get; set; }
    }
}