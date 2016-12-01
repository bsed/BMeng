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

namespace BAMENG.LOGIC
{
    /// <summary>
    /// 管理员相关业务逻辑
    /// </summary>
    public class ManagerLogic
    {


        /// <summary>
        /// 编辑用户信息
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool EditUser(AdminLoginModel model)
        {
            if (model.ID > 0)
                return Update(model);
            else
                return Insert(model) > 0;
        }


        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static bool Update(AdminLoginModel model)
        {
            using (var dal = FactoryDispatcher.ManagerFactory())
            {
                return dal.Update(model);
            }
        }

        /// <summary>
        /// 设置用户激活状态
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static bool SetUserStatus(int userId)
        {
            using (var dal = FactoryDispatcher.ManagerFactory())
            {
                return dal.SetUserStatus(userId);
            }
        }


        /// <summary>
        /// 更新最后登录时间
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool UpdateLastLoginTime(int userId)
        {
            using (var dal = FactoryDispatcher.ManagerFactory())
            {
                return dal.UpdateLastLoginTime(userId);
            }
        }

        /// <summary>
        /// Inserts the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static int Insert(AdminLoginModel model)
        {
            using (var dal = FactoryDispatcher.ManagerFactory())
            {
                return dal.Insert(model);
            }
        }

        /// <summary>
        /// 获取管理员信息
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <returns>AdminLoginModel.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static AdminLoginModel GetModel(int userid)
        {
            using (var dal = FactoryDispatcher.ManagerFactory())
            {
                return dal.GetModel(userid);
            }
        }

        /// <summary>
        /// 获取管理员列表
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ResultPageModel.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static ResultPageModel GetManagerList(SearchModel model)
        {
            using (var dal = FactoryDispatcher.ManagerFactory())
            {
                return dal.GetManagerList(model);
            }
        }


        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static bool Delete(int userId)
        {
            using (var dal = FactoryDispatcher.ManagerFactory())
            {
                return dal.Delete(userId);
            }
        }


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="useridentity">The useridentity.</param>
        /// <param name="oldPassword">The old password.</param>
        /// <param name="password">The password.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool ChanagePassword(int userId, int useridentity, string oldPassword, string password)
        {
            using (var dal = FactoryDispatcher.ManagerFactory())
            {
                return dal.ChanagePassword(userId, useridentity, oldPassword, password);
            }
        }
    }
}
