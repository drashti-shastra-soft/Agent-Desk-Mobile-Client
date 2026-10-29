using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDesk.Models.Auth;

public class User
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public object Company { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public string MobileNumber { get; set; }
    public string Password { get; set; }
    public object PlainTextPassword { get; set; }
    public object Status { get; set; }
    public string Token { get; set; }
    public int RoleId { get; set; }
    public bool IsActive { get; set; }
    public int CreatedBy { get; set; }
    public object LastModifiedBy { get; set; }
    public string FullName => $"{FirstName} {LastName}";

}
