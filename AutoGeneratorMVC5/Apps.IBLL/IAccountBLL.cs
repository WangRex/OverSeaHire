using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apps.Models;
using Apps.Models.Sys;

namespace Apps.IBLL
{
    public partial interface IAccountBLL
    {
        SysUser Login(string username, string pwd, ref string ErrorMsg);
    }
}
