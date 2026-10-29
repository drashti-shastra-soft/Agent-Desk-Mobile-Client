using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDesk.Constants;

public static class ApiEndpoints
{
    // Authentication
    public const string Login = "api/Authentication";

    public const string ForgotPassword = "api/ForgotPassword";

    public const string ChangePassword = "api/ChangePassword";

    // Group
    public const string GetGroups = "api/Group/GetAll";

    public const string AddGroup = "api/Group";

    public const string GetGroup = "api/Group/{0}";

    public const string DeleteGroup = "api/Group/{0}/LoginUser/{1}";

    public const string GetActiveGroups = "api/Group/GetActiveGroup";

    // Agency
    public const string GetAgencies = "api/Agency/GetAll";

    public const string AddAgency = "api/Agency";

    public const string GetAgency = "api/Agency/{0}";

    public const string DeleteAgency = "api/Agency/{0}/LoginUser/{1}";

    public const string GetActiveAgencies = "api/Agency/GetActiveAgency";

    //Client
    public const string GetClients = "api/Client/GetAll";

    public const string AddClient = "api/Client";

    public const string GetClient = "api/Client/{0}";

    public const string DeleteClient = "api/Client/{0}/LoginUser/{1}";

    public const string AddClientIdentification = "api/ClientIdentification";

    public const string GetClientIdentification = "api/ClientIdentification/{0}";

    public const string DeleteClientIdentification = "api/ClientIdentification/{0}/LoginUser/{1}";

    public const string AddFinancialInfo = "api/FinancialInfo";

    public const string GetFinancialInfo = "api/FinancialInfo/{0}";

    public const string DeleteFinancialInfo = "api/FinancialInfo/{0}/LoginUser/{1}";

    public const string AddClientHealth = "api/ClientHealth";

    public const string GetClientHealth = "api/ClientHealth/{0}";

    public const string DeleteClientHealth = "api/ClientHealth/{0}/LoginUser/{1}";

    public const string AddClientDocument = "api/ClientDocument";

    public const string GetClientDocument = "api/ClientDocument/{0}";

    public const string DeleteClientDocument = "api/ClientDocument/{0}/LoginUser/{1}";


    //Policy
    public const string GetPolicies = "api/Policy/GetAll";

    public const string AddPolicy = "api/Policy";

    public const string GetPolicy = "api/Policy/{0}";

    public const string DeletePolicy = "api/Policy/{0}/LoginUser/{1}";

    public const string AddPolicyPremium = "api/PolicyPremium";

}
