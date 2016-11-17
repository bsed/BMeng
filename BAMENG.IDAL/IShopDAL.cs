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
    public interface IShopDAL : IDisposable
    {
        /// <summary>
        /// 获取门店列表
        /// </summary>
        /// <param name="ShopType">门店类型1 总店 0分店</param>
        /// <param name="ShopBelongId">门店所属总店ID，门店类型为总店时，此值0</param>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultPageModel GetShopList(int ShopType, int ShopBelongId, SearchModel model);

        /// <summary>
        /// 更新门店信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateShopInfo(ShopModel model);
        /// <summary>
        /// 添加门店
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int AddShopInfo(ShopModel model);

        /// <summary>
        /// 冻结/解冻门店
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        bool UpdateShopActive(int shopId, int active);
        /// <summary>
        /// 删除门店
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        bool DeleltShopInfo(int shopId);


        /// <summary>
        /// 获取门店列表
        /// </summary>
        /// <param name="shopType">1 总店 2分店</param>
        /// <param name="shopId">门店ID，如果shopType为总店时，shopId无效</param>
        /// <returns>List&lt;ShopModel&gt;.</returns>
        List<ShopModel> GetShopList(int shopType, int shopId);

    }
}
