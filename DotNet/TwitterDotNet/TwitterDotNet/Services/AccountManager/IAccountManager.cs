using System.Threading.Tasks;

namespace TwitterDotNet.Services.AccountManager
{
    public interface IAccountManager
    {
        void CreateJsonData();
        Task SaveDataToFile();
        Task LoadDataFromFile();
        void LoadAccountDataFromJson();
    }
}
