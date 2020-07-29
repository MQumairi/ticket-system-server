namespace API.Models
{
    public class CurrentUser
    {
        public int user_id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string token { get; set; }
    }
}