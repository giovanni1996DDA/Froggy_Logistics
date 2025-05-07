using Services.Dao.Implemenations.SQLServer.Helpers;
using Services.Dao.Interfaces;
using Services.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Services.Dao.Implementations.SQLServer
{
    public class DataBaseDao
    {
        private readonly static string connectionString = ConfigurationManager.ConnectionStrings["ServicesSqlConnection"].ToString();
        private static string BackupQuery
        {
            get
            {
                return "BACKUP DATABASE @DatabaseName TO DISK = @FullBackupPath WITH INIT;";
            }
        }
        public static void BackupDatabase()
        {
             using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string backupPath = ConfigurationManager.AppSettings["BackUpPath"];

                string ServicesFileName = $"ServicesDB-{DateTime.Now:yyyy-MM-dd - HH 'hs'}.bak";

                string fullServicesBackupPath = backupPath + ServicesFileName;

                string BusinessFileName = $"BusinessDB-{DateTime.Now:yyyy-MM-dd - HH 'hs'} .bak";

                string fullBusinessBackupPath = backupPath + BusinessFileName;

                SqlCommand command = new SqlCommand(BackupQuery, connection);

                // Agregar los parámetros a la colección Parameters
                command.Parameters.Add(new SqlParameter("@FullBackupPath", fullServicesBackupPath));
                command.Parameters.Add(new SqlParameter("@DatabaseName", ConfigurationManager.AppSettings["ServicesDBName"]));

                connection.Open();

                command.ExecuteNonQuery();

                command = new SqlCommand(BackupQuery, connection);

                command.Parameters.Add(new SqlParameter("@FullBackupPath", fullBusinessBackupPath));
                command.Parameters.Add(new SqlParameter("@DatabaseName", ConfigurationManager.AppSettings["BusinessDBName"]));

                command.ExecuteNonQuery();
            }
        }

        public static List<BackupFile> GetAllBackups()
        {
            // Lista para almacenar los archivos
            List<BackupFile> backupFiles = new List<BackupFile>();

            string backupPath = ConfigurationManager.AppSettings["BackUpPath"];

            try
            {
                // Obtener todos los archivos .bak en el directorio
                string[] files = Directory.GetFiles(backupPath, "*.bak");

                // Crear objetos BackupFile para cada archivo
                foreach (string file in files)
                {
                    backupFiles.Add(new BackupFile
                    {
                        FileName = Path.GetFileName(file), // Nombre del archivo
                        BackupPath = file                       // Ruta completa
                    });
                }

                return backupFiles;
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(backupPath);

                return GetAllBackups();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void RestoreBackup(string filename)
        {

            string dbName = filename.Split('-')[0];

            string fullBackupPath = ConfigurationManager.AppSettings["BackUpPath"] + filename;

            string RestoreQuery =$@"DECLARE @SQL NVARCHAR(MAX);
                                    SET @SQL = '
                                    USE master;
                                            ALTER DATABASE[{dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                                            RESTORE DATABASE[{dbName}] FROM DISK = ''{fullBackupPath}
                                            '' WITH REPLACE;
                                            ALTER DATABASE[{dbName}] SET MULTI_USER;
                                            ';
                                    EXEC sp_executesql @SQL; ";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand command = new SqlCommand(RestoreQuery, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }

                    Console.WriteLine($"Restauración de la base de datos {dbName} completada exitosamente.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al restaurar la base de datos: {ex.Message}");
                }
            }
        }
    }
}
