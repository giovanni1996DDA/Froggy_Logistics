using Services.Dao.Implementations.SQLServer;
using Services.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Logic
{
    public class DataBaseService
    {
        private static DataBaseService instance = new DataBaseService();

        /// <summary>
        /// Instancia única del servicio de acceso (patrón Singleton).
        /// </summary>
        public static DataBaseService Instance { get { return instance; } }

        /// <summary>
        /// Constructor privado para implementar el patrón Singleton.
        /// </summary>
        private DataBaseService()
        {
        }

        public void BackupDataBase()
        {
            try
            {
                DataBaseDao.BackupDatabase();
                
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<BackupFile> GetAllBackups()
        {
            try
            {
                return DataBaseDao.GetAllBackups();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RestoreBackup(string fileName)
        {
            try
            {
                DataBaseDao.RestoreBackup(fileName);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
