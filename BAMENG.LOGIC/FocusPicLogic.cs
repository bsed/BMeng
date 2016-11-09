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
    /// 焦点广告图
    /// </summary>
    public class FocusPicLogic
    {
        /// <summary>
        /// 获取焦点广告图列表
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ResultPageModel.</returns>
        public static ResultPageModel GetList(SearchModel model)
        {
            using (var dal = FactoryDispatcher.FocusFactory())
            {
                return dal.GetList(model);
            }
        }

        /// <summary>
        /// 获取播图
        /// </summary>
        /// <param name="type">0 资讯轮播图 1首页轮播图</param>
        /// <returns>List&lt;FocusPicModel&gt;.</returns>
        public static List<FocusPicModel> GetAppList(int type)
        {
            using (var dal = FactoryDispatcher.FocusFactory())
            {
                return dal.GetAppList(type);
            }
        }


        /// <summary>
        /// 编辑焦点广告图
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static bool EditFocusPic(FocusPicModel model)
        {
            using (var dal = FactoryDispatcher.FocusFactory())
            {
                if (model.ID > 0)
                    return dal.UpdateFocusPic(model);
                else
                    return dal.AddFocusPic(model) > 0;
            }
        }


        /// <summary>
        /// 删除焦点广告图
        /// </summary>
        /// <param name="fid">The fid.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool DeleteFocusPic(int fid)
        {
            using (var dal = FactoryDispatcher.FocusFactory())
            {
                return dal.DeleteFocusPic(fid);
            }
        }

        /// <summary>
        /// 设置启用状态
        /// </summary>
        /// <param name="fid">The fid.</param>        
        /// <returns></returns>
        public static bool SetEnable(int fid)
        {
            using (var dal = FactoryDispatcher.FocusFactory())
            {
                return dal.SetEnable(fid);
            }
        }
    }
}
