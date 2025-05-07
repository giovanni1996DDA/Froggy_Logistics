using Services.Domain;
using Services.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Facade
{
    public static class DataBaseFacade
    {
        public static void BackupDatabase()
        {
            DataBaseService.Instance.BackupDataBase();
        }

        public static List<BackupFile> GetAllBackups()
        {
            return DataBaseService.Instance.GetAllBackups();
        }
        public static void RestoreBackup(string fileName)
        {
            DataBaseService.Instance.RestoreBackup(fileName);
        }
    }
}
