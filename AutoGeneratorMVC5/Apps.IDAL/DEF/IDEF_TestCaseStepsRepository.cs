using System;
using Apps.Models.DEF;
using Apps.Models;
namespace Apps.IDAL.DEF
{
    public partial interface IDEF_TestCaseStepsRepository
    {

        int Edit(DEF_TestCaseStepsModel model);
        string GetNameById(string id);
    }
}
