using SqlSugar;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = "Server=localhost;Database=testsugar;Uid=root;Pwd=123456;CharSet=UTF8",
                DbType = DbType.MySql,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true
            });
            db.DbMaintenance.CreateDatabase();
            db.CodeFirst.InitTables(typeof(Test));
            if (!db.Queryable<Test>().Any())
            {
                db.Insertable(new Test() { Name = "sss" }).ExecuteCommand();
            }

            #region 这个是好的
            var ss = db.Queryable<Test>().Where(x => x.Id == 1).GroupBy(x => x.Name).Select(x => x.Name).ToPageListAsync(1, 10);
            ss.Wait();
            var ssRes = ss.Result;
            #endregion
            //=======================================================
            #region SqlSugarException: Queryable<String> Error ,String is invalid , need is a class,and can new().
            RefAsync<int> total = 0;
            var task = db.Queryable<Test>().Where(x => x.Id == 1).GroupBy(x => x.Name).Select(x => x.Name).ToPageListAsync(1, 10, total);
            task.Wait();
            var res = task.Result; 
            #endregion
        }
    }


    public class Test
    {
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        [SugarColumn(Length = 21)]
        public string Name { get; set; }
    }
}
