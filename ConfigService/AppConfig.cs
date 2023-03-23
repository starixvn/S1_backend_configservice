namespace ConfigService
{
    public class AppConfig
    {
        public Connectionstrings ConnectionStrings { get; set; }

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
    }

    public class Connectionstrings
    {
        public string ConnectionString { get; set; }
        public string ConnectionStringReport { get; set; }
        public string ConnectionStringWager { get; set; }
        public string ConnectionStringLog { get; set; }
        public string ConnectionStringIPTable { get; set; }
        public string ConnectionStringReportIPTable { get; set; }

        public string API_MainDB { get; set; }
        public string API_ReportDB { get; set; }
        public string API_WagerDB { get; set; }
        public string API_LogDB { get; set; }
        public string API_IPTable { get; set; }
        public string API_ReportIPTable { get; set; }

        public string Worker_MainDB { get; set; }
        public string Worker_ReportDB { get; set; }
        public string Worker_WagerDB { get; set; }
        public string Worker_LogDB { get; set; }
        public string Worker_IPTable { get; set; }
        public string Worker_ReportIPTable { get; set; }
    }

}


