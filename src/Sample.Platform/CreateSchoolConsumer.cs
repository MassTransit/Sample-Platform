using System;
using System.Threading.Tasks;
using MassTransit;
using Sample.Platform.Contracts;

namespace Sample.Platform
{
    public class CreateSchoolConsumer : IConsumer<ICreateSchool>
    {
        private ISchoolContext _schoolContext;

        public CreateSchoolConsumer(ISchoolContext schoolContext)
        {
            this._schoolContext = schoolContext;
        }

        public async Task Consume(ConsumeContext<ICreateSchool> context)
        {
            var school = _schoolContext.Create(new School()
            {
                SchoolName = context.Message.SchoolName
            });

            await context.RespondAsync<ISchool>(school);
        }
    }
}
