namespace WebAPI.Identity
{
    public class LoginResponse
    {
        public string Message { get; set; }
        public string Login { get; set; }
        public string JwtBearer { get; set; }
        public string RefreshJwtBearer { get; set; }
        public bool Success { get; set; }
    }
}
