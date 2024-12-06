using System.Data.Common;
using System.Security.Cryptography;
using System.Text;

namespace ConfigService
{
    public class AppConfig
    {
        private static readonly DESCryptoServiceProvider CryptoServiceProvider = new DESCryptoServiceProvider();

        private static readonly byte[] RgbKey = new byte[8] { 33, 99, 84, 90, 145, 172, 110, 204 };

        private static readonly byte[] RgbIv = Encoding.UTF8.GetBytes("&％#@?,:*!~^.");
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
                if (config.API_User != null && name.StartsWith("API_", StringComparison.OrdinalIgnoreCase))
                {
                    var (user, password) = GetUserAndPassword(value);
                    var conn = value.Replace(user, config.API_User).Replace(password, DecryptPassword(config.API_Password));
                    prop.SetValue(ConnectionStrings, conn);
                }

                if (config.Worker_User != null && name.StartsWith("Worker_", StringComparison.OrdinalIgnoreCase))
                {
                    var (user, password) = GetUserAndPassword(value);
                    var conn = value.Replace(user, config.Worker_User).Replace(password, DecryptPassword(config.Worker_Password));
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
            ConnectionStrings.ConnectionString = DecryptConnectionString(ConnectionStrings.ConnectionString);
            ConnectionStrings.ConnectionStringReport = DecryptConnectionString(ConnectionStrings.ConnectionStringReport);
            ConnectionStrings.ConnectionStringWager = DecryptConnectionString(ConnectionStrings.ConnectionStringWager);
            ConnectionStrings.ConnectionStringLog = DecryptConnectionString(ConnectionStrings.ConnectionStringLog);
            ConnectionStrings.ConnectionStringIPTable = DecryptConnectionString(ConnectionStrings.ConnectionStringIPTable);
            ConnectionStrings.ConnectionStringReportIPTable = DecryptConnectionString(ConnectionStrings.ConnectionStringReportIPTable);
            ConnectionStrings.API_MainDB = DecryptConnectionString(ConnectionStrings.API_MainDB);
            ConnectionStrings.API_ReportDB = DecryptConnectionString(ConnectionStrings.API_ReportDB);
            ConnectionStrings.API_WagerDB = DecryptConnectionString(ConnectionStrings.API_WagerDB);
            ConnectionStrings.API_LogDB = DecryptConnectionString(ConnectionStrings.API_LogDB);
            ConnectionStrings.API_IPTable = DecryptConnectionString(ConnectionStrings.API_IPTable);
            ConnectionStrings.API_ReportIPTable = DecryptConnectionString(ConnectionStrings.API_ReportIPTable);
            ConnectionStrings.API_ReportBackupDB1 = DecryptConnectionString(ConnectionStrings.API_ReportBackupDB1);
            ConnectionStrings.API_ReportBackupDB2 = DecryptConnectionString(ConnectionStrings.API_ReportBackupDB2);
            ConnectionStrings.API_ReportCenter = DecryptConnectionString(ConnectionStrings.API_ReportCenter);
            ConnectionStrings.Worker_MainDB = DecryptConnectionString(ConnectionStrings.Worker_MainDB);
            ConnectionStrings.Worker_ReportDB = DecryptConnectionString(ConnectionStrings.Worker_ReportDB);
            ConnectionStrings.Worker_WagerDB = DecryptConnectionString(ConnectionStrings.Worker_WagerDB);
            ConnectionStrings.Worker_LogDB = DecryptConnectionString(ConnectionStrings.Worker_LogDB);
            ConnectionStrings.Worker_IPTable = DecryptConnectionString(ConnectionStrings.Worker_IPTable);
            ConnectionStrings.Worker_ReportIPTable = DecryptConnectionString(ConnectionStrings.Worker_ReportIPTable);
            ConnectionStrings.Worker_ReportBackupDB1 = DecryptConnectionString(ConnectionStrings.Worker_ReportBackupDB1);
            ConnectionStrings.Worker_ReportBackupDB2 = DecryptConnectionString(ConnectionStrings.Worker_ReportBackupDB2);
            ConnectionStrings.Worker_ReportCenter = DecryptConnectionString(ConnectionStrings.Worker_ReportCenter);
        }


        private static string DecryptConnectionString(string connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException("connectionString");
            }

            DbConnectionStringBuilder dbConnectionStringBuilder = new DbConnectionStringBuilder
            {
                ConnectionString = connectionString
            };
            string password = dbConnectionStringBuilder["password"].ToString();
            if (!string.IsNullOrEmpty(password))
            {
                string newPassword = DecryptPassword(password);
                dbConnectionStringBuilder["password"] = newPassword;
                return dbConnectionStringBuilder.ToString();
            }

            return connectionString;
        }

        private static string EncryptPassword(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            using ICryptoTransform transform = ((SymmetricAlgorithm)(object)CryptoServiceProvider).CreateEncryptor(RgbKey, RgbIv);
            using MemoryStream memoryStream = new MemoryStream();
            using CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();
            return Convert.ToBase64String(memoryStream.ToArray());
        }

        private static string DecryptPassword(string encryptedPassword)
        {
            if (encryptedPassword == null)
            {
                throw new ArgumentNullException("encryptedPassword");
            }

            string s = StandardizeEncryptedPasswordLength(encryptedPassword);
            byte[] buffer = Convert.FromBase64String(s);
            using MemoryStream stream = new MemoryStream(buffer);
            using ICryptoTransform transform = ((SymmetricAlgorithm)(object)CryptoServiceProvider).CreateDecryptor(RgbKey, RgbIv);
            using StreamReader streamReader = new StreamReader(new CryptoStream(stream, transform, CryptoStreamMode.Read));
            return streamReader.ReadToEnd().Trim();
        }
        private static string StandardizeEncryptedPasswordLength(string encryptedPassword)
        {
            string result = string.Empty;
            if (!string.IsNullOrWhiteSpace(encryptedPassword))
            {
                result = encryptedPassword[..(encryptedPassword.Length >> 2 << 2)];
            }

            return result;
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


