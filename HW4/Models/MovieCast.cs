namespace HW4.Models
{
    public class MovieCast
    {
        public List<Role> Cast { get; set; }
    }

    public class Role
    {
        public string Name { get; set; }
        public string Character { get; set; }
    }
}