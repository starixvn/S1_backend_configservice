using System.Data.Common;
using Fanex.Data;

namespace ConfigService
{
    public class AppConfig
    {
        public Connectionstrings ConnectionStrings { get; set; }

        public ServiceHosts ServiceHosts { get; set; }

        public string ProxyHost { get; set; }

        public int ProxyPort { get; set; }
        public string RedisConnection { get; set; }

        public void ChangeUserAndPassWord(ServiceConfig config)
        {
            foreach (var prop in ConnectionStrings.GetType().GetProperties())
            {
                var name = prop.Name;
                var value = prop.GetValue(ConnectionStrings).ToString();
                if (config.API_User != null && name.StartsWith("API_"))
                {
                    var (user, password) = GetUserAndPassword(value);
                    var conn = value.Replace(user, config.API_User).Replace(password, config.API_Password);
                    prop.SetValue(ConnectionStrings, conn);
                }

                if (config.Worker_User != null && name.StartsWith("Worker_"))
                {
                    var (user, password) = GetUserAndPassword(value);
                    var conn = value.Replace(user, config.Worker_User).Replace(password, config.Worker_Password);
                    prop.SetValue(ConnectionStrings, conn);
                }
            }
        }

        private (string User, string Password) GetUserAndPassword(string value)
        {
            var userStartIndex = value.IndexOf("user=") + 5;
            var userEndIndex = value.IndexOf(';', userStartIndex);
            var user = value.Substring(userStartIndex, userEndIndex - userStartIndex);
            var passwordStartIndex = value.IndexOf("password=") + 9;
            var passwordEndIndex = value.IndexOf(';', passwordStartIndex);
            var password = value.Substring(passwordStartIndex, passwordEndIndex - passwordStartIndex);
            return (user, password);
        }

        public void DecryptCustomizeAppConfig()
        {
            ConnectionStrings.ConnectionString = DecryptPassword(ConnectionStrings.ConnectionString);
            ConnectionStrings.ConnectionStringReport = DecryptPassword(ConnectionStrings.ConnectionStringReport);
            ConnectionStrings.ConnectionStringWager = DecryptPassword(ConnectionStrings.ConnectionStringWager);
            ConnectionStrings.ConnectionStringLog = DecryptPassword(ConnectionStrings.ConnectionStringLog);
            ConnectionStrings.ConnectionStringIPTable = DecryptPassword(ConnectionStrings.ConnectionStringIPTable);
            ConnectionStrings.ConnectionStringReportIPTable = DecryptPassword(ConnectionStrings.ConnectionStringReportIPTable);
            ConnectionStrings.API_MainDB = DecryptPassword(ConnectionStrings.API_MainDB);
            ConnectionStrings.API_ReportDB = DecryptPassword(ConnectionStrings.API_ReportDB);
            ConnectionStrings.API_WagerDB = DecryptPassword(ConnectionStrings.API_WagerDB);
            ConnectionStrings.API_LogDB = DecryptPassword(ConnectionStrings.API_LogDB);
            ConnectionStrings.API_IPTable = DecryptPassword(ConnectionStrings.API_IPTable);
            ConnectionStrings.API_ReportIPTable = DecryptPassword(ConnectionStrings.API_ReportIPTable);
            ConnectionStrings.API_ReportBackupDB1 = DecryptPassword(ConnectionStrings.API_ReportBackupDB1);
            ConnectionStrings.API_ReportBackupDB2 = DecryptPassword(ConnectionStrings.API_ReportBackupDB2);
            ConnectionStrings.API_ReportCenter = DecryptPassword(ConnectionStrings.API_ReportCenter);
            ConnectionStrings.Worker_MainDB = DecryptPassword(ConnectionStrings.Worker_MainDB);
            ConnectionStrings.Worker_ReportDB = DecryptPassword(ConnectionStrings.Worker_ReportDB);
            ConnectionStrings.Worker_WagerDB = DecryptPassword(ConnectionStrings.Worker_WagerDB);
            ConnectionStrings.Worker_LogDB = DecryptPassword(ConnectionStrings.Worker_LogDB);
            ConnectionStrings.Worker_IPTable = DecryptPassword(ConnectionStrings.Worker_IPTable);
            ConnectionStrings.Worker_ReportIPTable = DecryptPassword(ConnectionStrings.Worker_ReportIPTable);
            ConnectionStrings.Worker_ReportBackupDB1 = DecryptPassword(ConnectionStrings.Worker_ReportBackupDB1);
            ConnectionStrings.Worker_ReportBackupDB2 = DecryptPassword(ConnectionStrings.Worker_ReportBackupDB2);
            ConnectionStrings.Worker_ReportCenter = DecryptPassword(ConnectionStrings.Worker_ReportCenter);
        }

        private static string DecryptPassword(string customizeAppConfig)
        {
            var builder = new DbConnectionStringBuilder
            {
                ConnectionString = customizeAppConfig
            };

            builder["password"] = Cryptography.DecryptPassword(builder["password"].ToString());
            return builder.ConnectionString;
        }
    }

    public class Connectionstrings
    {
        #region 保留給舊的連線字串使用

        public string ConnectionString { get; set; }
        public string ConnectionStringReport { get; set; }
        public string ConnectionStringWager { get; set; }
        public string ConnectionStringLog { get; set; }
        public string ConnectionStringIPTable { get; set; }
        public string ConnectionStringReportIPTable { get; set; }

        #endregion

        #region ServiceType:API

        public string API_MainDB { get; set; }
        public string API_ReportDB { get; set; }
        public string API_WagerDB { get; set; }
        public string API_LogDB { get; set; }
        public string API_IPTable { get; set; }
        public string API_ReportIPTable { get; set; }
        public string API_ReportBackupDB1 { get; set; }
        public string API_ReportBackupDB2 { get; set; }
        public string API_ReportCenter { get; set; }

        #endregion

        #region ServiceType:Worker

        public string Worker_MainDB { get; set; }
        public string Worker_ReportDB { get; set; }
        public string Worker_WagerDB { get; set; }
        public string Worker_LogDB { get; set; }
        public string Worker_IPTable { get; set; }
        public string Worker_ReportIPTable { get; set; }
        public string Worker_ReportBackupDB1 { get; set; }
        public string Worker_ReportBackupDB2 { get; set; }
        public string Worker_ReportCenter { get; set; }

        #endregion
    }

    public class ServiceHosts
    {
        public string CommonService { get; set; }
        public string TpService { get; set; }
        public string PayService { get; set; }
        public string BonusService { get; set; }
        public string StoreService { get; set; }
        public string BackendService { get; set; }
        public string SeamlessService { get; set; }
        public string ReportService { get; set; }
    }

}


