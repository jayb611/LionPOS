using MySql.Data.MySqlClient;
using LionPOSDbContracts.DomainModels.Branch;
using LionPOSDbContracts.DomainModels.Maintenance;
using LionPOSDbContracts.DomainModels.User;
using LionPOSDbContracts.DomainModels.Configuration;
using LionPOSDbContracts.DomainModels.Product;
using LionPOSDbContracts.DomainModels.warehouse;
using LionPOSDbContracts.DomainModels.Account;
using LionPOSDbContracts.DomainModels.Supplier;
using LionPOSDbContracts.DomainModels.Employee;

namespace DomainModels
{
    public class UniversDbContext
    {

        public userlvitsposdbEntities userDbContext(bool LazyLoadingEnabled = true, bool ProxyCreationEnabled = true)
        {
            userlvitsposdbEntities db = new userlvitsposdbEntities();
            db.Configuration.LazyLoadingEnabled = LazyLoadingEnabled;
            db.Configuration.ProxyCreationEnabled = ProxyCreationEnabled;
            MySqlConnectionStringBuilder MyString = new MySqlConnectionStringBuilder();
            MyString.Server = "localhost";
            MyString.Port = 3306;
            MyString.UserID = "root";
            MyString.Password = "jayjay";
            MyString.Database = "userlvitsposdb";
            db.Database.Connection.ConnectionString = MyString.ConnectionString;
            db.SaveChanges();
            return db;
        }

        public accountinglvitsposdbEntities accountDbContext(bool LazyLoadingEnabled = true, bool ProxyCreationEnabled = true)
        {
            accountinglvitsposdbEntities db = new accountinglvitsposdbEntities();
            db.Configuration.LazyLoadingEnabled = LazyLoadingEnabled;
            db.Configuration.ProxyCreationEnabled = ProxyCreationEnabled;
            MySqlConnectionStringBuilder MyString = new MySqlConnectionStringBuilder();
            MyString.Server = "localhost";
            MyString.Port = 3306;
            MyString.UserID = "root";
            MyString.Password = "jayjay";
            MyString.Database = "accountinglvitsposdb";
            db.Database.Connection.ConnectionString = MyString.ConnectionString;
            db.SaveChanges();
            return db;
        }

        public warehouselvitsposdbEntities warehouseDbContext(bool LazyLoadingEnabled = true, bool ProxyCreationEnabled = true)
        {
            warehouselvitsposdbEntities db = new warehouselvitsposdbEntities();
            db.Configuration.LazyLoadingEnabled = LazyLoadingEnabled;
            db.Configuration.ProxyCreationEnabled = ProxyCreationEnabled;
            MySqlConnectionStringBuilder MyString = new MySqlConnectionStringBuilder();
            MyString.Server = "localhost";
            MyString.Port = 3306;
            MyString.UserID = "root";
            MyString.Password = "jayjay";
            MyString.Database = "warehouselvitsposdb";
            db.Database.Connection.ConnectionString = MyString.ConnectionString;
            db.SaveChanges();
            return db;
        }
        public productlvitsposdbEntities productDbContext(bool LazyLoadingEnabled = true, bool ProxyCreationEnabled = true)
        {
            productlvitsposdbEntities db = new productlvitsposdbEntities();
            db.Configuration.LazyLoadingEnabled = LazyLoadingEnabled;
            db.Configuration.ProxyCreationEnabled = ProxyCreationEnabled;
            MySqlConnectionStringBuilder MyString = new MySqlConnectionStringBuilder();
            MyString.Server = "localhost";
            MyString.Port = 3306;
            MyString.UserID = "root";
            MyString.Password = "jayjay";
            MyString.Database = "productlvitsposdb";
            db.Database.Connection.ConnectionString = MyString.ConnectionString;
            db.SaveChanges();
            return db;
        }

        public employeelvitsposdbEntities employeeDbContext(bool LazyLoadingEnabled = true, bool ProxyCreationEnabled = true)
        {
            employeelvitsposdbEntities db = new employeelvitsposdbEntities();
            db.Configuration.LazyLoadingEnabled = LazyLoadingEnabled;
            db.Configuration.ProxyCreationEnabled = ProxyCreationEnabled;
            MySqlConnectionStringBuilder MyString = new MySqlConnectionStringBuilder();
            MyString.Server = "localhost";
            MyString.Port = 3306;
            MyString.UserID = "root";
            MyString.Password = "jayjay";
            MyString.Database = "employeelvitsposdb";
            db.Database.Connection.ConnectionString = MyString.ConnectionString;
            db.SaveChanges();
            return db;
        }

        public branchlvitsposdbEntities branchDbContext(bool LazyLoadingEnabled = true, bool ProxyCreationEnabled = true)
        {
            branchlvitsposdbEntities db = new branchlvitsposdbEntities();
            db.Configuration.LazyLoadingEnabled = LazyLoadingEnabled;
            db.Configuration.ProxyCreationEnabled = ProxyCreationEnabled;
            MySqlConnectionStringBuilder MyString = new MySqlConnectionStringBuilder();
            MyString.Server = "localhost";
            MyString.Port = 3306;
            MyString.UserID = "root";
            MyString.Password = "jayjay";
            MyString.Database = "branchlvitsposdb";
            MyString.DefaultCommandTimeout = 20000;
            db.Database.Connection.ConnectionString = MyString.ConnectionString;
            db.SaveChanges();
            return db;
        }

        public maintenancelvitsposdbEntities maintenanceDbContext(bool LazyLoadingEnabled = true, bool ProxyCreationEnabled = true)
        {
            maintenancelvitsposdbEntities db = new maintenancelvitsposdbEntities();
            db.Configuration.LazyLoadingEnabled = LazyLoadingEnabled;
            db.Configuration.ProxyCreationEnabled = ProxyCreationEnabled;
            MySqlConnectionStringBuilder MyString = new MySqlConnectionStringBuilder();
            MyString.Server = "localhost";
            MyString.Port = 3306;
            MyString.UserID = "root";
            MyString.Password = "jayjay";
            MyString.Database = "maintenancelvitsposdb";
            MyString.DefaultCommandTimeout = 20000;
            db.Database.Connection.ConnectionString = MyString.ConnectionString;
            db.SaveChanges();
            return db;
        }

        public configurationlvitsposdbEntities configurationDbContext(bool LazyLoadingEnabled = true, bool ProxyCreationEnabled = true)
        {
            configurationlvitsposdbEntities db = new configurationlvitsposdbEntities();
            db.Configuration.LazyLoadingEnabled = LazyLoadingEnabled;
            db.Configuration.ProxyCreationEnabled = ProxyCreationEnabled;
            MySqlConnectionStringBuilder MyString = new MySqlConnectionStringBuilder();
            MyString.Server = "localhost";
            MyString.Port = 3306;
            MyString.UserID = "root";
            MyString.Password = "jayjay";
            MyString.Database = "configurationlvitsposdb";
            MyString.DefaultCommandTimeout = 20000;
            db.Database.Connection.ConnectionString = MyString.ConnectionString;
            db.SaveChanges();
            return db;
        }

        public supplierlvitsposdbEntities supplierDbContext(bool LazyLoadingEnabled = true, bool ProxyCreationEnabled = true)
        {
            supplierlvitsposdbEntities db = new supplierlvitsposdbEntities();
            db.Configuration.LazyLoadingEnabled = LazyLoadingEnabled;
            db.Configuration.ProxyCreationEnabled = ProxyCreationEnabled;
            MySqlConnectionStringBuilder MyString = new MySqlConnectionStringBuilder();
            MyString.Server = "localhost";
            MyString.Port = 3306;
            MyString.UserID = "root";
            MyString.Password = "jayjay";
            MyString.Database = "supplierlvitsposdb";
            db.Database.Connection.ConnectionString = MyString.ConnectionString;
            db.SaveChanges();
            return db;
        }
    }
}