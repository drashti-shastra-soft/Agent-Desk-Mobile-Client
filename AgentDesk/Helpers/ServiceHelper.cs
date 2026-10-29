using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDesk.Helpers;

public static class ServiceHelper
{
    public static T GetService<T>()
        where T : notnull
    {
        return Application.Current!
            .Handler!
            .MauiContext!
            .Services
            .GetRequiredService<T>();
    }
}
