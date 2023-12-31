﻿
using Mall.Common.Result;
using Mall.Repository;
using Mall.Repository.Enums;
using Mall.Repository.Models;
using Mall.Services.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Mall.Services
{
    public class ManageOrderService
    {
        private readonly MallContext context;

        public ManageOrderService(MallContext context)
        {
            this.context = context;
        }
        // CheckDone 修改订单状态为配货成功
        public async Task Ch_eckDone(List<long> ids)
        {

            var orders = await context.Orders
                 .Where(w => ids.Contains(w.OrderId))
                 .ToListAsync();
            if (orders.Count() == 0) throw ResultException.FailWithMessage("未查询到订单");



            foreach (var order in orders)
            {
                //订单存在未支付
                if (order.OrderStatus == OrderStatusEnum.ORDER_PRE_PAY.Code())
                    throw ResultException.FailWithMessage("存在未支付订单，不能配货");
            }





            await context.Orders
                .Where(i => ids.Contains(i.OrderId))
                .ExecuteUpdateAsync(s => s
                .SetProperty(p => p.OrderStatus, 2)
                .SetProperty(p => p.UpdateTime, DateTime.Now));
        }

        // CheckOut 出库
        public async Task CheckOut(List<long> ids)
        {

            var orders = await context.Orders
                 .Where(w => ids.Contains(w.OrderId))
                 .ToListAsync();
            if (orders.Count() == 0) throw ResultException.FailWithMessage("未查询到订单");




            //订单存在未支付


            foreach (var order in orders)
            {
                if (order.OrderStatus == OrderStatusEnum.ORDER_PAID.Code())
                    if (order.OrderStatus == OrderStatusEnum.ORDER_PACKAGED.Code())
                        throw ResultException.FailWithMessage("未支付或未配货状态，不能出库");
            }





            await context.Orders
                .Where(i => ids.Contains(i.OrderId))
                .ExecuteUpdateAsync(s => s
                .SetProperty(p => p.OrderStatus, 3)
                .SetProperty(p => p.UpdateTime, DateTime.Now));
        }


        public async Task CloseOrder(List<long> ids)
        {
            var orders = await context.Orders
                 .Where(w => ids.Contains(w.OrderId))
                 .ToListAsync();
            if (orders.Count() == 0) throw ResultException.FailWithMessage("未查询到订单");






            //订单存在已支付支付 未出库 未配货
            foreach (var order in orders)
            {
                //如果已支付
                if (order.OrderStatus == OrderStatusEnum.ORDER_PAID.Code())

                {
                    //已经配货 不能关闭
                    if (order.OrderStatus == OrderStatusEnum.ORDER_PACKAGED.Code())
                    {
                        throw ResultException.FailWithMessage("订单已支付未出库，不能关闭");
                    }

                }
            }

            await context.Orders
           .Where(i => ids.Contains(i.OrderId))
           .ExecuteUpdateAsync(s => s
           .SetProperty(p => p.OrderStatus, -3)
           .SetProperty(p => p.UpdateTime, DateTime.Now));
        }

        public async Task<NewBeeMallOrderDetailVO> GetMallOrder(long id)
        {

            var order = await context.Orders
                 .SingleOrDefaultAsync(w => w.OrderId == id)
                 ?? throw ResultException.FailWithMessage("未查询到订单");

            //获取订单下的条目
            var orderItems = await context.OrderItems
                         .Where(w => w.OrderId == order.OrderId)
                         .AsNoTracking()
                         .ToListAsync();

            if (orderItems.Count() == 0) throw ResultException.FailWithMessage("订单异常");

            var newBeeMallOrderDetailVO = order.Adapt<NewBeeMallOrderDetailVO>();
            newBeeMallOrderDetailVO.NewBeeMallOrderItemVOS = orderItems.Adapt<List<NewBeeMallOrderItemVO>>();

            string statusStr = MallOrderStatusExtensions.GetNewBeeMallOrderStatusEnumByStatus(order.OrderStatus);
            string payTapStr = MallOrderStatusExtensions.GetNewBeeMallOrderStatusEnumByStatus(order.PayStatus);
            newBeeMallOrderDetailVO.OrderStatusString = statusStr;
            newBeeMallOrderDetailVO.PayTypeString = payTapStr;

            return newBeeMallOrderDetailVO;
        }

        public async Task<(List<Order>, long)> GetMallOrderInfoList(PageInfo info, string? orderNo, string? orderStatus)
        {
            var limit = info.PageSize;
            var offset = limit * (info.PageNumber - 1);


            var query = context.Orders.AsQueryable();

            if (!string.IsNullOrEmpty(orderNo))
            {
                query = query.Where(w => w.OrderNo == orderNo);
            }

            if (!string.IsNullOrEmpty(orderStatus))
            {
                var status = int.Parse(orderStatus);
                query = query.Where(w => w.OrderStatus == status);
            }




            var count = await query.CountAsync();

            var list = await query.Skip(offset).Take(limit).ToListAsync();

            return (list, count);
        }
    }
}
