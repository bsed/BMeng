/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2016. All rights reserved.
 * author guomw
**/


using BAMENG.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAMENG.MODEL;
using HotCoreUtils.DB;
using BAMENG.CONFIG;
using System.Data;
using System.Data.SqlClient;

namespace BAMENG.DAL
{
    public class MessageDAL : AbstractDAL, IMessageDAL
    {


        private const string SELECT_SQL = "select ID,Title,AuthorName,SendTargetIds,MessageBody,IsSend,CreateTime from BM_MessageManage where IsDel=0 ";

        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public int AddMessageInfo(MessageModel model)
        {
            string strSql = "insert into BM_MessageManage(Title,AuthorName,SendTargetIds,MessageBody,IsSend) values(@Title,@AuthorName,@SendTargetIds,@MessageBody,@IsSend)";
            var parm = new[] {
                new SqlParameter("@Title", model.Title),
                new SqlParameter("@AuthorName", model.AuthorName),
                new SqlParameter("@SendTargetIds", model.SendTargetIds),
                new SqlParameter("@MessageBody", model.MessageBody),
                new SqlParameter("@IsSend", model.IsSend)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parm);
        }

        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="messageId">The message identifier.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool DeleteMessageInfo(int messageId)
        {
            string strSql = "update BM_MessageManage  set IsDel=1 where  ID=@ID";
            var parm = new[] {
                new SqlParameter("@ID",messageId)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parm) > 0;
        }

        /// <summary>
        /// 获取消息列表
        /// </summary>
        /// <param name="shopId">门店ID 0总后台，</param>
        /// <param name="model">The model.</param>
        /// <returns>ResultPageModel.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public ResultPageModel GetMessageList(int shopId, SearchModel model)
        {
            ResultPageModel result = new ResultPageModel();
            if (model == null)
                return result;

            string strSql = string.Empty;
            if (model.type == 1)
            {
                strSql = SELECT_SQL;
                if (!string.IsNullOrEmpty(model.key))
                    strSql += string.Format(" and Title like '%{0}%' ", model.key);
                strSql += " and ShopId=@ShopId ";
                if (!string.IsNullOrEmpty(model.startTime))
                    strSql += " and CONVERT(nvarchar(10),CreateTime,121)>=@startTime ";
                if (!string.IsNullOrEmpty(model.endTime))
                    strSql += " and CONVERT(nvarchar(10),CreateTime,121)<=@endTime ";
                var param = new[] {
                    new SqlParameter("@startTime", model.startTime),
                    new SqlParameter("@endTime", model.endTime),
                    new SqlParameter("@ShopId", shopId)
                };
                //生成sql语句
                return getPageData<MessageModel>(model.PageSize, model.PageIndex, strSql, "CreateTime", param);
            }
            else
            {
                strSql = @"select M.ID,M.Title,M.AuthorName,M.SendTargetIds,M.MessageBody,M.IsSend,T.IsRead,M.CreateTime from BM_MessageManage M
                            inner join BM_MessageSendTarget T on T.MessageId = M.ID
                            where M.IsDel = 0";

                if (!string.IsNullOrEmpty(model.startTime))
                    strSql += " and CONVERT(nvarchar(10),M.CreateTime,121)>=@startTime ";
                if (!string.IsNullOrEmpty(model.endTime))
                    strSql += " and CONVERT(nvarchar(10),M.CreateTime,121)<=@endTime ";

                strSql += "  and T.SendTargetShopId=@ShopId ";

                var param = new[] {
                    new SqlParameter("@startTime", model.startTime),
                    new SqlParameter("@endTime", model.endTime),
                    new SqlParameter("@ShopId", shopId)
                };
                //生成sql语句
                return getPageData<MessageModel>(model.PageSize, model.PageIndex, strSql, "M.CreateTime", param);

            }
        }

        /// <summary>
        /// 获取消息信息
        /// </summary>
        /// <param name="messageId">The message identifier.</param>
        /// <returns>MessageModel.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public MessageModel GetModel(int messageId)
        {
            MessageModel model = new MessageModel();
            string strSql = SELECT_SQL + " and ID=@ID";
            var parms = new[] {
               new SqlParameter("@ID",messageId)
           };
            using (IDataReader dr = DbHelperSQLP.ExecuteReader(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parms))
            {
                model = DbHelperSQLP.GetEntity<MessageModel>(dr);
            }
            return model;
        }

        /// <summary>
        /// 修改消息
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool UpdateMessageInfo(MessageModel model)
        {
            string strSql = "update BM_MessageManage set Title=@Title,AuthorName=@AuthorName,MessageBody=@MessageBody,IsSend=@IsSend where ID=@ID";
            var parm = new[] {
                new SqlParameter("@Title", model.Title),
                new SqlParameter("@AuthorName", model.AuthorName),
                new SqlParameter("@MessageBody", model.MessageBody),
                new SqlParameter("@IsSend", model.IsSend),
                new SqlParameter("@ID", model.ID)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parm) > 0;
        }
    }
}
