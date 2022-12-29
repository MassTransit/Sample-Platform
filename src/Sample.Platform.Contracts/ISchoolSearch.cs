using System;

namespace Sample.Platform.Contracts
{
    /// <summary>
    /// Search a school, using the school id, for a given Admin person
    /// </summary>
    public interface ISchoolSearch
    {
        string SchoolId { get; set; }
        string SearchName { get; set; }
    }
}
