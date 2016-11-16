/*
    版权所有:杭州火图科技有限公司
    地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
    (c) Copyright Hangzhou Hot Technology Co., Ltd.
    Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
    2013-$today.year. All rights reserved.
**/

using BAMENG.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMENG.IDAL
{
    public interface ICustomerDAL : IDisposable
    {
        /// <summary>
        /// 根据用户ID，获取客户列表
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="shopId">The shop identifier.</param>
        /// <param name="isvalid">是否是有效客户</param>
        /// <returns>ResultPageModel.</returns>
        ResultPageModel GetCustomerList(SearchModel model,int shopId, bool isvalid = true);


        /// <summary>
        /// 获取客户列表
        /// </summary>
        /// <param name="UserId">The user identifier.</param>
        /// <param name="identity">0盟友  1盟主</param>
        /// <param name="type">0所有客户 1未处理  2已处理</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>ResultPageModel.</returns>
        ResultPageModel GetAppCustomerList(int UserId, int identity, int type, int pageIndex, int pageSize);



        /// <summary>
        /// 获取客户信息
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>CustomerModel.</returns>
        CustomerModel GetModel(int customerId);

        /// <summary>
        /// 修改客户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateCustomerInfo(CustomerModel model);

        /// <summary>
        /// 添加客户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int InsertCustomerInfo(CustomerModel model);

        /// <summary>
        /// 删除客户
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        bool DeleteCustomerInfo(int customerId);


        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="status"></param>
        /// <param name="userId">操作人ID(此方法只有盟主操作)</param>
        /// <returns></returns>
        bool UpdateStatus(int customerId, int status, int userId);


        /// <summary>
        /// 更新客户进店状态
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="status">1进店 0未进店</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        bool UpdateInShopStatus(int customerId, int status);

        /// <summary>
        /// 判断客户是否存在
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="addr">地址</param>
        /// <returns></returns>
        bool IsExist(string mobile, string addr);


        /// <summary>
        /// 获得客户信息
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        CustomerModel getCustomerModel(string mobile, string address);

    }
}
