using System;
using System.Linq;
using Apps.IDAL.Flow;
using Apps.Models;
using System.Data;

namespace Apps.DAL.Flow
{
    public partial class Flow_FormContentStepCheckRepository
    {

        public IQueryable<Flow_FormContentStepCheck> GetListByFormId(string formId,string contentId)
        {
            IQueryable<Flow_FormContentStepCheck> list = from a in Context.Flow_FormContentStepCheck
                                                         join b in Context.Flow_Step
                                                         on a.StepId equals b.Id
                                                         where b.FormId == formId & a.ContentId==contentId
                                                         select a;
            return list;
        }
    
        public void ResetCheckStateByFormCententId(string stepCheckId, string contentId, int checkState, int checkFlag)
        {
            Context.P_Flow_ResetCheckStepState(stepCheckId, contentId, checkState, checkFlag);
        }
    }
}
