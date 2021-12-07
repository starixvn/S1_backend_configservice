namespace ConfigService
{
    public class AppConfig
    {
        public Connectionstrings ConnectionStrings { get; set; }

        public string? ProxyHost { get; set; }

        public int ProxyPort { get; set; }
    }

    public class Connectionstrings
    {
        public string? API_MainDB { get; set; }
        public string? API_ReportDB { get; set; }
        public string? API_WagerDB { get; set; }

        public string? Worker_MainDB { get; set; }
        public string? Worker_ReportDB { get; set; }
        public string? Worker_WagerDB { get; set; }
    }

}


