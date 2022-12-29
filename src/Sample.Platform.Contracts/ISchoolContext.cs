using System;

namespace Sample.Platform.Contracts
{
    public interface ISchoolContext
    {
        ISchool Get(string schoolId);
        ISchool Create(ISchool school);
        ISchool AddAdmin(ICreateAdmin admin);
    }
}
