/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2016. All rights reserved.
 * author guomw
**/


using BAMENG.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMENG.IDAL
{
    /// <summary>
    /// 后台管理相关接口
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IManagerDAL : IDisposable
    {
        /// <summary>
        /// 获取管理员列表
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ResultPageModel.</returns>
        ResultPageModel GetManagerList(SearchModel model);

        /// <summary>
        /// 获取管理员信息
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <returns>AdminLoginModel.</returns>
        AdminLoginModel GetModel(int userid);

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        bool Update(AdminLoginModel model);

        /// <summary>
        /// Inserts the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>System.Int32.</returns>
        int Insert(AdminLoginModel model);

        /// <summary>
        /// 设置用户激活状态
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        bool SetUserStatus(int userId);


        /// <summary>
        /// 更新最后登录时间
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        bool UpdateLastLoginTime(int userId);

        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        bool Delete(int userId);

    }
}
