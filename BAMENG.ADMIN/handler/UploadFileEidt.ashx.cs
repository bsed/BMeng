using BAMENG.LOGIC;
using HotCoreUtils.Helper;
using HotCoreUtils.Uploader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace BAMENG.ADMIN.handler
{
    /// <summary>
    /// UploadFileEidt 的摘要说明
    /// </summary>
    public class UploadFileEidt : BaseLogicFactory, IHttpHandler
    {
        private string _sAllowType = "";
        //string sInputName = "filedata";//表单文件域name
        private string _xheditorPath = System.Configuration.ConfigurationManager.AppSettings["uploadfile"] ?? "uploadpic/";
        int iMsgType = 2;                 //返回上传参数的格式：1，只返回url，2，返回参数数组
        private HttpContext content { get; set; }

        #region 属性
        /// <summary>
        /// 要在服务器上生成的目录名
        /// </summary>
        private string ClientPath
        {
            get { return this.GetQueryString("p", "pic"); }

        }
        /// <summary>
        /// 判断是否需要生成缩略图
        /// </summary>
        private bool IsBuildThumbnail
        {
            get
            {
                return this.GetQueryString("bt", 0) > 0;
            }
        }
        /// <summary>
        /// 缩略图宽
        /// </summary>
        private int ThmbnailW { get { return this.GetQueryString("tbw", 80); } }
        /// <summary>
        /// 缩略图高
        /// </summary>
        private int ThmbnailH { get { return this.GetQueryString("tbh", 80); } }

        /// <summary>
        /// 文件名称
        /// </summary>
        private string Picname { get { return this.GetQueryString("picname", System.DateTime.Now.ToString("yyyyMMddHHmmss") + StringHelper.CreateCheckCodeWithNum(6)); } }


        /// <summary>
        /// 判断上传来源true 表示来源于jupload
        /// </summary>
        private bool UploadType { get { return this.GetQueryString("uploadtype", 0) > 0; } }

        private bool imageUpType { get { return this.GetQueryString("imageuptype", 0) > 0; } }

        private string ClientUserPath
        {
            get { return this.GetQueryString("userid", "pic") + "/"; }
        }


        /// <summary>
        ///上传图片的大小，单位KB 默认4M
        /// </summary>
        private int MaxSize
        {
            get { return this.GetQueryString("m", 40000); }
        }

        /// <summary>
        /// 图片的宽度 0表示：不限制
        /// </summary>
        private int Width
        {
            get { return this.GetQueryString("w", 0); }
        }

        /// <summary>
        /// 图片高度 0表示：不限制
        /// </summary>
        private int Height
        {
            get { return this.GetQueryString("h", 0); }
        }
        /// <summary>
        /// 正方形
        /// </summary>
        private bool Square
        {
            get
            {
                return this.GetQueryString("square", 0) > 0;
            }
        }

        public int MaxThumbWidth { get; set; }

        public int MaxThumbHeight { get; set; }

        public int UseThumb { get; set; }
        #endregion

        public HttpContext ctx { get; set; }



        public new void ProcessRequest(HttpContext context)
        {
            ctx = context;
           // context.Response.ContentType = "application/json";

            #region 其他上传
            string disposition = context.Request.ServerVariables["HTTP_CONTENT_DISPOSITION"];
            //------------------------------------------------------------------------------
            #region 初始文件保存路径 文件类型

            switch (context.Request.QueryString["type"])
            {
                case "image":
                    _sAllowType = ".jpg|.gif|.png|.jpeg|.bmp|.ico";
                    break;
                case "file":
                    _sAllowType = ".rar|.txt|.doc|.zip";
                    break;
                case "flash":
                    _sAllowType = ".swf";
                    break;
                case "media":
                    _xheditorPath = System.Configuration.ConfigurationManager.AppSettings["uploadMusic"];
                    _sAllowType = ".avi|.rm|.wmv|.cd|.rmvb|.dvd|.mp3|.wma|.wav";
                    break;
                default:
                    _sAllowType = ".jpg|.gif|.png|.jpeg|.bmp|.ico|.pfx|.p12";
                    break;
            }
            _xheditorPath = Path.Combine(_xheditorPath, ClientUserPath, ClientPath);
            #endregion


            string err = "";
            string msg = "''";
            string responseMsg = string.Empty;
            string localname = "";
            byte[] fileByte;
            if (disposition != null)
            {
                #region HTML5上传
                fileByte = context.Request.BinaryRead(context.Request.TotalBytes);
                localname = Regex.Match(disposition, "filename=\"(.+?)\"").Groups[1].Value;// 读取原始文件名
                err = CheckFile(localname, fileByte.Length);
                if (err == "")
                {
                    string FileName = _xheditorPath + Picname + Path.GetExtension(localname);
                    if (!IsBuildThumbnail ? FileUploadHelper.UploadFile(fileByte, FileName) : FileUploadHelper.UploadPicFileAndThumbnail(fileByte, FileName, ThmbnailW, ThmbnailH, ThumbnailMode.W))
                    {
                        if (iMsgType == 1) msg = "'" + GetFileUrl(FileName) + "'";
                        else msg = "{'url':'" + GetFileUrl(FileName) + "','localname':'" + jsonString(localname) + "','id':'1'}";
                    }
                    else
                    {
                        err = "上传文件失败。";
                        responseMsg = "{'err':'" + jsonString(err) + "','msg':" + msg + "}";
                    }

                }
                #endregion
            }
            else
            {
                //获取上传文件队列   
                HttpPostedFile oFile = context.Request.Files[0];
                #region 默认上传
                err = CheckFile(oFile.FileName, oFile.ContentLength);
                if (err == "")
                {
                    string FileName = _xheditorPath + Picname + Path.GetExtension(oFile.FileName);
                    if (Width > 0)
                    {
                        System.Drawing.Image _bmp = System.Drawing.Image.FromStream(oFile.InputStream);
                        int bw = _bmp.Width;   //判断W*H
                        int bh = _bmp.Height;
                        if (Square ? (bw != bh) : (bw > Width))
                        {
                            err = Square ? "请上传正方形图片" : "上传文件宽度超过" + Width + "。";
                            responseMsg = "{'err':'" + jsonString(err) + "','msg':" + msg + "}";
                        }
                        else if (Square ? (bw != bh) : (bh > Height))
                        {
                            err = Square ? "请上传正方形图片" : "上传文件高度超过" + Height + "。";
                            responseMsg = "{'err':'" + jsonString(err) + "','msg':" + msg + "}";
                        }
                        else
                        {
                            if (!IsBuildThumbnail ? FileUploadHelper.UploadFile(oFile, FileName) : FileUploadHelper.UploadPicFileAndThumbnail(oFile, FileName, ThmbnailW, ThmbnailH, ThumbnailMode.W))
                            {
                                if (UploadType)
                                {
                                    responseMsg = "{success: true,fileUrl:'" + FileName + "',size:'" + bw + "x" + bh + "'}";
                                }
                                else
                                {
                                    if (iMsgType == 1) msg = "'" + GetFileUrl(FileName) + "'";
                                    else msg = "{'url':'" + GetFileUrl(FileName) + "','localname':'" + jsonString(oFile.FileName) + "','id':'1'}";

                                    responseMsg = "{'err':'" + jsonString(err) + "','msg':" + msg + "}";
                                }
                            }
                            else
                            {
                                if (UploadType)
                                {
                                    responseMsg = "{ success: false, fileUrl:'',err:'上传失败'  }";
                                }
                                else
                                {
                                    err = "上传文件失败。";
                                    responseMsg = "{'err':'" + jsonString(err) + "','msg':" + msg + "}";
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!IsBuildThumbnail ? FileUploadHelper.UploadFile(oFile, FileName) : FileUploadHelper.UploadPicFileAndThumbnail(oFile, FileName, ThmbnailW, ThmbnailH, ThumbnailMode.W))
                        {
                            if (UploadType)
                            {
                                if (context.Request.QueryString["type"] == "image")
                                {
                                    System.Drawing.Image _bmp = System.Drawing.Image.FromStream(oFile.InputStream);
                                    responseMsg = "{success:true,fileUrl:'" + FileName + "',size:'" + _bmp.Width + "x" + _bmp.Height + "'}";
                                }
                                else
                                {
                                    responseMsg = "{success:true,fileUrl:'" + FileName + "'}";
                                }
                            }
                            else
                            {
                                if (iMsgType == 1) msg = "'" + GetFileUrl(FileName) + "'";
                                else msg = "{'url':'" + GetFileUrl(FileName) + "','localname':'" + jsonString(oFile.FileName) + "','id':'1'}";

                                responseMsg = "{'err':'" + jsonString(err) + "','msg':" + msg + "}";
                            }
                        }
                        else
                        {
                            if (UploadType)
                            {
                                responseMsg = "{ success: false, fileUrl:'',err:'上传失败' }";
                            }
                            else
                            {
                                err = "上传文件失败。";
                                responseMsg = "{'err':'" + jsonString(err) + "','msg':" + msg + "}";
                            }
                        }
                    }
                }
                #endregion
            }
            context.Response.Charset = "utf-8";
            context.Response.Write(responseMsg);
            #endregion
        }

        public new bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 检测文件是否符合要求
        /// </summary>
        /// <param name="oFile"></param>
        /// <param name="iMaxSize"></param>
        /// <returns></returns>
        protected string CheckFile(string filename, int contentlength)
        {
            string err = "";
            if (contentlength == 0)
            {
                err = "无数据提交";
            }
            else
                if (MaxSize != 0 && MaxSize < contentlength / 1024)
            {
                if (MaxSize > 1024)
                {
                    err = "文件大小限制为" + (MaxSize / 1024) + "MB以内";
                }
                else
                {
                    err = "文件大小限制为" + MaxSize + "KB以内";
                }
            }
            else
            {
                if (!FileUploadHelper.CheckFileExt(filename, _sAllowType))
                {
                    err = "上传文件扩展名必需为：" + _sAllowType;
                }
            }
            return err;
        }

        protected string jsonString(string str)
        {
            str = str.Replace("\\", "\\\\");
            str = str.Replace("/", "\\/");
            str = str.Replace("'", "\\'");
            return str;
        }

        private string GetFileUrl(string sFileFullName)
        {
            if (!string.IsNullOrEmpty(sFileFullName))
            {
                string urlPath = ctx.Request.Url.AbsoluteUri;
                string _urlpath = urlPath.Substring(0, urlPath.LastIndexOf("/"));

                return _urlpath + "/" + sFileFullName.Replace("~", "");
            }
            return string.Empty;
        }
    }
}