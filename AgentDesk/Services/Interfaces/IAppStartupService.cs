using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDesk.Services.Interfaces;

public interface IAppStartupService
{
    Task InitializeAsync();
}
