/*
    版权所有:杭州火图科技有限公司
    地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
    (c) Copyright Hangzhou Hot Technology Co., Ltd.
    Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
    2013-2016. All rights reserved.
**/


using BAMENG.CONFIG;
using BAMENG.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMENG.LOGIC
{
    public class ShopLogic
    {
        /// <summary>
        /// 获取门店列表
        /// </summary>
        /// <param name="ShopType">门店类型1 总店 0分店</param>
        /// <param name="ShopBelongId">门店所属总店ID，门店类型为总店时，此值0</param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ResultPageModel GetShopList(int ShopType, int ShopBelongId, SearchModel model)
        {
            using (var dal = FactoryDispatcher.ShopFactory())
            {
                return dal.GetShopList(ShopType, ShopBelongId, model);
            }
        }


        /// <summary>
        /// 编辑门店信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool EditShopInfo(ShopModel model, out ApiStatusCode apiCode)
        {
            apiCode = ApiStatusCode.OK;
            using (var dal = FactoryDispatcher.ShopFactory())
            {
                if (model.ShopID > 0)
                {
                    if (dal.UpdateShopInfo(model))
                    {
                        apiCode = ApiStatusCode.OK;
                        return true;
                    }
                    else
                        apiCode = ApiStatusCode.更新失败;
                }
                else
                {
                    int flag = dal.AddShopInfo(model);
                    if (flag == -1)
                    {
                        apiCode = ApiStatusCode.用户名已存在;
                        return false;
                    }
                    else if (flag > 0)
                        return true;
                    else
                        apiCode = ApiStatusCode.添加失败;
                }
                return false;
            }
        }

        /// <summary>
        /// 删除门店
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        public static bool DeleteShop(int shopId)
        {
            using (var dal = FactoryDispatcher.ShopFactory())
            {
                return dal.DeleltShopInfo(shopId);
            }
        }

        /// <summary>
        /// 冻结或解冻门店
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static bool UpdateShopActive(int shopId, int active)
        {
            using (var dal = FactoryDispatcher.ShopFactory())
            {

                int belongShopId = 1;
                //冻结操作时，获取当前门店的总店ID， 如果大于0，表示当前操作的门店是分店
                if (active == 0)
                    belongShopId = GetBelongShopId(shopId);
                if (belongShopId > 0)
                    return dal.UpdateShopActive(shopId, active);
                else
                {
                    //如果操作的是总店时，需要判断该店下的所有分店是否全部冻结
                    if (dal.IsAllDisableByShopID(shopId))
                        return dal.UpdateShopActive(shopId, active);
                }

                return false;
            }
        }
        /// <summary>
        /// 获取门店列表
        /// </summary>
        /// <param name="shopType">1 总店 2分店</param>
        /// <param name="shopId">门店ID，如果shopType为总店时，shopId无效</param>
        /// <returns>List&lt;ShopModel&gt;.</returns>
        public static List<ShopModel> GetShopList(int shopType, int shopId)
        {
            using (var dal = FactoryDispatcher.ShopFactory())
            {
                return dal.GetShopList(shopType, shopId);
            }
        }


        /// <summary>
        /// 获取门店所属总店ID
        /// </summary>
        /// <param name="shopId">The shop identifier.</param>
        /// <returns>System.Int32.</returns>
        public static int GetBelongShopId(int shopId)
        {
            using (var dal = FactoryDispatcher.ShopFactory())
            {
                return dal.GetBelongShopId(shopId);
            }
        }

        public static ShopModel GetShopModel(int shopId)
        {
            using (var dal = FactoryDispatcher.ShopFactory())
            {
                return dal.GetShopModel(shopId);
            }
        }

        /// <summary>
        /// 获取维护提醒时间
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        public static int GetShopTipHours(int shopId)
        {
            using (var dal = FactoryDispatcher.ShopFactory())
            {
                return dal.GetShopTipHours(shopId);
            }
        }
        /// <summary>
        /// 添加客户维护提醒时间
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="tipHours"></param>
        /// <returns></returns>
        public static int AddShopTipHours(int shopId, int tipHours)
        {
            using (var dal = FactoryDispatcher.ShopFactory())
            {
                return dal.AddShopTipHours(shopId, tipHours);
            }
        }
    }
}
