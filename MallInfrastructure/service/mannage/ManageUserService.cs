﻿using MallDomain.entity.mall;
using MallDomain.entity.mannage.request;
using MallDomain.service.manage;
using Microsoft.EntityFrameworkCore;

namespace MallInfrastructure.service.mannage
{
    public class ManageUserService : IManageUserService
    {
        private readonly MallContext context;

        public ManageUserService(MallContext context)
        {
            this.context = context;
        }

        // GetMallUserInfoList 分页获取商城注册用户列表
        public async Task<(List<User> list, int total)> GetMallUserInfoList(UserSearch search)
        {
            int limit = search.PageInfo.PageSize;
            int offset = limit * (search.PageInfo.PageNumber - 1);


            int total = await context.Users.CountAsync();

            var userlist = await context.Users
                .OrderByDescending(u => u.CreateTime)
                .Skip(offset)
                .Take(limit)
                .AsNoTracking()
                .ToListAsync();

            return (userlist, total);
        }

        // LockUser 修改用户状态
        public async Task LockUser(List<long> ids)
        {
            //使用ef core批量操作新特性
           await context.Users
                .Where(u=>ids.Contains(u.UserId))
                .ExecuteUpdateAsync
                (s => s.SetProperty
                       (p=>p.LockedFlag,1));
     
        }
    }
}