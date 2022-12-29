using System;
using System.Collections.Generic;

namespace Sample.Platform.Contracts
{
    public interface ISchool
    {
        string SchoolId { get; set; }
        string SchoolName { get; set; }
        List<IAdmin> Admins { get; set; }
    }

    public class School : ISchool
    {
        public string SchoolId { get; set; }
        public string SchoolName { get; set; }
        public List<IAdmin> Admins { get; set; }
    }
}
