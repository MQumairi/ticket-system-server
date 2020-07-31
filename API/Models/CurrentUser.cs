namespace API.Models
{
    public class CurrentUser
    {
        public string user_id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string avatar { get; set; }
        public string token { get; set; }
    }
}