var Constants = (function (con) {
    con.globalFlashUrl = "http://dltongjian.net:8005/";
    con.globalHomeUrl = "http://dltongjian.net:8006/";
    con.Employee = {
        StatisticEmployee: "/api/Employee/StatisticEmployee",
    };
    con.Attach = {
        GetAttachList: "api/Attach/GetAttachList",
        GetAttachListGrid: "api/Attach/GetAttachListGrid",
        DeleteAttach: "api/Attach/DeleteAttach",
        ImageUpload: "api/ImageUpload/PostFile",
    };
    return con;
}(Constants || {}));