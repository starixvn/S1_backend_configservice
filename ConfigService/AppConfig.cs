namespace ConfigService
{
    public class AppConfig
    {
        public Connectionstrings ConnectionStrings { get; set; }

        public string? ProxyHost { get; set; }

        public int? ProxyPort { get; set; }
    }

    public class Connectionstrings
    {
        public string? MainDB { get; set; }
        public string? ReportDB { get; set; }
        public string? WagerDB { get; set; }
    }

}


