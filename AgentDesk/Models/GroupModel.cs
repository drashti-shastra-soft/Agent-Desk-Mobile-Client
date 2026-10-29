using AgentDesk.Models.Common;

namespace AgentDesk.Models;

public class GroupModel
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string HeadName { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int CreatedBy { get; set; }
    public int? LastModifiedBy { get; set; }
    public string DisplayName => $"{Code} - {Name}";
}
public class GroupLister
{
    public List<GroupModel> List { get; set; } = new();

    public GroupModel SearchCriteria { get; set; } = new();

    public Pagination Pagination { get; set; } = new();
}