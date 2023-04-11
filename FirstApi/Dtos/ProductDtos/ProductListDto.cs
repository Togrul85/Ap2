﻿using FirstApi.Models;

namespace FirstApi.Dtos.ProductDtos
{
    public class ProductListDto
    {
        public int TotalCount { get; set; }

        public List<ProductListItemDto> Items { get; set; }

    }
}
