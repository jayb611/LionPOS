using MySql.Data.MySqlClient;
using ChikaraksDbContracts.DomainModels;

namespace DomainModels
{
    public class UniversDbContext
    {



        public chikaraksEntities chikaraksDbContext(bool LazyLoadingEnabled = true, bool ProxyCreationEnabled = true)
        {
            //182.50.133.91;user id=chikaraksuser;password=Chikaraks@12
            chikaraksEntities db = new chikaraksEntities();
            db.Configuration.LazyLoadingEnabled = LazyLoadingEnabled;
            db.Configuration.ProxyCreationEnabled = ProxyCreationEnabled;
            MySqlConnectionStringBuilder MyString = new MySqlConnectionStringBuilder();
            MyString.Server = "182.50.133.91";
            MyString.Port = 3306;
            MyString.UserID = "chikaraksuser";
            MyString.Password = "Chikaraks@12";
            MyString.Database = "chikaraks";
            MyString.DefaultCommandTimeout = 20000;
            db.Database.Connection.ConnectionString = MyString.ConnectionString;
            db.SaveChanges();
            return db;
        }






    }
}