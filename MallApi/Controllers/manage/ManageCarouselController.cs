﻿using MallDomain.entity.common.request;
using MallDomain.entity.common.response;
using MallDomain.entity.mannage.request;
using MallDomain.service.manage;
using MallDomain.utils.validator;
using Microsoft.AspNetCore.Mvc;

namespace MallApi.Controllers.mannage
{
    [Route("manage-api/v1")]
    [ApiController]
    public class ManageCarouselController : ControllerBase
    {
        private readonly IManageCarouselService mallCarouselService;

        public ManageCarouselController(IManageCarouselService mallCarouselService)
        {
            this.mallCarouselService = mallCarouselService;
        }

        [HttpPost("carousels")]
        public async Task<Result> CreateCarousel([FromBody] CarouselAddParam carouselAddParam)
        {
            var rs = await ValidatorFactory.CreateValidator(carouselAddParam)!.ValidateAsync(carouselAddParam);

            if (!rs.IsValid)
            {
                return Result.FailWithMessage("参数不合法");
            }
            await mallCarouselService.CreateCarousel(carouselAddParam);
            return Result.OkWithMessage("创建轮播图成功");
        }
        [HttpDelete("carousels")]
        public async Task<Result> DeleteCarousel([FromBody] IdsReq ids)
        {
            await mallCarouselService.DeleteCarousel(ids.Ids);
            return Result.Ok();
        }


        [HttpPut("carousels")]
        public async Task<Result> UpdateCarousel(CarouselUpdateParam req)
        {
            var rs = await ValidatorFactory.CreateValidator(req)!.ValidateAsync(req);

            if (!rs.IsValid)
            {
                return Result.FailWithMessage("参数不合法");
            }
            await mallCarouselService.UpdateCarousel(req);

            return Result.OkWithMessage("更新成功");
        }
        [HttpGet("carousels/{id}")]
        public async Task<Result> FindCarousel(long id)
        {
            var cas = await mallCarouselService.GetCarousel(id);
            return Result.OkWithData(cas);
        }
        [HttpGet("carousels")]
        public async Task<Result> GetCarouselList([FromQuery] PageInfo csh)
        {
            var (list, total) = await mallCarouselService.GetCarouselInfoList(csh);
            return Result.OkWithDetailed(new PageResult()
            {
                List = list,
                CurrPage = csh.PageNumber,
                TotalCount = total,
                PageSize = csh.PageSize,
                TotalPage = (int)Math.Ceiling((double)total / csh.PageSize)
            }, "获取成功");


        }

    }


}
