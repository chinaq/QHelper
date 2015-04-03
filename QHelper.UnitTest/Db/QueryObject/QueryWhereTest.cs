using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHelper.UnitTest.Db.QueryObject
{
    [TestClass]
    public class QueryWhereTest
    {
        [TestMethod]
        public void Test_GetClause() { 
            //Arrange
            WhereQuery query = new WhereQueryCom();
            WhereCriteria cri1 = new WhereCriteria();
            WhereCriteria cri2 = new WhereCriteria();

            query.Add(cri1);
            query.Add(cri2);

            //Action
            WhereClause clause = query.GeneralClause();

            //Assert
            Assert.AreEqual("Name = @0 AND Point = @1", clause.sqlStr);
        }
    }
}
