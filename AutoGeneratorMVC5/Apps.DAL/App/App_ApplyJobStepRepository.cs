using Apps.Models;
using Apps.Models.App;
using Apps.IDAL.App;
using System;
namespace Apps.DAL.App
{
    public partial class App_ApplyJobStepRepository
    {
        #region 获取步骤对应名称
        /// <summary>
        /// 获取步骤对应名称
        /// </summary>
        /// <param name="Step"></param>
        /// <returns></returns>
        public string GetStepName(string Step)
        {
            string StepName = "";
            var ApplyJobStep = Find(EF => EF.Step == Step);
            if (null != ApplyJobStep)
            {
                StepName = ApplyJobStep.Name;
            }
            return StepName;
        }
        #endregion
    }
}