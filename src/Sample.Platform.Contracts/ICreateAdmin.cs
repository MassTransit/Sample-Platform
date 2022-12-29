using System;

namespace Sample.Platform.Contracts
{
    public interface ICreateAdmin
    {
        string SchoolId { get; set; }
        string AdminName { get; set; }
    }

    public interface IAdmin
    {
        Guid AdminId { get; set; }
        string AdminName { get; set; }
    }

    public class Admin : IAdmin
    {
        public Guid AdminId { get; set; }
        public string AdminName { get; set; }
    }

    public class CreateAdmin : ICreateAdmin
    {
        public string SchoolId { get; set; }
        public string AdminName { get; set; }
    }
}
