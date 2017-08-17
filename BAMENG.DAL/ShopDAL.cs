/*
    版权所有:杭州火图科技有限公司
    地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
    (c) Copyright Hangzhou Hot Technology Co., Ltd.
    Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
    2013-2016. All rights reserved.
**/


using BAMENG.CONFIG;
using BAMENG.IDAL;
using BAMENG.MODEL;
using HotCoreUtils.DB;
using HotCoreUtils.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMENG.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public class ShopDAL : AbstractDAL, IShopDAL
    {
        /// <summary>
        /// 添加门店
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddShopInfo(ShopModel model)
        {
            if (!IsExist(model.LoginName))
            {
                string strSql = @"insert into BM_ShopManage(ShopName,ShopType,ShopBelongId,ShopProv,ShopCity,ShopArea,ShopAddress,Contacts,ContactWay,LoginName,LoginPassword,IsActive)
                                values (@ShopName,@ShopType,@ShopBelongId,@ShopProv,@ShopCity,@ShopArea,@ShopAddress,@Contacts,@ContactWay,@LoginName,@LoginPassword,@IsActive)";

                var param = new[] {
                        new SqlParameter("@ShopName", model.ShopName),
                        new SqlParameter("@ShopType", model.ShopType),
                        new SqlParameter("@ShopBelongId", model.ShopBelongId),
                        new SqlParameter("@ShopProv", model.ShopProv),
                        new SqlParameter("@ShopCity", model.ShopCity),
                        new SqlParameter("@ShopArea", model.ShopArea),
                        new SqlParameter("@ShopAddress", model.ShopAddress),
                        new SqlParameter("@Contacts", model.Contacts),
                        new SqlParameter("@ContactWay", model.ContactWay),
                        new SqlParameter("@LoginName", model.LoginName),
                        new SqlParameter("@LoginPassword",  EncryptHelper.MD5_8(model.LoginPassword)),
                        new SqlParameter("@IsActive",model.IsActive)
                };
                return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql, param);
            }
            else
                return -1;
        }

        /// <summary>
        /// 判断用户名是否存在
        /// </summary>
        /// <param name="loginName">Name of the login.</param>
        /// <returns>true if the specified login name is exist; otherwise, false.</returns>
        public bool IsExist(string loginName)
        {
            string strSql = "select COUNT(1) from BM_ShopManage where LoginName=@LoginName";
            var param = new[] {
                        new SqlParameter("@LoginName",loginName)
            };
            return Convert.ToInt32(DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql, param)) > 0;
        }

        /// <summary>
        /// 删除门店
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        public bool DeleltShopInfo(int shopId)
        {
            string strSql = "update BM_ShopManage set IsDel=1 where ShopID=@ShopID";
            var param = new[] {
                        new SqlParameter("@ShopID",shopId)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql, param) > 0;
        }

        /// <summary>
        /// 获取门店列表
        /// </summary>
        /// <param name="ShopType">门店类型1 总店 0分店</param>
        /// <param name="ShopBelongId">门店所属总店ID，门店类型为总店时，此值0</param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultPageModel GetShopList(int ShopType, int ShopBelongId, SearchModel model)
        {
            ResultPageModel result = new ResultPageModel();
            if (model == null)
                return result;
            string strSql = @"select  S.ShopID,S.ShopName,S.ShopType,S.ShopBelongId,S.ShopProv,S.ShopCity,S.ShopArea,S.ShopAddress,S.Contacts,S.ContactWay,S.LoginName,S.LoginPassword,S.IsActive,S.CreateTime {0} from BM_ShopManage S ";

            if (model.type == 2)
            {
                strSql = string.Format(strSql, ",S2.ShopName as BelongOneShopName ");
                strSql += " left join BM_ShopManage S2 on S2.ShopID=S.ShopBelongId ";
            }
            else
                strSql = string.Format(strSql, "");
            strSql += " where 1=1 and S.IsDel<>1  and S.ShopType=@ShopType";
            if (ShopType == 2 && ShopBelongId > 0)
            {
                strSql += " and S.ShopBelongId=@ShopBelongId";
            }

            if (!string.IsNullOrEmpty(model.key))
            {
                if (model.type == 2)
                    strSql += string.Format(" and (S.ShopName like '%{0}%' or S2.ShopName  like '%{0}%') ", model.key);
                else
                    strSql += string.Format(" and S.ShopName like '%{0}%' ", model.key);
            }

            if (!string.IsNullOrEmpty(model.province))
            {
                strSql += " and S.ShopProv=@ShopProv";
            }
            if (!string.IsNullOrEmpty(model.city))
            {
                strSql += " and S.ShopCity=@ShopCity";
            }


            if (!string.IsNullOrEmpty(model.startTime))
                strSql += " and CONVERT(nvarchar(10),S.CreateTime,121)>=CONVERT(nvarchar(10),@startTime,121) ";
            if (!string.IsNullOrEmpty(model.endTime))
                strSql += " and CONVERT(nvarchar(10),S.CreateTime,121)<=CONVERT(nvarchar(10),@endTime,121) ";


            var param = new[] {
                        new SqlParameter("@ShopType", ShopType),
                        new SqlParameter("@ShopBelongId", ShopBelongId),
                        new SqlParameter("@ShopProv", model.province),
                        new SqlParameter("@ShopCity", model.city),
                        new SqlParameter("@startTime", model.startTime),
                        new SqlParameter("@endTime", model.endTime),

            };

            //生成sql语句
            return getPageData<ShopModel>(model.PageSize, model.PageIndex, strSql, "S.CreateTime", false, param);

        }
        /// <summary>
        /// 冻结/解冻门店账户
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public bool UpdateShopActive(int shopId, int active)
        {
            string strSql = "update BM_ShopManage set IsActive=@IsActive where ShopID=@ShopID";
            var param = new[] {
                        new SqlParameter("@IsActive",active),
                        new SqlParameter("@ShopID",shopId)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql, param) > 0;
        }

        /// <summary>
        /// 根据总店ID，判断其所有分店是否全部冻结
        /// </summary>
        /// <param name="shopId">The shop identifier.</param>
        /// <returns>true if [is all disable by shop identifier] [the specified shop identifier]; otherwise, false.</returns>
        public bool IsAllDisableByShopID(int shopId)
        {
            string strSql = "select COUNT(1) from BM_ShopManage where ShopBelongId=@ShopID and IsActive=1  and IsDel=0";
            var param = new[] {
                new SqlParameter("@ShopID",shopId)
            };
            return Convert.ToInt32(DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql, param)) == 0;
        }


        /// <summary>
        /// 更新门店信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateShopInfo(ShopModel model)
        {
            string strSql = @"update BM_ShopManage set ShopName=@ShopName,ShopProv=@ShopProv,ShopCity=@ShopCity,ShopArea=@ShopArea,ShopAddress=@ShopAddress,Contacts=@Contacts,ContactWay=@ContactWay";

            if (!string.IsNullOrEmpty(model.LoginPassword))
                strSql += ",LoginPassword=@LoginPassword";

            strSql += " where ShopID=@ShopID";
            var param = new[] {
                        new SqlParameter("@ShopName", model.ShopName),
                        new SqlParameter("@ShopProv", model.ShopProv),
                        new SqlParameter("@ShopCity", model.ShopCity),
                        new SqlParameter("@ShopArea", model.ShopArea),
                        new SqlParameter("@ShopAddress", model.ShopAddress),
                        new SqlParameter("@Contacts", model.Contacts),
                        new SqlParameter("@ContactWay", model.ContactWay),
                        new SqlParameter("@LoginPassword", EncryptHelper.MD5_8(model.LoginPassword)),
                        new SqlParameter("@ShopID",model.ShopID)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql, param) > 0;
        }




        /// <summary>
        /// 获取门店列表
        /// </summary>
        /// <param name="shopType">1 总店 2分店</param>
        /// <param name="shopId">门店ID，如果shopType为总店时，shopId无效</param>
        /// <returns>List&lt;ShopModel&gt;.</returns>
        public List<ShopModel> GetShopList(int shopType, int shopId)
        {
            string strSql = "select ShopID,ShopName,ShopType,ShopBelongId,ShopProv,ShopCity,ShopArea,ShopAddress,Contacts,ContactWay,LoginName,CreateTime,IsActive from BM_ShopManage where 1=1 and IsActive=1 ";

            strSql += " and ShopType=@ShopType";
            if (shopType == 2)
                strSql += " and ShopBelongId=@ShopBelongId";
            var param = new[] {
                        new SqlParameter("@ShopType", shopType),
                        new SqlParameter("@ShopBelongId", shopId)
            };
            using (SqlDataReader dr = DbHelperSQLP.ExecuteReader(WebConfig.getConnectionString(), CommandType.Text, strSql, param))
            {
                return DbHelperSQLP.GetEntityList<ShopModel>(dr);
            }
        }

        /// <summary>
        /// 获取门店所属总店ID
        /// </summary>
        /// <param name="shopId">The shop identifier.</param>
        /// <returns>System.Int32.</returns>
        public int GetBelongShopId(int shopId)
        {
            string strSql = "select ShopBelongId from BM_ShopManage where ShopID=@ShopID  and ShopType=2 ";
            var param = new[] {
                        new SqlParameter("@ShopID", shopId)
            };
            object obj = DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql, param);
            if (obj != null)
                return Convert.ToInt32(obj);
            return 0;
        }

        /// <summary>
        /// 获取门店信息
        /// </summary>
        /// <param name="shopId">The shop identifier.</param>
        /// <returns>ShopModel.</returns>
        public ShopModel GetShopModel(int shopId)
        {
            string strSql = "select ShopID,ShopName,ShopType,ShopBelongId,ShopProv,ShopCity,ShopArea,ShopAddress,Contacts,ContactWay,LoginName,CreateTime,IsActive from BM_ShopManage where 1=1 and IsActive=1 ";
            strSql += " and ShopID=@ShopID";
            var param = new[] {
                new SqlParameter("@ShopID", shopId)
            };
            using (SqlDataReader dr = DbHelperSQLP.ExecuteReader(WebConfig.getConnectionString(), CommandType.Text, strSql, param))
            {
                return DbHelperSQLP.GetEntity<ShopModel>(dr);
            }
        }


        /// <summary>
        /// 获取门店客户维护提醒时间
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        public int GetShopTipHours(int shopId)
        {
            string strSql = "select TipHours from BM_ShopConfig where ShopId=@ShopId";
            var param = new[] {
                new SqlParameter("@ShopID", shopId)
            };


            var obj = DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql, param);
            if (obj != null)
                return Convert.ToInt32(obj);
            else
                return 0;

        }

        /// <summary>
        /// 添加门店客户维护提醒时间
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="tipHours"></param>
        /// <returns></returns>
        public int AddShopTipHours(int shopId, int tipHours)
        {
            string strSql = @"IF EXISTS (SELECT * FROM BM_ShopConfig WHERE ShopId=@ShopId)  
                                            BEGIN
                                                UPDATE BM_ShopConfig set TipHours=@TipHours where ShopId=@ShopId
                                            END
                                            ELSE 
                                            BEGIN
                                                INSERT INTO BM_ShopConfig(TipHours,ShopId) VALUES(@TipHours,@ShopId)
                                            END";
            var param = new[] {
                new SqlParameter("@ShopID", shopId),
                new SqlParameter("@TipHours", tipHours)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql, param);
        }


    }
}
