/*
    版权所有:杭州火图科技有限公司
    地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
    (c) Copyright Hangzhou Hot Technology Co., Ltd.
    Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
    2013-2016. All rights reserved.
**/

function goTo(pageNo, callback) {
    callback(pageNo);
}
$.extend({});

$.fn.extend({
    trim: function () {
        return $.trim(this.val());
    },
    lTrim: function () {
        return this.val().replace(/^\s+/, '');
    },
    rTrim: function () {
        return this.val().replace(/\s+$/, '');
    },

    setDisabled: function (disabled) {
        return this.each(function () {
            $(this).attr('disabled', disabled).css('opacity', disabled ? 0.5 : 1.0);
        });
    },
    setReadOnly: function (readonly) {
        return this.each(function () {
            $(this).attr('readonly', readonly).css('opacity', readonly ? 0.5 : 1.0);
        });
    },
    setChecked: function (checked, value) {
        return this.each(function () {
            if (value == undefined) {
                $(this).attr('checked', checked);
            } else if ($(this).val() == value.toString()) {
                $(this).attr('checked', checked);
            }
        });
    }
});



var hotUtil = hotUtil || {};

$.extend(hotUtil, {
    GetCookie: function (name) {
        var r = new RegExp('(^|;|\\s+)' + name + '=([^;]*)(;|$)');
        var m = document.cookie.match(r);
        return (!m ? '' : decodeURIComponent(m[2]));
    },
    SetCookie: function (name, value, expire, domain, path) {
        var s = name + '=' + encodeURIComponent(value);
        if (!J.IsUndefined(path))
            s = s + '; path=' + path;
        if (expire > 0) {
            var d = new Date();
            d.setTime(d.getTime() + expire * 1000);
            if (!J.IsUndefined(domain))
                s = s + '; domain=' + domain;
            s = s + '; expires=' + d.toGMTString();
        }
        document.cookie = s;
    },
    RemoveCookie: function (name, domain, path) {
        var s = name + '=';
        if (!J.IsUndefined(domain))
            s = s + '; domain=' + domain;
        if (!J.IsUndefined(path))
            s = s + '; path=' + path;
        s = s + '; expires=Fri, 02-Jan-1970 00:00:00 GMT';
        document.cookie = s;
    },

    isInt: function (str) { return /^-?\d+$/.test(str); },
    isNumber: function (obj) { return typeof obj == 'number'; },
    isFloat: function (str) { return /^(-?\d+)(\.\d+)?$/.test(str); },
    isEmail: function (str) { return /^[A-Z_a-z0-9-\.]+@([A-Z_a-z0-9-]+\.)+[a-z0-9A-Z]{2,4}$/.test(str); },
    isMobile: function (str) { return /^(1)\d{10}$/.test(str); },
    encode: function (str) { return encodeURIComponent(str); },
    decode: function (str) { return decodeURIComponent(str); },
    /*
    * @brief 判断字符是否为null或空或undefined
    */
    isNullOrEmpty: function (str) {
        if (typeof str != 'undefined' && str != null && str.length > 0)
            return false;
        else
            return true;
    },
    newTab: function (url, name) {
        if (name.length > 25) {
            name = name.substring(0, 25) + "...";
        }
        parent.newTab(url, name);
    },
    /*
     * @brief 跳转登录页面
     */
    redirectLogin: function (obj) {
        if (obj)
            parent.parent.window.location.href = "/login.html";
        else
            parent.window.location.href = "/login.html";
    },
    tip: {
        success: function (content, callback, time) {
            var $msg = $('<div class="hottip-wrap"><div class="layui-layer layui-layer-dialog layui-layer-border layui-layer-msg layui-layer-hui layer-anim hottip-content" style="z-index: 99891018;top: 30%;position: relative;background-color: rgba(28, 175, 154, 0.8);"><div id="" class="layui-layer-content">' + content + '</div></div></div>');
            layer.closeAll("loading");
            $("body").append($msg);

            if (typeof time == 'undefined') {
                time = 1500;
            }

            setTimeout(function () {
                $msg.remove();
                if (typeof callback == 'function')
                    callback();
            }, time);
        },
        error: function (content, callback, time) {
            if (typeof time == 'undefined') {
                time = 3000;
            }

            layer.closeAll("loading");
            var $msg = $('<div class="hottip-wrap"><div class="layui-layer layui-layer-dialog layui-layer-border layui-layer-msg layui-layer-hui layer-anim hottip-content" style="z-index: 99891018;top: 30%;position: relative;background-color: rgba(217, 82, 79, 0.8);"><div id="" class="layui-layer-content">' + content + '</div></div></div>');
            $("body").append($msg);
            setTimeout(function () {
                $msg.remove();
                if (typeof callback == 'function')
                    callback();
            }, time);
        },
        msg: function (content, callback, time) {
            if (typeof time == 'undefined') {
                time = 3000;
            }

            layer.closeAll("loading");
            var $msg = $('<div class="hottip-wrap"><div class="layui-layer layui-layer-dialog layui-layer-border layui-layer-msg layui-layer-hui layer-anim hottip-content" style="z-index: 99891018;top: 30%;position: relative;"><div id="" class="layui-layer-content">' + content + '</div></div></div>');
            $("body").append($msg);
            setTimeout(function () {
                $msg.remove();
                if (typeof callback == 'function')
                    callback();
            }, time);
        }
    },
    loading: {
        show: function () {
            return layer.load()
        },
        close: function () {
            layer.closeAll('loading');
        }
    },
    /*
    * @brief 获得页面参数
    * @param 参数名
    * */
    getQuery: function (name) {
        var strHref = window.document.location.href;
        var intPos = strHref.indexOf("?");
        var strRight = strHref.substr(intPos + 1);
        var arrTmp = strRight.split("&");
        for (var i = 0; i < arrTmp.length; i++) {
            var arrTemp = arrTmp[i].split("=");
            if (arrTemp[0].toUpperCase() == name.toUpperCase()) return arrTemp[1];
        }
        if (arguments.length == 1)
            return "";
        if (arguments.length == 2)
            return arguments[1];
    },
    /*
     * @brief ajaxPost请求
     * @param data 请求数据
     * @param callback 回调函数(ret,err)
     * */
    ajaxCall: function (url, data, callback) {
        var op = {
            url: url,
            data: data,
            type: "post",
            dataType: "json",
            success: function (ret) {
                if (typeof callback == "function") {
                    if (typeof ret == "undefined")
                        ret = null;
                    callback(ret, null);
                }
            },
            error: function (err) {
                if (typeof callback == "function") {
                    if (typeof err == "undefined")
                        err = null;
                    callback(null, err);
                }
            }
        };
        $.ajax(op);
    },
    /*
     * @brief ajaxGet请求
     * @param data 请求数据
     * @param callback 回调函数(ret,err)
     * */
    ajaxCallGet: function (url, data, callback) {
        var op = {
            url: url,
            data: data,
            type: "get",
            dataType: "json",
            success: function (ret) {
                if (typeof callback == "function") {
                    if (typeof ret == "undefined")
                        ret = null;
                    callback(ret, null);
                }
            },
            error: function (err) {
                if (typeof callback == "function") {
                    if (typeof err == "undefined")
                        err = null;
                    callback(null, err);
                }
            }
        };
        $.ajax(op);
    },
    /*
     * @brief 序列化表单
     * @param obj 序列化对象
     */
    serializeForm: function (obj) {
        var data = {};
        $(obj).each(function (idx, element) {
            data[element.id] = $(element).val();
        });
        return data;
    },
    /*
     * @brief 手机或身份证号部分隐藏
     * @param value 值
     */
    subStringText: function (value) {
        if (value != null) {
            var result = "";
            if (value.length == 11) {
                for (var i = 0; i < value.length; i++) {
                    result += (i < 3 || i > 8) ? value.substr(i, 1) : "*";
                }
            }
            else if (value.length == 18) {
                for (var i = 0; i < value.length; i++) {
                    result += (i < 3 || i > 15) ? value.substr(i, 1) : "*";
                }
            }
            else if (value.length == 15) {
                for (var i = 0; i < value.length; i++) {
                    result += (i < 3 || i > 12) ? value.substr(i, 1) : "*";
                }
            }
            return result;
        }
        return "";
    },
    /*
     * @brief 手机或身份证号部分隐藏
     * @param el 操作对象
     * @param disCls 禁用样式名
     */
    LastTime: function (el, disCls) {
        if (el.length < 1)
            return;
        var oldText = el.html();
        var functionName = el.attr("onclick");
        el.removeAttr("onclick", "");
        if (el.hasClass(disCls)) {
            el.addClass("disabled");
            el.removeClass(disCls);
            var time = 60;
            var elTime = el.find("span");
            if (elTime.length == 0) {
                el.html("<span>" + time + "</span>秒后重发");
                elTime = el.find("span");
            }
            var timer = setInterval(function () {
                if (time > 1) {
                    var str = time - 1;
                    time = time - 1;
                    elTime.html(str);
                } else {
                    str = "";
                    el.html(oldText);
                    el.addClass(disCls);
                    el.removeClass("disabled");
                    if (functionName) { el.attr("onclick", functionName); }
                    clearInterval(timer);
                }

            }, 1000);
        } else {
            el.addClass(disCls);
            var time = 60;
            var elTime = el.find("span");
            if (elTime.length == 0) {
                el.html("<span>" + time + "</span>秒后重发");
                elTime = el.find("span");
            }
            var timer = setInterval(function () {
                if (time > 1) {
                    var str = time - 1;
                    time = time - 1;
                    elTime.html(str);
                } else {
                    str = "";
                    el.html(oldText);
                    if (functionName) { el.attr("onclick", functionName); }
                    el.removeClass(disCls);
                    clearInterval(timer);
                }

            }, 1000)
        }
    },
    /*
     * @brief 分页函数
     * @param loadObj 操作对象
     * @param pageNo 当前页
     * @param pageSize 每页数量
     * @param totalPages 总页数
     * @param recordCount 总数
     * @param btnCount 按钮数量
    */
    paging: function (loadObj, pageNo, pageSize, totalPages, recordCount, btnCount) {
        this.currentBtnPage = 0;
        this.obj = $(loadObj);
        this.pageNo = pageNo;
        this.totalPages = totalPages;
        this.btnCount = btnCount || 7;

        $(loadObj + "_pageIndex").text(pageNo);
        $(loadObj + "_recordCount").text(recordCount);
        $(loadObj + "_pageCount").text(totalPages == 0 ? 1 : totalPages);
        $(loadObj + "_pageSize").text(pageSize);


        this.init = function (callback) {
            if (this.obj && recordCount > 0) {
                this.obj.html('');
                if (this.totalRecords == 0) {
                    this.obj.html('');
                    return;
                }
                if (this.pageNo >= totalPages) {
                    this.pageNo = totalPages;
                }
                if (this.pageNo <= 1) {
                    this.pageNo = 1;
                }

                if (this.pageNo > 1) {
                    //输出首页和上一页按钮
                    this.obj.append('<li ' + (this.pageNo == 1 ? 'class="disabled"' : '') + '><a href="javascript:goTo(1,' + callback + ')" aria-label="Previous"><span aria-hidden="true">&laquo;</span></a></li>');
                    this.obj.append('<li ' + (this.pageNo == 1 ? 'class="disabled"' : '') + '><a href="javascript:goTo(' + (pageNo - 1) + ',' + callback + ')"><i class="fa fa-angle-left"></i></a></li>');
                }

                var btnStart = parseInt(this.btnCount / 2);//从哪个索引开始变换
                this.btnCount = Math.min(totalPages, this.btnCount);

                if (this.pageNo > this.totalPages - btnStart) {
                    this.currentBtnPage = this.totalPages - this.btnCount + 1;
                } else {
                    this.currentBtnPage = Math.max(this.pageNo - btnStart, 1);
                }

                //输出中间八个按钮
                for (var i = this.currentBtnPage; i < this.btnCount + this.currentBtnPage; i++) {
                    this.obj.append('<li ' + (this.pageNo == i ? 'class="active"' : '') + '><a href="' + (this.pageNo == i ? 'javascript:void(0);' : 'javascript:goTo(' + i + ',' + callback + ')') + '">' + i + '</a></li>');
                }

                //输出下一页和末页
                if (this.pageNo != totalPages) {
                    this.obj.append('<li ' + (pageNo == totalPages ? 'class="disabled"' : '') + '><a href="javascript:goTo(' + (pageNo + 1) + ',' + callback + ')"><i class="fa fa-angle-right"></i></a></li>');
                    this.obj.append('<li ' + (pageNo == totalPages ? 'class="disabled"' : '') + '><a href="javascript:goTo(' + totalPages + ',' + callback + ')" aria-label="Next"><span aria-hidden="true">&raquo;</span></a></li>');
                }
            }
        };
    },
    /*
     * @brief 上传图片
    */
    uploadImg: function (btnFile, uploadPath, callback, data) {
        var uploadUrl = '/handler/UploadFileEidt.ashx?uploadtype=1&userid=' + uploadPath + '';
        $.ajaxFileUpload({
            url: uploadUrl,
            secureuri: false,//安全协议
            fileElementId: btnFile,//id
            dataType: 'json',
            type: "get",
            data: data,
            error: function (data, status, e) {                
            },
            success: function (json) {                
                if (json.success) {                    
                    callback(json.fileUrl);
                } else {
                    callback(false);                    
                }
            }
        });
    }
});

