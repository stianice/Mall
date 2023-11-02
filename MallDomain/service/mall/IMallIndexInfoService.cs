﻿using MallDomain.entity.mall.response;

namespace MallDomain.service.mall {
    public interface IMallIndexInfoService {
        // GetConfigGoodsForIndex 首页返回相关IndexConfig
        public Task<List<MallIndexConfigGoodsResponse>> GetConfigGoodsForIndex(int configType, int num);

    }
}