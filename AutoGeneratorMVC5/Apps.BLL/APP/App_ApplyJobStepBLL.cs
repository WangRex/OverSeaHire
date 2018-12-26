using Apps.Common;
using Apps.Models;
using System.Linq;
using System.Collections.Generic;
using System.Linq;
using Apps.Models.App;

namespace Apps.BLL.App
{
    public partial class App_ApplyJobStepBLL
    {

        #region 获取名称
        /// <summary>
        /// 获取名称
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string GetName(string ID)
        {
            var Name = string.Empty;
            var _App_ApplyJobStepModel = GetById(ID);
            if (null != _App_ApplyJobStepModel)
            {
                Name = _App_ApplyJobStepModel.Name;
            }
            return Name;
        }
        #endregion

        #region 获取应聘步骤
        /// <summary>
        /// 获取应聘步骤
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<App_ApplyJobStepVm> GetApplyStep(string UserId, ref string ErrorMsg)
        {
            List<App_ApplyJobStepVm> app_ApplyJobStepVms = new List<App_ApplyJobStepVm>();
            var list = m_Rep.FindList().OrderBy(EF => EF.Step).ToList();
            if (list == null || list.Count == 0)
            {
                ErrorMsg = "获取配置应聘步骤为空";
                return null;
            }
            foreach (var item in list)
            {
                App_ApplyJobStepVm app_ApplyJobStepVm = new App_ApplyJobStepVm();
                app_ApplyJobStepVm.Id = item.Id;
                app_ApplyJobStepVm.Name = item.Name;
                app_ApplyJobStepVm.Icon = item.Icon;
                app_ApplyJobStepVm.Description = item.Description;
                app_ApplyJobStepVm.Step = item.Step;
                app_ApplyJobStepVms.Add(app_ApplyJobStepVm);
            }
            ErrorMsg = "获取配置应聘步骤成功";
            return app_ApplyJobStepVms;
        }
        #endregion

        #region 获取步骤对应名称
        /// <summary>
        /// 获取步骤对应名称
        /// </summary>
        /// <param name="Step"></param>
        /// <returns></returns>
        public string GetStepName(string Step)
        {
            string StepName = "";
            var ApplyJobStep = m_Rep.Find(EF => EF.Step == Step);
            if (null != ApplyJobStep)
            {
                StepName = ApplyJobStep.Name;
            }
            return StepName;
        }
        #endregion
    }
}

