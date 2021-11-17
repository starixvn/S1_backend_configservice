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
        public string? Main { get; set; }
        public string? Report { get; set; }
        public string? Wager { get; set; }
    }

}


