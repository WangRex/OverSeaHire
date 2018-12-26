var globalUtils = (function (gu) {
    gu.globalAjax = function (url, data, successCallback, type, dataType, async, params) {
        console.log('接口:' + url);
        console.log('类型:' + type || "get");
        console.log('输入:' + JSON.stringify(data) || {});
        $.ajax({
            type: type || "get",
            dataType: dataType || "json",
            url: url,
            data: data || guid,
            cache: false,
            async: async || "false",
            beforeSend: function (request) {
                showLoading();
            },
            success: function (result) {
                hideLoading();
                if (successCallback) {
                    successCallback(result, params || {});
                }
            },
            complete: function () {
            }
        });
    }
    gu.isArray = function (obj) {
        return Object.prototype.toString.call(obj) === '[object Array]';
    }
    gu.getNowFormatDate = function () {
        var date = new Date();
        var seperator1 = "-";
        var seperator2 = ":";
        var month = date.getMonth() + 1;
        var strDate = date.getDate();
        if (month >= 1 && month <= 9) {
            month = "0" + month;
        }
        if (strDate >= 0 && strDate <= 9) {
            strDate = "0" + strDate;
        }
        var currentdate = date.getFullYear() + seperator1 + month + seperator1 + strDate + " " + date.getHours() + seperator2 + date.getMinutes() + seperator2 + date.getSeconds();
        return currentdate;
    }
    gu.fillinInfoFromTmpl = function (result, params) {
        //模板如果想显示带有HTML标签的内容，就在显示前边加上#
        //例如 {{#value.BodyContent}}
        var html = template(params.tmplId, result);
        switch (params.way) {
            case "after":
                params.target.after(html);
                break;
            case "append":
                params.target.append(html);
                break;
            case "appendTo":
                params.target.appendTo(html);
                break;
            case "html":
                params.target.html(html);
                break;
            case "prepend":
                params.target.prepend(html);
            default:
                break;
        }
        if (params.callback) {
            if (params.callbackParams) {
                params.callback(params.callbackParams);
            } else {
                params.callback();
            }
        }
    }
    gu.arrayToString = function (array) {
        var result = array || '';
        if (gu.isArray(array) && array.length > 1) {
            result = "";
            for (var i = 0; i < array.length; i++) {
                result += "," + array[i];
            }
            result = result.substring(1);
        }
        return result;
    }
    gu.stringToArray = function (string) {
        var result = new Array();
        if (string) {
            if (string.indexOf(',') != -1) {
                result = string.split(',');
            } else {
                result = result.push(string);
            }
        }
        return result;
    }
    gu.CKupdate = function () {
        for (instance in CKEDITOR.instances)
            CKEDITOR.instances[instance].updateElement();
    }
    gu.loadPage = function (parentId, pageName, callback, params) {
        $("#" + parentId || "main-content").load(pageName + ".html", function (response, status, xhr) {
            if (callback) {
                callback(params);
            }

        });
    }
    gu.getUrlParam = function (params) {
        var reg = new RegExp("(^|&)" + params + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
        var r = window.location.search.substr(1).match(reg); //匹配目标参数
        if (r != null) return unescape(r[2]);
        return null; //返回参数值
    }
    gu.GoPage = function (path) {
        window.location.href = path;
    }
    gu.DownloadAttach = function (path, name) {
        var extIndex = path.lastIndexOf(".");
        var ext = path.substring(extIndex);
        var $a = $("<a></a>").attr("href", path).attr("download", name + ext);
        $a[0].click();
    }
    gu.NumberFormatter = function (number) {
        if (isNaN(number) || !number) {
            return "";
        }
        if (0 == Number(number)) {
            return "0";
        }
        number = number.replace(/\,/g, "");
        number = Math.round(number * 100) / 100;
        if (number < 0)
            return '-' + this.FormatterInt(Math.floor(Math.abs(number) - 0) + '') + this.FormatterIntCents(Math.abs(number) - 0);
        else
            return this.FormatterInt(Math.floor(number - 0) + '') + this.FormatterIntCents(number - 0);
    }
    gu.FormatterInt = function (number) {
        if (number.length <= 3)
            return (number == '' ? '0' : number);
        else {
            var mod = number.length % 3;
            var output = (mod == 0 ? '' : (number.substring(0, mod)));
            for (i = 0; i < Math.floor(number.length / 3) ; i++) {
                if ((mod == 0) && (i == 0))
                    output += number.substring(mod + 3 * i, mod + 3 * i + 3);
                else
                    output += ',' + number.substring(mod + 3 * i, mod + 3 * i + 3);
            }
            return (output);
        }
    }
    gu.FormatterIntCents = function (amount) {
        amount = Math.round(((amount) - Math.floor(amount)) * 100);
        return (amount < 10 ? '.0' + amount : '.' + amount);
    }
    return gu;
}(globalUtils || {}));
