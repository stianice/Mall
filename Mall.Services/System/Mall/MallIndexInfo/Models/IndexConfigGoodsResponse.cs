﻿namespace Mall.Services.Models
{
    public class IndexConfigGoodsResponse
    {
        public long GoodsId { get; set; }
        public string? GoodsName { get; set; }
        public string? GoodsIntro { get; set; }
        public string? GoodsCoverImg { get; set; }
        public int SellingPrice { get; set; }

        public string? Tag { get; set; }


    }
}
