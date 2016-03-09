using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TwitterDotNet.Services.AccountManager
{
    public interface IAccountManager
    {
        void CreateJsonData();
        Task SaveDataToFile();
    }
}
