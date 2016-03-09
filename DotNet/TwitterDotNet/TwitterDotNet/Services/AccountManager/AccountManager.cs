using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.IO;
using Windows.Storage;

namespace TwitterDotNet.Services.AccountManager
{
    public class AccountManager : IAccountManager
    {
        private AccountData _accountData = new AccountData();
        public AccountData AccountData
        {
            get { return _accountData; }
            set { _accountData = value; }
        }

        private string _jsonDataToSave;
        public string JsonDataToSave
        {
            get { return _jsonDataToSave; }
            set { _jsonDataToSave = value; }
        }

        public void CreateJsonData()
        {
            JsonDataToSave = JsonConvert.SerializeObject(AccountData);
        }

        public async Task SaveDataToFile()
        {
            var fileName = ("accounts.json");
            var folderUsed = ApplicationData.Current.LocalFolder;
            var folderOp = Windows.Storage.CreationCollisionOption.ReplaceExisting;
            var fileUsed = await folderUsed.CreateFileAsync(fileName, folderOp);

            await Windows.Storage.FileIO.WriteTextAsync(fileUsed, JsonDataToSave);
        }

    }
}
