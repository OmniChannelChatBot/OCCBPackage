namespace OCCBPackage.Options
{
    public class CorsPolicyOptions
    {
        public const string CorsPolicy = nameof(CorsPolicy);

        public string[] Origins { get; set; }

        public string[] Methods { get; set; }

        public string[] Headers { get; set; }

        public bool IsCredentials { get; set; }
    }
}
