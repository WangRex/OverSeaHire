/**
* 命名空间: Apps.Models.Sys
*
* 功 能： N/A
* 类 名： SysAttachViewModel
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
* V0.01 2017-12-20 18:48:38 王仁禧 初版
*
* Copyright (c) 2017 Lir Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：大连安琪科技有限公司 　　　　　　　　　　　　　　       │
*└──────────────────────────────────┘
*/
using System.ComponentModel.DataAnnotations;

namespace Apps.Models.Sys
{
    /// <summary>
    /// 附件
    /// </summary>
    public class SysAttachViewModel
    {

        [Display(Name = "主键")]
        public string Id { get; set; }

        [Display(Name = "业务")]
        public string BusinessID { get; set; }

        [Display(Name = "附件")]
        public string AttachPath { get; set; }

        [Display(Name = "文件名")]
        public string FileName { get; set; }

        [Display(Name = "后缀")]
        public string ExtName { get; set; }

        [Display(Name = "类型")]
        public string EnumType { get; set; }

        public override string ToString()
        {
            return "主键:" + Id + ",业务:" + BusinessID + ",附件:" + AttachPath + ",文件名:" + FileName + ",后缀:" + ExtName + ",类型:" + EnumType;
        }
    }
}
