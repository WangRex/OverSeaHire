//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Apps.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DEF_TestJobs
    {
        public DEF_TestJobs()
        {
            this.DEF_Defect = new HashSet<DEF_Defect>();
            this.DEF_TestJobsDetail = new HashSet<DEF_TestJobsDetail>();
        }
    
        public string VerCode { get; set; }
        public string Name { get; set; }
        public Nullable<bool> Result { get; set; }
        public string Description { get; set; }
        public string Creator { get; set; }
        public Nullable<System.DateTime> CrtDt { get; set; }
        public Nullable<bool> CloseState { get; set; }
        public string Closer { get; set; }
        public Nullable<System.DateTime> CloseDt { get; set; }
        public Nullable<bool> Def { get; set; }
        public Nullable<bool> CheckFlag { get; set; }
    
        public virtual ICollection<DEF_Defect> DEF_Defect { get; set; }
        public virtual ICollection<DEF_TestJobsDetail> DEF_TestJobsDetail { get; set; }
    }
}
