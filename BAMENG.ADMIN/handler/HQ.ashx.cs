using BAMENG.CONFIG;
using BAMENG.LOGIC;
using BAMENG.MODEL;
using HotCoreUtils.Helper;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace BAMENG.ADMIN.handler
{
    /// <summary>
    /// HQ--总部
    /// </summary>
    public class HQ : BaseLogicFactory, IHttpHandler
    {
        public AdminLoginModel user { get; set; }
        public new void ProcessRequest(HttpContext context)
        {
            string resultMsg = string.Format(@"{0} header:{1} Form:{2} UserAgent:{3} IP:{4};referrer:{5}"
                  , context.Request.Url.ToString()
                  , context.Request.Headers.ToString()
                  , context.Request.Form.ToString()
                  , StringHelper.ToString(context.Request.UserAgent)
                  , StringHelper.GetClientIP()
                  , context.Request.UrlReferrer != null ? StringHelper.ToString(context.Request.UrlReferrer.AbsoluteUri) : ""
                 );
            try
            {
                DoRequest(context);
                LogHelper.Log(resultMsg, LogHelperTag.INFO, WebConfig.debugMode());
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("{0} StackTrace:{1} Message:{2}", resultMsg, ex.StackTrace, ex.Message), LogHelperTag.ERROR);
            }
        }

        public new bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public int ShopID
        {
            get
            {
                return GetFormValue("shopId", 0);
            }
        }

        public int UserId
        {
            get
            {
                return GetFormValue("userid", 0);
            }
        }


        public string action { get { return GetFormValue("action", ""); } }

        public string json { get; set; }



        private void DoRequest(HttpContext context)
        {
            user = GetCurrentUser();
            if (user == null)
            {
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.没有登录));
                context.Response.ContentType = "application/json";
                context.Response.Write(json);
                return;
            }
            //全部转换为大写
            try
            {
                switch (action.ToUpper())
                {
                    case "GETSHOPLIST": //获取门店列表
                        GetShopList();
                        break;
                    case "UPDATESHOP":
                        UpdateShopInfo();
                        break;

                    case "DELETESHOP":
                        DeleteShop();
                        break;
                    case "UPDATESHOPACTIVE":
                        UpdateShopActive();
                        break;

                    case "GETUSERLIST":
                        GetUserList();
                        break;
                    case "EDITUSER":
                        EditUser();
                        break;

                    case "DELETEUSER":
                        DeleteUser();
                        break;
                    case "UPDATEUSERACTIVE":
                        UpdateUserActive();
                        break;
                    case "GETUSERINFO":
                        GetUserInfo();
                        break;
                    case "GETALLYLIST":
                        GetAllyList();
                        break;
                    case "GETCUSTOMERLIST":
                        GetCustomerList();
                        break;

                    case "EDITCUSTOMERINFO":
                        EditCustomerInfo();
                        break;
                    case "DELETECUSTOMERINFO":
                        DeleteCustomerInfo();
                        break;

                    case "GETLEVELLIST":
                        GetLevelList();
                        break;
                    case "DELETELEVEL":
                        DeleteLevel();
                        break;
                    case "EDITLEVEL":
                        EditLevel();
                        break;
                    case "GETARTICLELIST":
                        GetArticleList();
                        break;
                    case "GETARTICLEINFO":
                        GetArticleInfo();
                        break;
                    case "UPDATEARTICLECODE":
                        UpdateArticleCode();
                        break;
                    case "EDITARTICLE":
                        EditArticle();
                        break;
                    case "GETMENULIST":
                        GetMenuList();
                        break;

                    case "GETFOCUSPICLIST":
                        GetFocusPicList();
                        break;
                    case "EDITFOCUSPIC":
                        EditFocusPic();
                        break;
                    case "DELETEFOCUSPIC":
                        DeleteFocusPic();
                        break;
                    case "SETFOCUSENABLE":
                        SetFocusEnable();
                        break;
                    case "GETCONFIGLIST":
                        GetConfiglist();
                        break;
                    case "EDITCONFIG":
                        EditConfig();
                        break;


                    case "EDITMANAGER":
                        EditManager();
                        break;
                    case "DELETEMANAGER":
                        DeleteManager();
                        break;
                    case "SETMANAGERUSERSTATUS":
                        SetManagerUserStatus();
                        break;
                    case "GETMANAGERLIST":
                        GetManagerList();
                        break;


                    case "GETCASHCOUPONLIST":
                        GetCashCouponList();
                        break;
                    case "DELETECASHCOUPON":
                        DeleteCashCoupon();
                        break;
                    case "EDITCASHCOUPON":
                        EditCashCoupon();
                        break;

                    case "SETCOUPONENABLE":
                        SetCouponEnable();
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("action:{0} StackTrace:{1} Message:{2}", action, ex.StackTrace, ex.Message), LogHelperTag.ERROR);
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.SERVICEERROR));
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(json);

        }


        /// <summary>
        /// 获取门店列表
        /// </summary>
        private void GetShopList()
        {
            int shopType = user.UserIndentity == 0 ? 1 : 2;
            int shopBelongId = user.UserIndentity == 0 ? 0 : user.ID;

            SearchModel model = new SearchModel()
            {
                PageIndex = Convert.ToInt32(GetFormValue("pageIndex", 1)),
                PageSize = Convert.ToInt32(GetFormValue("pageSize", 20)),
                startTime = GetFormValue("startTime", ""),
                endTime = GetFormValue("endTime", ""),
                key = GetFormValue("key", ""),
                city = GetFormValue("city", ""),
                province = GetFormValue("prov", "")
            };
            var data = ShopLogic.GetShopList(shopType, shopBelongId, model);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }
        /// <summary>
        /// 修改门店信息
        /// </summary>
        private void UpdateShopInfo()
        {
            string shopname = GetFormValue("shopname", "");
            string username = GetFormValue("username", "");
            string usermobile = GetFormValue("usermobile", "");
            string userloginname = GetFormValue("userloginname", "");
            string password = GetFormValue("password", "");
            string shopprov = GetFormValue("shopprov", "");
            string shopcity = GetFormValue("shopcity", "");
            string shopaddress = GetFormValue("shopaddress", "");
            ApiStatusCode apiCode;
            bool flag = ShopLogic.EditShopInfo(new ShopModel()
            {
                ShopID = ShopID,
                ShopName = shopname,
                ShopArea = "",
                ShopAddress = shopaddress,
                ShopBelongId = user.UserIndentity == 0 ? 0 : user.ID,
                ShopCity = shopcity,
                ShopProv = shopprov,
                Contacts = username,
                ContactWay = usermobile,
                LoginName = userloginname,
                LoginPassword = password,
                IsActive = 1,
                ShopType = user.UserIndentity == 0 ? 1 : 2
            }, out apiCode);
            if (flag)
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(apiCode));
        }


        /// <summary>
        /// Deletes the shop.
        /// </summary>
        private void DeleteShop()
        {
            if (ShopLogic.DeleteShop(ShopID))
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.删除失败));
        }

        /// <summary>
        /// 冻结或解冻门店
        /// </summary>
        private void UpdateShopActive()
        {
            int active = GetFormValue("active", 1);
            if (ShopLogic.UpdateShopActive(ShopID, active))
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.更新失败));
        }

        /// <summary>
        /// 获取用户列表(总后台，获取盟主列表)
        /// </summary>
        private void GetUserList()
        {

            SearchModel model = new SearchModel()
            {
                PageIndex = Convert.ToInt32(GetFormValue("pageIndex", 1)),
                PageSize = Convert.ToInt32(GetFormValue("pageSize", 20)),
                startTime = GetFormValue("startTime", ""),
                endTime = GetFormValue("endTime", ""),
                key = GetFormValue("key", ""),
                searchType = GetFormValue("searchType", 0)
            };
            int currentUserId = user.UserIndentity != 0 ? user.ID : 0;
            var data = UserLogic.GetUserList(currentUserId, GetFormValue("ally", 1), model);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }

        /// <summary>
        /// 编辑用户信息
        /// </summary>
        private void EditUser()
        {
            ApiStatusCode apiCode = ApiStatusCode.OK;
            //方便测试，此处门店ID写死，真实逻辑是，根据当前登录的门店账户，获取门店ID
            bool flag = UserLogic.EditUserInfo(new UserRegisterModel()
            {
                loginName = GetFormValue("userloginname", ""),
                username = GetFormValue("username", ""),
                nickname = GetFormValue("usernickname", ""),
                loginPassword = EncryptHelper.MD5(GetFormValue("password", "")),
                mobile = GetFormValue("usermobile", ""),
                storeId = ConstConfig.storeId,
                ShopId = user.ID,
                UserIdentity = user.UserIndentity,
                UserId = UserId
            }, ref apiCode);
            if (flag)
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(apiCode));
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        private void DeleteUser()
        {
            if (UserLogic.DeleteUser(UserId))
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.删除失败));
        }

        /// <summary>
        /// 冻结、解冻用户账户
        /// </summary>
        private void UpdateUserActive()
        {
            int active = GetFormValue("active", 1);
            if (UserLogic.UpdateUserActive(UserId, active))
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.更新失败));
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        private void GetUserInfo()
        {
            var data = UserLogic.GetModel(UserId);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }

        /// <summary>
        /// 根据盟主ID，获取他的盟友列表
        /// </summary>
        private void GetAllyList()
        {
            SearchModel model = new SearchModel()
            {
                PageIndex = Convert.ToInt32(GetFormValue("pageIndex", 1)),
                PageSize = Convert.ToInt32(GetFormValue("pageSize", 20)),
                UserId = UserId
            };
            var data = UserLogic.GetAllyList(model);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }
        /// <summary>
        /// 根据用户ID获取该用户下的客户列表
        /// </summary>
        private void GetCustomerList()
        {
            SearchModel model = new SearchModel()
            {
                PageIndex = Convert.ToInt32(GetFormValue("pageIndex", 1)),
                PageSize = Convert.ToInt32(GetFormValue("pageSize", 20)),
                UserId = UserId,
                startTime = GetFormValue("startTime", ""),
                endTime = GetFormValue("endTime", ""),
                key = GetFormValue("key", ""),
                searchType = GetFormValue("searchType", 0)
            };
            var data = CustomerLogic.GetCustomerList(model, user.UserIndentity == 0 ? 0 : user.ID);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }

        /// <summary>
        /// 编辑客户信息
        /// </summary>
        private void EditCustomerInfo()
        {
            CustomerModel model = new CustomerModel()
            {
                ID = GetFormValue("customerid", 0),
                Name = GetFormValue("username", ""),
                Mobile = GetFormValue("usermobile", ""),
                Addr = GetFormValue("useraddress", "")
            };

            if (CustomerLogic.UpdateCustomerInfo(model))
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.更新失败));
        }


        /// <summary>
        /// 删除客户信息
        /// </summary>
        private void DeleteCustomerInfo()
        {
            if (CustomerLogic.DeleteCustomerInfo(GetFormValue("customerid", 0)))
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.删除失败));
        }


















        /// <summary>
        /// 获取等级
        /// </summary>
        public void GetLevelList()
        {
            var data = UserLogic.GetLevelList(GetFormValue("type", -1));
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }

        /// <summary>
        /// 删除等级
        /// </summary>
        private void DeleteLevel()
        {
            if (UserLogic.DeleteLevel(GetFormValue("levelId", 0)))
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.删除失败));
        }
        /// <summary>
        /// 编辑等级
        /// </summary>
        private void EditLevel()
        {
            int levelId = GetFormValue("levelid", 0);
            int levelType = GetFormValue("leveltype", 0);
            string levelname = GetFormValue("levelname", "");
            int upgradeCount = GetFormValue("levelmembernum", 0);
            if (UserLogic.EditLevel(levelId, levelType, levelname, upgradeCount))
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.删除失败));
        }


        /// <summary>
        /// 获取资讯列表
        /// </summary>
        public void GetArticleList()
        {


            SearchModel model = new SearchModel()
            {
                PageIndex = Convert.ToInt32(GetFormValue("pageIndex", 1)),
                PageSize = Convert.ToInt32(GetFormValue("pageSize", 20)),
                startTime = GetFormValue("startTime", ""),
                endTime = GetFormValue("endTime", ""),
                key = GetFormValue("key", ""),
                searchType = GetFormValue("searchType", 0),
                Status = GetFormValue("type", 1)
            };

            //作者身份类型，0集团，1总店，2分店  3盟主 4盟友
            //AuthorIdentity 根据作者ID来判断作者身份           

            int AuthorIdentity = user.UserIndentity;
            int AuthorId = user.ID;
            var data = ArticleLogic.GetArticleList(AuthorId, AuthorIdentity, model);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }
        /// <summary>
        /// 获取资讯信息
        /// </summary>
        public void GetArticleInfo()
        {
            int articleId = GetFormValue("articleId", 0);
            var data = ArticleLogic.GetModel(articleId);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }


        /// <summary>
        /// 修改状态
        /// </summary>
        private void UpdateArticleCode()
        {
            int articleId = GetFormValue("articleId", 0);
            int type = GetFormValue("type", 0);//1修改置顶，2修改发布，3，删除 ,4审核
            int active = GetFormValue("active", 0);
            string remark = GetFormValue("remark", "");

            bool flag = false;
            if (type == 1)//置顶
                flag = ArticleLogic.SetArticleEnableTop(articleId, active == 1);
            else if (type == 2)//发布
                flag = ArticleLogic.SetArticleEnablePublish(articleId, active == 1);
            else if (type == 3)//删除
                flag = ArticleLogic.DeleteArticle(articleId);
            else if (type == 4)//删除
                flag = ArticleLogic.SetArticleStatus(articleId, active, remark);

            if (flag)
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.更新失败));
        }

        /// <summary>
        /// 编辑资讯
        /// </summary>
        private void EditArticle()
        {
            int ArticleId = GetFormValue("articleid", 0);
            //方便测试，此处门店ID写死，真实逻辑是，根据当前登录的门店账户，获取门店ID
            bool flag = ArticleLogic.EditArticle(new ArticleModel()
            {
                ArticleId = GetFormValue("articleid", 0),
                ArticleIntro = HttpUtility.UrlDecode(GetFormValue("intro", "")),
                ArticleTitle = HttpUtility.UrlDecode(GetFormValue("title", "")),
                EnableTop = GetFormValue("top", 0),
                EnablePublish = GetFormValue("publish", 0),
                ArticleCover = GetFormValue("cover", ""),
                ArticleBody = HttpUtility.UrlDecode(GetFormValue("content", "")),
                SendTargetId = GetFormValue("targetid", 0),
                ArticleSort = 0,
                ArticleStatus = user.UserIndentity == 0 ? 1 : 0,
                AuthorName = user.UserName,
                AuthorId = user.ID,
                AuthorIdentity = user.UserIndentity,
                PublishTime = DateTime.Now,
                TopTime = DateTime.Now
            });
            if (flag)
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ArticleId > 0 ? ApiStatusCode.更新失败 : ApiStatusCode.添加失败));
        }



        /// <summary>
        /// 获取菜单
        /// </summary>
        private void GetMenuList()
        {
            ApiStatusCode appCode = ApiStatusCode.OK;
            SystemLeftModel resultData = new SystemLeftModel();
            if (CheckLogin(ref appCode))
            {
                resultData.menuData = SystemLogic.GetMenuList(user.UserIndentity);
                resultData.userData = user;
                resultData.authority = "";
            }
            json = JsonHelper.JsonSerializer(new ResultModel(appCode, resultData));
        }


        /// <summary>
        /// 获取焦点广告图
        /// </summary>
        private void GetFocusPicList()
        {
            SearchModel model = new SearchModel()
            {
                PageIndex = Convert.ToInt32(GetFormValue("pageIndex", 1)),
                PageSize = Convert.ToInt32(GetFormValue("pageSize", 20)),
                startTime = GetFormValue("startTime", ""),
                endTime = GetFormValue("endTime", ""),
                key = GetFormValue("key", ""),
                type = GetFormValue("type", 0),//0 资讯轮播图 1首页轮播图
            };
            var data = FocusPicLogic.GetList(model);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }

        /// <summary>
        /// 编辑焦点广告图
        /// </summary>
        private void EditFocusPic()
        {
            FocusPicModel model = new FocusPicModel()
            {
                ID = GetFormValue("focusid", 0),
                Title = GetFormValue("focustitle", ""),
                LinkUrl = GetFormValue("focuslinkurl", ""),
                PicUrl = GetFormValue("focuspicurl", ""),
                Type = GetFormValue("type", 0),
                IsEnable = GetFormValue("focusenable", 0),
                Sort = GetFormValue("focussort", 0),
                Description = GetFormValue("focusdescription", "")
            };
            if (FocusPicLogic.EditFocusPic(model))
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.操作失败));
        }

        /// <summary>
        /// 删除焦点广告图
        /// </summary>
        private void DeleteFocusPic()
        {
            if (FocusPicLogic.DeleteFocusPic(GetFormValue("focusid", 0)))
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.删除失败));
        }

        /// <summary>
        /// 设置轮播图启用或禁用
        /// </summary>
        private void SetFocusEnable()
        {
            if (FocusPicLogic.SetEnable(GetFormValue("focusid", 0)))
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.操作失败));
        }

        /// <summary>
        /// 编辑配置
        /// </summary>
        private void GetConfiglist()
        {
            var data = ConfigLogic.GetConfigList();
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }


        /// <summary>
        /// 编辑配置
        /// </summary>
        private void EditConfig()
        {
            string config = HttpUtility.UrlDecode(GetFormValue("config", ""));
            if (!string.IsNullOrEmpty(config))
            {
                List<ConfigModel> lst = JsonHelper.JsonDeserialize<List<ConfigModel>>(config);
                ConfigLogic.UpdateValue(lst);
            }
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
        }



        /// <summary>
        /// Edits the user.
        /// </summary>
        private void EditManager()
        {
            bool flag = ManagerLogic.EditUser(new AdminLoginModel()
            {
                LoginName = GetFormValue("loginName", ""),
                LoginPassword = EncryptHelper.MD5(GetFormValue("password", "")),
                RoleId = GetFormValue("roleid", 0),
                UserEmail = GetFormValue("email", ""),
                UserMobile = GetFormValue("mobile", ""),
                UserName = GetFormValue("name", ""),
                UserStatus = 1,
                ID = UserId
            });
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
        }

        /// <summary>
        /// Deletes the manager.
        /// </summary>
        private void DeleteManager()
        {
            ManagerLogic.Delete(UserId);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
        }

        /// <summary>
        /// Sets the manager user status.
        /// </summary>
        private void SetManagerUserStatus()
        {
            ManagerLogic.SetUserStatus(UserId);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
        }

        /// <summary>
        /// 获取管理员列表
        /// </summary>
        private void GetManagerList()
        {
            SearchModel model = new SearchModel()
            {
                PageIndex = Convert.ToInt32(GetFormValue("pageIndex", 1)),
                PageSize = Convert.ToInt32(GetFormValue("pageSize", 20)),
                startTime = GetFormValue("startTime", ""),
                endTime = GetFormValue("endTime", ""),
                key = GetFormValue("key", "")
            };
            var data = ManagerLogic.GetManagerList(model);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }


        /// <summary>
        /// 获取现金券
        /// </summary>
        private void GetCashCouponList()
        {
            SearchModel model = new SearchModel()
            {
                PageIndex = Convert.ToInt32(GetFormValue("pageIndex", 1)),
                PageSize = Convert.ToInt32(GetFormValue("pageSize", 20)),
                startTime = GetFormValue("startTime", ""),
                endTime = GetFormValue("endTime", ""),
                key = GetFormValue("key", ""),
                Status = GetFormValue("status", -100)
            };
            var data = CouponLogic.GetCashCouponList(user.ID, model);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }

        /// <summary>
        /// 删除现金券
        /// </summary>
        private void DeleteCashCoupon()
        {
            int couponId = GetFormValue("couponId", 0);
            CouponLogic.DeleteCashCoupon(couponId);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
        }
        /// <summary>
        /// 编辑优惠券
        /// </summary>
        private void EditCashCoupon()
        {
            bool flag = CouponLogic.EditCashCoupon(new CashCouponModel()
            {
                CouponId = GetFormValue("couponid", 0),
                Money = GetFormValue("couponmoney", 0),
                Title = GetFormValue("coupontitle", ""),
                StartTime = Convert.ToDateTime(GetFormValue("couponstarttime", DateTime.Now.ToString("yyyy-MM-dd"))),
                EndTime = Convert.ToDateTime(GetFormValue("couponendtime", DateTime.Now.AddDays(5).ToString("yyyy-MM-dd"))),
                ShopId = user.ID,
                IsEnable = GetFormValue("couponenable", 1)
            });
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
        }

        /// <summary>
        /// Sets the coupon enable.
        /// </summary>
        private void SetCouponEnable()
        {
            if (CouponLogic.SetCouponEnable(GetFormValue("couponId", 0)))
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.操作失败));
        }
    }
}