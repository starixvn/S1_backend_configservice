namespace ConfigService
{
    public class AppConfig
    {
        public Connectionstrings ConnectionStrings { get; set; }

        public string? ProxyHost { get; set; }

        public int ProxyPort { get; set; }
        public string? RedisConnection { get; set; }
    }

    public class Connectionstrings
    {
        public string? ConnectionString { get; set; }
        public string? ConnectionStringReport { get; set; }
        public string? ConnectionStringWager { get; set; }
        public string? ConnectionStringLog { get; set; }
        public string? ConnectionStringIPTable { get; set; }

        public string? API_MainDB { get; set; }
        public string? API_ReportDB { get; set; }
        public string? API_WagerDB { get; set; }
        public string? API_LogDB { get; set; }
        public string? API_IPTable { get; set; }

        public string? Worker_MainDB { get; set; }
        public string? Worker_ReportDB { get; set; }
        public string? Worker_WagerDB { get; set; }
        public string? Worker_LogDB { get; set; }
        public string? Worker_IPTable { get; set; }

    }

}


