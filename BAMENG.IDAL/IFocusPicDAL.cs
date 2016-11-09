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
    /// Interface 焦点广告图片
    /// </summary>
    public interface IFocusPicDAL : IDisposable
    {
        /// <summary>
        /// 获取焦点广告图列表
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ResultPageModel.</returns>
        ResultPageModel GetList(SearchModel model);


        /// <summary>
        /// 获取播图
        /// </summary>
        /// <param name="type">0 资讯轮播图 1首页轮播图</param>
        /// <returns>List&lt;FocusPicModel&gt;.</returns>
        List<FocusPicModel> GetAppList(int type);

        /// <summary>
        /// 添加焦点广告图
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        int AddFocusPic(FocusPicModel model);

        /// <summary>
        /// 更新焦点广告图
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        bool UpdateFocusPic(FocusPicModel model);


        /// <summary>
        /// 删除焦点广告图
        /// </summary>
        /// <param name="fid">The fid.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool DeleteFocusPic(int fid);


        /// <summary>
        /// 设置启用状态
        /// </summary>
        /// <param name="fid">The fid.</param>        
        /// <returns></returns>
        bool SetEnable(int fid);





    }
}
