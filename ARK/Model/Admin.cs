namespace ARK.Model
{
    public class Admin
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Contact { get; set; }

        //Navigation property
        public virtual Member Member { get; set; }
    }
}