namespace API.Models.DTO
{
    public class UserDto
    {
        public string id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string surname { get; set; }
        public AvatarDto avatar { get; set; }

    }
}