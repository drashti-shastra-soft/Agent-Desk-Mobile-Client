using AgentDesk.Models;
using AgentDesk.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDesk.Services.Interfaces;

public interface IPolicyService
{
    Task<Response> GetAllAsync(PolicyLister policyLister);
    Task<Response> AddAsync(Policy policy);
    Task<Response> GetByIdAsync(int id);
    Task<Response> DeleteAsync(int id, int loginUserId);
    Task<Response> AddPremiumAsync(PolicyPremium policyPremium);
}
