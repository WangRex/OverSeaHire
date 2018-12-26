/**
* 命名空间: Apps.Common
*
* 功 能： N/A
* 类 名： LambdaHelper
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
* V0.01 2017-9-6 16:47:37 王仁禧 初版
*
* Copyright (c) 2017 Lir Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：大连安琪科技有限公司 　　　　　　　　　　　　　　       │
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Common
{
    public static class LambdaHelper
    {
        // var query = people.DistinctBy(p => p.Id);
        // var query = people.DistinctBy(p => new { p.Id, p.Name });
        /// <summary>
        /// 去重复数据
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector">用于去重的表达式，单个字段如：var query = people.DistinctBy(p => p.Id);多个字段如：var query = people.DistinctBy(p => new { p.Id, p.Name });</param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
