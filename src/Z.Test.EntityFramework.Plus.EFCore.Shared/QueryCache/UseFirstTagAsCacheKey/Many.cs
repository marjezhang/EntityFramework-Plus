﻿// Description: Entity Framework Bulk Operations & Utilities (EF Bulk SaveChanges, Insert, Update, Delete, Merge | LINQ Query Cache, Deferred, Filter, IncludeFilter, IncludeOptimize | Audit)
// Website & Documentation: https://github.com/zzzprojects/Entity-Framework-Plus
// Forum & Issues: https://github.com/zzzprojects/EntityFramework-Plus/issues
// License: https://github.com/zzzprojects/EntityFramework-Plus/blob/master/LICENSE
// More projects: http://www.zzzprojects.com/
// Copyright © ZZZ Projects Inc. 2014 - 2016. All rights reserved.

using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.EntityFramework.Plus;

namespace Z.Test.EntityFramework.Plus
{
    public partial class QueryCache_UseFirstTagAsCacheKey
    {
        [TestMethod]
        public void Many()
        {
            TestContext.DeleteAll(x => x.Entity_Basics);
            TestContext.Insert(x => x.Entity_Basics, 3);

            using (var ctx = new TestContext())
            {
                var tags = new string[] { "zzzprojects", "tag2", "tag3" };

                StringBuilder cacheKey = new StringBuilder();
                cacheKey.AppendLine(QueryCacheManager.CachePrefix);
                cacheKey.AppendLine(QueryCacheManager.GetConnectionStringForCacheKey(ctx.Entity_Basics));
                cacheKey.AppendLine(tags[0]);

                var query = ctx.Entity_Basics.Where(x => x.ColumnInt > 0);

                var cacheKey1 = QueryCacheManager.GetCacheKey(query, new string[0]);
                QueryCacheManager.UseFirstTagAsCacheKey = true;
                var cacheKey2 = QueryCacheManager.GetCacheKey(query, tags);
                QueryCacheManager.UseFirstTagAsCacheKey = false;

                // Cache key are different
                Assert.AreNotEqual(cacheKey1, cacheKey2);

                // Cache key2 is equal to hardcoded cacheKey
                Assert.AreEqual(cacheKey.ToString(), cacheKey2);
            }
        }
    }
}