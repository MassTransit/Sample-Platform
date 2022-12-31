using System;

namespace Sample.Platform.Contracts
{
    public interface ICreateSchool
    {
        string SchoolName { get; set; }
    }

    public class CreateSchool : ICreateSchool
    {
        public string SchoolName { get; set; }
    }
}
