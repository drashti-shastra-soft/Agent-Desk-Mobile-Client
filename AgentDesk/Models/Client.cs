using AgentDesk.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDesk.Models;

public class Client
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public int GroupId { get; set; }
    public string Group { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime? DOB { get; set; }
    public string Gender { get; set; }
    public string MaritalStatus { get; set; }
    public string Occupation { get; set; }
    public bool IsActive { get; set; }
    public int? AddressId { get; set; }
    public int CreatedBy { get; set; }
    public int? LastModifiedBy { get; set; }
    public string FullName => $"{FirstName} {LastName}";


}

public class ClientLister
{
    public List<Client> List { get; set; } = new List<Client>();

    public Client SearchCriteria { get; set; } = new Client();

    public Pagination Pagination { get; set; } = new Pagination();
}