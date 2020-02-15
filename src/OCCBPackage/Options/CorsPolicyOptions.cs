namespace OCCBPackage.Options
{
    public class CorsPolicyOptions
    {
        public string[] Origins { get; set; }

        public string[] Methods { get; set; }

        public string[] Headers { get; set; }

        public bool IsCredentials { get; set; }
    }
}
