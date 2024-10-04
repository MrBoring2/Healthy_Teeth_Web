namespace WebAPI.Identity
{
    public class JwtTokenResult
    {
        public string access_token { get; set; }
        public string user_name { get; set; }
        public string role_name { get; set; }
    }
}
