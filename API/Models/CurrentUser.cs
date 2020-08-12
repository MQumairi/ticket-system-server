namespace API.Models
{
    public class CurrentUser
    {
        public string user_id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string first_name {get; set;}
        public string surname {get; set;}
        public string token { get; set; }

        public string avatar {get; set;}
    }
}