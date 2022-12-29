using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sample.Platform.Contracts;

namespace Sample.Platform.db
{

    public class SchoolContext : ISchoolContext
    {
        private static List<ISchool> _schools = new List<ISchool>();

        public ISchool Get(string schoolId)
        {
            return _schools.SingleOrDefault(x => x.SchoolId == schoolId);
        }

        public ISchool Create(ISchool school)
        {
            school.SchoolId = Guid.NewGuid().ToString();
            school.Admins = new List<IAdmin>();
            _schools.Add(school);
            return school;
        }

        public ISchool AddAdmin(ICreateAdmin admin)
        {
            var school = Get(admin.SchoolId);
            school.Admins.Add( new Admin() { AdminName = admin.AdminName } );
            return school;
        }
    }
}
