using System;
using Apps.Models;

namespace Apps.IDAL.DEF
{
    public partial interface IDEF_TestCaseRelationRepository
    {
        DEF_TestCaseRelation GetById(string pcode, string ccode);

        int GetTestCaseRelationByCode(string pcode);
        int DeleteByPcodeCcode(string pcode, string ccode);
    }
}
