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
        private string _jsonDataToSave;
        private string _jsonDataLoaded;
        private string _fileName;
        private StorageFolder _folderUsed;
        private CreationCollisionOption _folderOperation;
        private StorageFile _fileUsed;

        public AccountData AccountData { get { return _accountData; } set { _accountData = value; } }
        public string JsonDataToSave { get { return _jsonDataToSave; } set { _jsonDataToSave = value; } }
        public string JsonDataLoaded { get { return _jsonDataLoaded; } set { _jsonDataLoaded = value; } }
        public string FileName { get { return _fileName; } set { _fileName = value; } }
        public StorageFolder FolderUsed { get { return _folderUsed; } set { _folderUsed = value; } }
        public CreationCollisionOption FolderOperation { get { return _folderOperation; } set { _folderOperation = value; } }
        public StorageFile FileUsed { get { return _fileUsed; } set { _fileUsed = value; } }

        public AccountManager()
        {
            FileName = "accounts.json";
            FolderUsed = ApplicationData.Current.LocalFolder;
            FolderOperation = CreationCollisionOption.ReplaceExisting;
        }

        public void CreateJsonData()
        {
            JsonDataToSave = JsonConvert.SerializeObject(AccountData);
        }

        public async Task SaveDataToFile()
        {
            FolderOperation = CreationCollisionOption.ReplaceExisting;
            FileUsed = await FolderUsed.CreateFileAsync(FileName, FolderOperation);
            await FileIO.WriteTextAsync(FileUsed, JsonDataToSave);
        }

        public async Task LoadDataFromFile()
        {
            FolderOperation = CreationCollisionOption.OpenIfExists;
            FileUsed = await FolderUsed.CreateFileAsync(FileName, FolderOperation);

            JsonDataLoaded = await FileIO.ReadTextAsync(FileUsed);
        }

        public void LoadAccountDataFromJson()
        {
            AccountData = new AccountData();
            AccountData = JsonConvert.DeserializeObject<AccountData>(JsonDataLoaded);
        }
    }
}
