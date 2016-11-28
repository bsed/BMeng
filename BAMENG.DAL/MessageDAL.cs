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


        private const string SELECT_SQL = "select ID,Title,AuthorId,AuthorName,IsSendBelongShopId,SendTargetIds,MessageBody,IsSend,CreateTime,AuthorIdentity from BM_MessageManage where IsDel=0 ";

        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public int AddMessageInfo(MessageModel model)
        {
            string strSql = "insert into BM_MessageManage(Title,AuthorName,SendTargetIds,MessageBody,IsSend,IsSendBelongShopId,AuthorIdentity,AuthorId) values(@Title,@AuthorName,@SendTargetIds,@MessageBody,@IsSend,@IsSendBelongShopId,@AuthorIdentity,@AuthorId);select @@IDENTITY";
            var parm = new[] {
                new SqlParameter("@Title", model.Title),
                new SqlParameter("@AuthorName", model.AuthorName),
                new SqlParameter("@SendTargetIds", model.SendTargetIds),
                new SqlParameter("@MessageBody", model.MessageBody),
                new SqlParameter("@IsSend", model.IsSend),
                new SqlParameter("@IsSendBelongShopId",model.IsSendBelongShopId),
                new SqlParameter("@AuthorIdentity",model.AuthorIdentity),
                new SqlParameter("@AuthorId",model.AuthorId)
            };
            object obj = DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parm);
            if (obj != null)
                return Convert.ToInt32(obj);
            else
                return 0;
        }

        /// <summary>
        /// 添加消息发送目标
        /// </summary>
        /// <param name="messageId">The message identifier.</param>
        /// <param name="SendTargetShopId">如果是总店往总后台发布信息通知，则值为-1</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public bool AddMessageSendTarget(int messageId, int SendTargetShopId)
        {
            string strSql = "insert into BM_MessageSendTarget(MessageId,SendTargetShopId) values(@MessageId,@SendTargetShopId)";
            var parm = new[] {
                new SqlParameter("@MessageId", messageId),
                new SqlParameter("@SendTargetShopId",SendTargetShopId)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parm) > 0;
        }



        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="messageId">The message identifier.</param>
        /// <param name="shopId">The shop identifier.</param>
        /// <param name="type">The type.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool DeleteMessageInfo(int messageId, int shopId, int type)
        {
            string strSql = string.Empty;
            if (type == 1)
                strSql = "update BM_MessageManage  set IsDel=1 where  ID=@ID";
            else
                strSql = "delete from BM_MessageSendTarget where MessageId=@ID and SendTargetShopId=@ShopID";


            var parm = new[] {
                new SqlParameter("@ID",messageId),
                new SqlParameter("@ShopID",shopId)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parm) > 0;
        }

        /// <summary>
        /// 获取消息列表
        /// </summary>
        /// <param name="shopId">门店ID 0总后台，</param>
        /// <param name="AuthorIdentity">作者身份 0总后台，1总店，2分店</param>
        /// <param name="model">The model.</param>
        /// <returns>ResultPageModel.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public ResultPageModel GetMessageList(int shopId, int AuthorIdentity, SearchModel model)
        {
            ResultPageModel result = new ResultPageModel();
            if (model == null)
                return result;

            string strSql = string.Empty;
            if (model.type == 1)
            {
                strSql = SELECT_SQL + " and AuthorIdentity=@AuthorIdentity";
                if (!string.IsNullOrEmpty(model.key))
                    strSql += string.Format(" and Title like '%{0}%' ", model.key);
                if (!string.IsNullOrEmpty(model.startTime))
                    strSql += " and CONVERT(nvarchar(10),CreateTime,121)>=@startTime ";
                if (!string.IsNullOrEmpty(model.endTime))
                    strSql += " and CONVERT(nvarchar(10),CreateTime,121)<=@endTime ";


                if (AuthorIdentity != 0)
                    strSql += " and AuthorId=@AuthorId";

                var param = new[] {
                    new SqlParameter("@startTime", model.startTime),
                    new SqlParameter("@endTime", model.endTime),
                    new SqlParameter("@AuthorIdentity", AuthorIdentity),
                    new SqlParameter("@AuthorId", shopId)
                };
                //生成sql语句
                return getPageData<MessageModel>(model.PageSize, model.PageIndex, strSql, "CreateTime", param, ((items) =>
                {
                    items.ForEach((item) =>
                    {
                        if (item.AuthorIdentity == 2)
                            item.SendTargetName = GetMessageShopName(item.AuthorId);
                        else
                        {
                            item.SendTargetName = GetMessageShopName(item.SendTargetIds);
                            if (item.IsSendBelongShopId == 1)
                                item.SendTargetName += " /总站";
                        }

                    });
                }));
            }
            else
            {
                if (AuthorIdentity != 0)
                {
                    strSql = @"select M.ID,M.Title,M.AuthorName,M.SendTargetIds,M.MessageBody,M.IsSend,T.IsRead,M.CreateTime from BM_MessageManage M
                            inner join BM_MessageSendTarget T on T.MessageId = M.ID
                            where M.IsDel = 0  and M.IsSend=1  and T.SendTargetShopId=@ShopId ";
                }
                else
                {
                    strSql = @"select M.ID,M.Title,M.AuthorName,M.SendTargetIds,M.MessageBody,M.IsSend,M.IsRead,M.CreateTime from BM_MessageManage M                            
                            where M.IsDel = 0  and M.IsSend=1  and M.IsSendBelongShopId=1 ";
                }

                if (!string.IsNullOrEmpty(model.startTime))
                    strSql += " and CONVERT(nvarchar(10),M.CreateTime,121)>=@startTime ";
                if (!string.IsNullOrEmpty(model.endTime))
                    strSql += " and CONVERT(nvarchar(10),M.CreateTime,121)<=@endTime ";

                var param = new[] {
                    new SqlParameter("@startTime", model.startTime),
                    new SqlParameter("@endTime", model.endTime),
                    new SqlParameter("@ShopId", shopId)
                };
                //生成sql语句
                return getPageData<MessageModel>(model.PageSize, model.PageIndex, strSql, "M.CreateTime", param, ((items) =>
                {
                    items.ForEach((item) =>
                    {
                        item.SendTargetName = item.AuthorName;
                    });
                }));

            }
        }



        /// <summary>
        /// 根据多个门店ID，获取门店名称，并拼接在一起
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>System.String.</returns>
        private string GetMessageShopName(string ids)
        {
            var arr = ids.Split(',');
            string strSql = string.Format("select stuff((select '/'+ShopName from BM_ShopManage where ShopID in ({0}) for xml path('')),1,1,'')", string.Join(",", arr));

            return DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql).ToString();
        }


        /// <summary>
        /// 根据门店ID，获取门店名称
        /// </summary>
        /// <param name="shopId">The shop identifier.</param>
        /// <returns>System.String.</returns>
        private string GetMessageShopName(int shopId)
        {
            string strSql = "select ShopName from BM_ShopManage where ShopID=@ShopID";
            var parms = new[] {
               new SqlParameter("@ShopID",shopId)
            };
            return DbHelperSQLP.ExecuteScalar(WebConfig.getConnectionString(), CommandType.Text, strSql, parms).ToString();
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
            string strSql = "update BM_MessageManage set Title=@Title,MessageBody=@MessageBody,IsSend=@IsSend where ID=@ID";
            var parm = new[] {
                new SqlParameter("@Title", model.Title),
                new SqlParameter("@MessageBody", model.MessageBody),
                new SqlParameter("@IsSend", model.IsSend),
                new SqlParameter("@ID", model.ID)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parm) > 0;
        }


        /// <summary>
        /// 修改阅读状态
        /// </summary>
        /// <param name="messageId">The message identifier.</param>
        /// <param name="shopId">The shop identifier.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public bool UpdateReadStatus(int messageId, int shopId)
        {
            string strSql = "update BM_MessageSendTarget set IsRead=1,ReadTime=GETDATE() where MessageId=@MessageId and SendTargetShopId=@SendTargetShopId";
            var parm = new[] {
                new SqlParameter("@MessageId",messageId),
                new SqlParameter("@SendTargetShopId",shopId)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parm) > 0;
        }
        /// <summary>
        /// 修改总后台阅读状态
        /// </summary>
        /// <param name="messageId">The message identifier.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public bool UpdateReadStatus(int messageId)
        {
            string strSql = "update BM_MessageManage set IsRead=1,ReadTime=GETDATE() where ID=@MessageId ";
            var parm = new[] {
                new SqlParameter("@MessageId",messageId)
            };
            return DbHelperSQLP.ExecuteNonQuery(WebConfig.getConnectionString(), CommandType.Text, strSql.ToString(), parm) > 0;

        }

    }
}
