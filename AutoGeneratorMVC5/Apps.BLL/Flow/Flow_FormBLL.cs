﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Apps.Models;
using Apps.Common;
using System.Transactions;
using Apps.Models.Flow;
using Apps.IBLL.Flow;
using Apps.IDAL.Flow;
using Apps.BLL.Core;
using Apps.Locale;

namespace Apps.BLL.Flow
{
    public partial class Flow_FormBLL
    {

        [Dependency]
        public IFlow_TypeRepository typeRep { get; set; }

        public List<Flow_FormModel> GetListByTypeId(string typeId)
        {
            IQueryable<Flow_Form> queryData = m_Rep.GetList(a => a.TypeId == typeId);
            return CreateModelList(ref queryData);

        }

    }
}
