/**
* 命名空间: Apps.Models.Sys
*
* 功 能： N/A
* 类 名： EnumDictionaryViewModel
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
* V0.01 2017-11-13 15:46:49 王仁禧 初版
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
    public class EnumDictionaryViewModel
    {
        [Display(Name = "主键")]
        public int Id { get; set; }

        [Display(Name = "表名")]
        public string TableName { get; set; }

        [Display(Name = "名字")]
        public string ItemName { get; set; }

        [Display(Name = "值")]
        public string ItemValue { get; set; }

        [Display(Name = "枚举照片")]
        public string ItemPhoto { get; set; }
    }
}
