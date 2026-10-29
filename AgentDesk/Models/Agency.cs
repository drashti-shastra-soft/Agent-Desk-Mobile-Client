using AgentDesk.Models.Common;

namespace AgentDesk.Models;

public class Agency
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string AgencyType { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string MobileNumber { get; set; }
    public string EmailAddress { get; set; }
    public string CompanyName { get; set; }
    public bool IsActive { get; set; }
    public string Status { get; set; }
    public int CreatedBy { get; set; }
    public int? LastModifiedBy { get; set; }
    public int? DeletedBy { get; set; }
}

public class AgencyLister
{
    public List<Agency> List { get; set; } = new List<Agency>();

    public Agency SearchCriteria { get; set; } = new Agency();

    public Pagination Pagination { get; set; } = new Pagination();
}
