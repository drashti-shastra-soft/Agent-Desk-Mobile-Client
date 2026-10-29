using AgentDesk.Models.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDesk.Models;

public partial class Policy : ObservableObject
{
    private bool _isExpanded = true;

    [JsonIgnore]
    public bool IsExpanded
    {
        get => _isExpanded;
        set => SetProperty(ref _isExpanded, value);
    }
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public int ClientId { get; set; }
    public int PolicyTypeId { get; set; }
    public string PolicyNumber { get; set; }
    public decimal? SumAssured { get; set; }
    public string IDV { get; set; }
    public DateTime? FUPDate { get; set; }
    public DateTime? CommercementDate { get; set; }
    public DateTime? MaturityDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? PremiumAmount { get; set; }
    public string PayMode { get; set; }
    public int PolicyStatusId { get; set; }
    public string MIME { get; set; }
    public string FilePath { get; set; }
    public string Client { get; set; }
    public string PolicyType { get; set; }
    public string PolicyStatus { get; set; }
    public bool IsActive { get; set; }
    public bool NeedToPremiumCollect { get; set; }
    public int CreatedBy { get; set; }
    public int? LastModifiedBy { get; set; }
    public string Name { get; set; }
    public string Base64String { get; set; }
    public string Status { get; set; }
    public string Address { get; set; }
    public int GroupId { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public DateTime? DOB { get; set; }
    public int AddressId { get; set; }
    public string Group { get; set; }
    public string NomineeName { get; set; }
    public int NomineeAge { get; set; }
    public string NomineeRelation { get; set; }
    //public string PlanTermPpt { get; set; }
    public string Plan { get; set; }
    public int Term { get; set; }
    public int PPT { get; set; }

    [JsonIgnore]
    public string PlanTermPpt => $"{Plan}/{Term}/{PPT}";

    [JsonIgnore]
    public bool IsSinglePay => string.Equals(PayMode, "Single", StringComparison.OrdinalIgnoreCase);
}


public class PolicyLister
{
    public List<Policy> List { get; set; } = new List<Policy>();
    public Policy SearchCriteria { get; set; } = new Policy();
    public Pagination Pagination { get; set; } = new Pagination();
}