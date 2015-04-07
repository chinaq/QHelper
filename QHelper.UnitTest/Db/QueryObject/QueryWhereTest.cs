using Microsoft.VisualStudio.TestTools.UnitTesting;
using QHelper.Db.QueryObject;
using QHelper.UnitTest.Db.QueryObject.EntitiesForTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Linq.Dynamic;



namespace QHelper.UnitTest.Db.QueryObject
{







    [TestClass]
    public class QueryWhereTest
    {







        #region Set_And_Clean
        [TestInitialize]
        public void setup()
        {
            DbClear();
        }


        [TestCleanup]
        public void CleanUp()
        {
            DbClear();
        }

        private void DbClear()
        {
            EFDbContext context = new EFDbContext();
            context.Videos.RemoveRange(context.Videos);
            context.SaveChanges();
        }
        #endregion







        #region 测试生成clause




        /// <summary>
        /// 一个query包含2个cri
        /// </summary>
        [TestMethod]
        public void Test_GeneralClause_With_One_Query_Contian_Two_Cris()
        { 
            //Arrange
            WhereQuery query = new WhereQueryCom(ConnectType.Non);
            WhereQuery cri1 = new WhereCriteria(ConnectType.Non, "Name", CriType.Equal, "骇客");
            WhereQuery cri2 = new WhereCriteria(ConnectType.And, "Point", CriType.Equal, 5);

            query.Add(cri1);
            query.Add(cri2);

            //Action
            WhereClause clause = query.GeneralClause();

            //Assert
            Assert.AreEqual("Name = @0 AND Point = @1 ", clause.sqlStr);
            Assert.AreEqual(2, clause.paras.Count);
        }


        /// <summary>
        /// 一个query包含2个cri和1个query
        /// </summary>
        [TestMethod]
        public void Test_GeneralClause_With_One_Query_Conctain_One_Query_And_Two_Cris()
        {
            //Arrange
            WhereQuery query_1 = new WhereQueryCom(ConnectType.Non);

            WhereQuery cri_1_1 = new WhereCriteria(ConnectType.Non, "Name", CriType.Equal, "骇客");
            WhereQuery cri_1_2 = new WhereCriteria(ConnectType.And, "Point", CriType.Equal, 5);
            WhereQuery query_1_3 = new WhereQueryCom(ConnectType.And);

            WhereQuery cri_1_3_1 = new WhereCriteria(ConnectType.Non, "Name", CriType.Equal, "白客");
            WhereQuery cri_1_3_2 = new WhereCriteria(ConnectType.And, "Point", CriType.Equal, 8);

            query_1.Add(cri_1_1);
            query_1.Add(cri_1_2);
            query_1.Add(query_1_3);

            query_1_3.Add(cri_1_3_1);
            query_1_3.Add(cri_1_3_2);

            //Action
            WhereClause clause = query_1.GeneralClause();

            //Assert
            Assert.AreEqual("Name = @0 AND Point = @1 AND (Name = @2 AND Point = @3 )", clause.sqlStr);
            Assert.AreEqual(4, clause.paras.Count);
        }




        /// <summary>
        /// 一个query包含2个cri和1个空的query
        /// </summary>
        [TestMethod]
        public void Test_GeneralClause_With_One_Query_Conctain_One_NullQuery_And_Two_Cris()
        {
            //Arrange
            WhereQuery query_1 = new WhereQueryCom(ConnectType.Non);

            WhereQuery cri_1_1 = new WhereCriteria(ConnectType.Non, "Name", CriType.Equal, "骇客");
            WhereQuery cri_1_2 = new WhereCriteria(ConnectType.And, "Point", CriType.Equal, 5);
            WhereQuery query_1_3 = new WhereQueryCom(ConnectType.And);

            query_1.Add(cri_1_1);
            query_1.Add(cri_1_2);
            query_1.Add(query_1_3);

            //Action
            WhereClause clause = query_1.GeneralClause();

            //Assert
            Assert.AreEqual("Name = @0 AND Point = @1 ", clause.sqlStr);
            Assert.AreEqual(2, clause.paras.Count);
        }



        /// <summary>
        /// 如果是第一个元素去除and或or
        /// </summary>
        [TestMethod]
        public void Test_GeneralClause_With_One_Query_Conctain_HeadWithAnd()
        {
            //Arrange
            WhereQuery query_1 = new WhereQueryCom(ConnectType.And);

            WhereQuery cri_1_1 = new WhereCriteria(ConnectType.And, "Name", CriType.Equal, "骇客");
            WhereQuery cri_1_2 = new WhereCriteria(ConnectType.And, "Point", CriType.Equal, 5);


            query_1.Add(cri_1_1);
            query_1.Add(cri_1_2);


            //Action
            WhereClause clause = query_1.GeneralClause();

            //Assert
            Assert.AreEqual("Name = @0 AND Point = @1 ", clause.sqlStr);
            Assert.AreEqual(2, clause.paras.Count);
        }

        #endregion






        #region 测试获取查询到的数据

        /// <summary>
        /// 使用query通过ef获取到1个Video实例
        /// </summary>
        [TestMethod]
        public void Test_GetDataFromEf_Find_1() {
            //Arrange   //Add to db
            Video videoToAdd = new Video()
            {
                Id = "123",
                Name = "骇客",
                Point = 5
            };
            EFDbContext contextToAdd = new EFDbContext();
            contextToAdd.Videos.Add(videoToAdd);
            contextToAdd.SaveChanges();

            EFDbContext context = new EFDbContext();
            List<Video> videos = context.Videos.ToList();
            Assert.AreEqual(1, videos.Count);


            //Arrange2      //Set query
            WhereQuery query = new WhereQueryCom(ConnectType.Non);
            WhereQuery cri1 = new WhereCriteria(ConnectType.Non, "Name", CriType.Equal, "骇客");
            WhereQuery cri2 = new WhereCriteria(ConnectType.And, "Point", CriType.Equal, 5);

            query.Add(cri1);
            query.Add(cri2);


            //Action
            WhereClause clause = query.GeneralClause();
            var videosByQuery = context.Videos.Where(clause.sqlStr, clause.paras.ToArray());
            List<Video> videosGetted = videosByQuery.ToList();


            //Assert
            Assert.AreEqual(1, videosGetted.Count);        
        }








        /// <summary>
        /// 使用query通过ef获取到0个Video实例
        /// </summary>
        [TestMethod]
        public void Test_GetDataFromEf_Find_0() {
            //Arrange    //Add to db
            Video videoToAdd = new Video()
            {
                Id = "123",
                Name = "骇客",
                Point = 5
            };
            EFDbContext contextToAdd = new EFDbContext();
            contextToAdd.Videos.Add(videoToAdd);
            contextToAdd.SaveChanges();

            EFDbContext context = new EFDbContext();
            List<Video> videos = context.Videos.ToList();
            Assert.AreEqual(1, videos.Count);


            //Arrange2    //Set query
            WhereQuery query_1 = new WhereQueryCom(ConnectType.Non);

            WhereQuery cri_1_1 = new WhereCriteria(ConnectType.Non, "Name", CriType.Equal, "骇客");
            WhereQuery cri_1_2 = new WhereCriteria(ConnectType.And, "Point", CriType.Equal, 5);
            WhereQuery query_1_3 = new WhereQueryCom(ConnectType.And);

            WhereQuery cri_1_3_1 = new WhereCriteria(ConnectType.Non, "Name", CriType.Equal, "白客");
            WhereQuery cri_1_3_2 = new WhereCriteria(ConnectType.And, "Point", CriType.Equal, 8);

            query_1.Add(cri_1_1);
            query_1.Add(cri_1_2);
            query_1.Add(query_1_3);

            query_1_3.Add(cri_1_3_1);
            query_1_3.Add(cri_1_3_2);


            //Action
            WhereClause clause = query_1.GeneralClause();
            var videosByQuery = context.Videos.Where(clause.sqlStr, clause.paras.ToArray());
            List<Video> videosGetted = videosByQuery.ToList();


            //Assert
            Assert.AreEqual(0, videosGetted.Count);
        }

        #endregion





        #region 测试or

        [TestMethod]
        public void Test_GenerateClause_GetOrClause() { 
            //Arrange
            WhereQuery query = new WhereQueryCom(ConnectType.And);
            WhereQuery cri1 = new WhereCriteria(ConnectType.And, "Name", CriType.Equal, "骇客");
            WhereQuery cri2 = new WhereCriteria(ConnectType.Or, "Point", CriType.Equal, 5);

            query.Add(cri1);
            query.Add(cri2);

            //Action
            WhereClause clause = query.GeneralClause();

            //Assert
            Assert.AreEqual("Name = @0 OR Point = @1 ", clause.sqlStr);
            Assert.AreEqual(2, clause.paras.Count);
        }


        [TestMethod]
        public void Test_GenerateClause_GetInstances_By_OrClause()
        {
            //Arrange   //Add to db
            IEnumerable<Video> videosToAdd = new List<Video>()
            {
                new Video(){ 
                    Id = "12300",
                    Name = "骇客",
                    Point = 5
                },
                new Video(){
                    Id = "12301",
                    Name = "白客",
                    Point = 6
                },
                new Video(){
                    Id = "12302",
                    Name = "散客",
                    Point = 7
                },
            };
            EFDbContext contextToAdd = new EFDbContext();
            contextToAdd.Videos.AddRange(videosToAdd);
            contextToAdd.SaveChanges();

            //Arrange2      //Set query
            WhereQuery query = new WhereQueryCom(ConnectType.Or);
            WhereQuery cri1 = new WhereCriteria(ConnectType.Or, "Name", CriType.Equal, "骇客");
            WhereQuery cri2 = new WhereCriteria(ConnectType.Or, "Point", CriType.Equal, 6);

            query.Add(cri1);
            query.Add(cri2);


            //Action
            WhereClause clause = query.GeneralClause();
            EFDbContext contextToGet = new EFDbContext();
            var videosByQuery = contextToGet.Videos.Where(clause.sqlStr, clause.paras.ToArray());
            List<Video> videosGetted = videosByQuery.ToList();

            //Assert
            Assert.AreEqual("Name = @0 OR Point = @1 ", clause.sqlStr);
            Assert.AreEqual(2, videosGetted.Count);
            Assert.AreEqual("12300", videosGetted[0].Id);
            Assert.AreEqual("12301", videosGetted[1].Id);
        }


        #endregion




        #region 测试Like

        /// <summary>
        /// 测试生成Contain语句
        /// </summary>
        [TestMethod]
        public void Test_GenerateClause_Get_Cluase_By_Contain() {
            //Arrange
            WhereQuery query = new WhereQueryCom(ConnectType.And);
            WhereQuery cri1 = new WhereCriteria(ConnectType.And, "Name", CriType.Contains, "骇");

            query.Add(cri1);

            //Action
            WhereClause clause = query.GeneralClause();

            //Assert
            Assert.AreEqual("Name.Contains(@0) ", clause.sqlStr);
        }




        /// <summary>
        /// 测试通过Contain语句，获取实体
        /// </summary>
        [TestMethod]
        public void Test_GenerateClause_Get_Instance_By_Contain()
        {
            //Arrange   //Add to db
            IEnumerable<Video> videosToAdd = new List<Video>()
            {
                new Video(){ 
                    Id = "12300",
                    Name = "骇客",
                    Point = 5
                },
                new Video(){
                    Id = "12301",
                    Name = "白客",
                    Point = 6
                },
                new Video(){
                    Id = "12302",
                    Name = "散客",
                    Point = 7
                },
            };
            EFDbContext contextToAdd = new EFDbContext();
            contextToAdd.Videos.AddRange(videosToAdd);
            contextToAdd.SaveChanges();

            //Arrange2      //Set query
            WhereQuery query = new WhereQueryCom(ConnectType.Or);
            WhereQuery cri1 = new WhereCriteria(ConnectType.Or, "Name", CriType.Contains, "骇");

            query.Add(cri1);


            //Action
            WhereClause clause = query.GeneralClause();
            EFDbContext contextToGet = new EFDbContext();
            var videosByQuery = contextToGet.Videos.Where(clause.sqlStr, clause.paras.ToArray());
            List<Video> videosGetted = videosByQuery.ToList();

            //Assert
            Assert.AreEqual("Name.Contains(@0) ", clause.sqlStr);
            Assert.AreEqual(1, videosGetted.Count);
            Assert.AreEqual("12300", videosGetted[0].Id);
        }

        #endregion
    }
}
