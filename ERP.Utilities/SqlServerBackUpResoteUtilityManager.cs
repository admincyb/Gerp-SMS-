using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.IO;
using BusinessObject;

namespace ERP.Utilities
{
    public sealed class SqlServerBackUpResoteUtilityManager
    {
        public static short BackUp(SqlConnectionStringBuilder sqlConnectionStringBuilder, string backupFolder, string backupFileName)
        {
            try
            {
                ////set something like  gERP_TEST_BIJU_150131.bak
                //backupFileName = String.Format("{0}_{1}.bak",
                //                            sqlConnectionStringBuilder.InitialCatalog,
                //                            DateTime.Now.ToString("yyMMdd"));

                //set something like C:\Temp\gERP_TEST_BIJU_150131.bak
                var backupFile = String.Format("{0}{1}", backupFolder, backupFileName);

                using (var connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
                {
                    var query = String.Format("BACKUP DATABASE {0} TO DISK='{1}'", sqlConnectionStringBuilder.InitialCatalog, backupFile);

                    using (var command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        int retVal= command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //throw new ApplicationException("Backup failed");
                return Convert.ToInt16(false);
            }
            return Convert.ToInt16(true);
        }

        public static short Restore(SqlConnectionStringBuilder sqlConnectionStringBuilder, string restoreDbName, string backupFileName)
        {
            short result;
            using (var connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                using (var command = new SqlCommand("SPADM_RESTORED_DB", connection))
                {
                    SqlParameter[] parameters = new SqlParameter[]{
                        new SqlParameter("@V_DB_NAME", restoreDbName),
                        new SqlParameter("@V_BACK_FILE",backupFileName)
                    };

                    command.Parameters.AddRange(parameters);
                    connection.Open();
                    result = (short)command.ExecuteNonQuery();
                }
            }

            return result;
        }

        public static short Restore(SqlConnectionStringBuilder sqlConnectionStringBuilder, string restoreDbName, BackupFileInfo backupFileInfo)
        {
            try
            {
                short result;
                using (var connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
                {
                    using (var command = new SqlCommand("SPADM_RESTORED_DB", connection))
                    {
                        SqlParameter[] parameters = new SqlParameter[]{
                        new SqlParameter("@V_DB_NAME",restoreDbName),
                        new SqlParameter("@V_BACK_FILE",backupFileInfo.BackupFileFullName)
                    };

                        command.Parameters.AddRange(parameters);
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandTimeout = 300;
                        connection.Open();
                        result = (short)command.ExecuteNonQuery();
                    }
                }

                return result;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public static List<BackupFileInfo> GetRestoreList(string backUpFolder)
        {
            List<BackupFileInfo> backupList = new List<BackupFileInfo>();

            try
            {
                foreach (var file in Directory.GetFiles(backUpFolder, "*.bak"))
                {
                    FileInfo finfo = new FileInfo(file);
                    backupList.Add(new BackupFileInfo
                    {
                        CreatedDate = finfo.CreationTime,
                        BackupFileName = finfo.Name,
                        BackupFileFullName = finfo.FullName,
                        BackupFileSize = FormatFileSize(finfo.Length),
                        FileSize = finfo.Length
                    });
                }
                return backupList.OrderByDescending(x => x.CreatedDate).ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Backup Device not found");
            }
        }

        private static string FormatFileSize(long fileSize)
        {
            if (fileSize < 1024)
                return string.Format("{0} bytes", fileSize);

            //bytes to GB
            double temp = fileSize / 1073741824;

            if(temp >= 1)
                return string.Format("{0:0.##} GB", temp);

            //bytes to MB
            temp = fileSize / 1048576;

            if (temp >= 1)
                return string.Format("{0:0.##} MB", temp);

            ////bytes to KB
            temp = fileSize / 1024;

            if (temp >= 1)
                return string.Format("{0:0.##} KB", temp);

            return string.Empty;
        }
    }

    public sealed class BackupFileInfo
    {
        public DateTime CreatedDate { get; set; }
        public string BackupFileFullName { get; set; }
        public string BackupFileName { get; set; }
        public string BackupFileSize { get; set; }
        public long FileSize { get; set; } // Only For Sorting
    }

    public struct GridParameters<T>
    {
        public string SortBy { get; set; }
        public string SortByOrder { get; set; }

        //public Func<T
    }
}
