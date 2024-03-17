using Acr.UserDialogs;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Wel.Data;
using Wel.SharedLibrary;

namespace Wel.Services
{

    public static class SQLFunctions
    {
        // SQLiteAsyncConnection Database;

        public static async Task Init()
        {
            SQLiteAsyncConnection Database = new SQLiteAsyncConnection(Const.DatabasePath, Const.Flags);
            //if (Database. is not null)
            //    return;



            if (DBExists() == true)
            {
                return;
            }
            Database = new SQLiteAsyncConnection(Const.DatabasePath, Const.Flags);
            try
            {

                var result = await Database.CreateTableAsync<DynamicDataUploadTable>();
                var result1 = await Database.CreateTableAsync<TblCompleted>();
                var result2 = await Database.CreateTableAsync<TblIntermediate>();
                var result3 = await Database.CreateTableAsync<tblSignatures>();


            }
            catch (Exception ex)
            {

            }
        }

        public static bool DBExists()
        {
            string DocumentPath = Const.DatabasePath;
            var path = Path.Combine(DocumentPath, Const.DatabaseFilename);
            return File.Exists(path);
        }


        public static async Task<List<DynamicDataUploadTable>> GetItemsAsync()
        {
            await Init();
            SQLiteAsyncConnection Database = new SQLiteAsyncConnection(Const.DatabasePath, Const.Flags);
            return await Database.Table<DynamicDataUploadTable>().ToListAsync();
        }

        public static async Task<List<TblCompleted>> GetAllCompletedQuestions()
        {
            await Init();
            SQLiteAsyncConnection Database = new SQLiteAsyncConnection(Const.DatabasePath, Const.Flags);
            //return await Database.Table<DyanamicData>().Where(t => t.TableName).ToListAsync();

            // SQL queries are also possible
            return await Database.QueryAsync<TblCompleted>("SELECT * FROM [TblCompleted]");
        }



        public static async Task<List<DynamicDataUploadTable>> GetAllQuestions()
        {
            await Init();
            SQLiteAsyncConnection Database = new SQLiteAsyncConnection(Const.DatabasePath, Const.Flags);
            //return await Database.Table<DyanamicData>().Where(t => t.TableName).ToListAsync();

            // SQL queries are also possible
            return await Database.QueryAsync<DynamicDataUploadTable>("SELECT * FROM [DynamicData]");
        }

        public static async Task<List<DynamicDataUploadTable>> GetQuery(string query)
        {
            await Init();
            SQLiteAsyncConnection Database = new SQLiteAsyncConnection(Const.DatabasePath, Const.Flags);
            //return await Database.Table<DyanamicData>().Where(t => t.TableName).ToListAsync();
            try
            {
                // SQL queries are also possible
                return await Database.QueryAsync<DynamicDataUploadTable>(query);
            }
            catch (Exception ex)
            {

                return null;
            }
            


        }

        public static async Task<bool> Deletequery(string query)
        {
            await Init();
            SQLiteAsyncConnection Database = new SQLiteAsyncConnection(Const.DatabasePath, Const.Flags);
            //return await Database.Table<DyanamicData>().Where(t => t.TableName).ToListAsync();

            // SQL queries are also possible
            await Database.QueryAsync<DynamicDataUploadTable>(query);
            return true;

        }

        public static async Task<DynamicDataUploadTable> GetItemAsync(int id)
        {
            await Init();
            SQLiteAsyncConnection Database = new SQLiteAsyncConnection(Const.DatabasePath, Const.Flags);
            return await Database.Table<DynamicDataUploadTable>().Where(i => i.UserId == id).FirstOrDefaultAsync();
        }

        public async static Task<string> SaveItemAsync(List<DyanamicData> item)
        {
            await Init();
            SQLiteAsyncConnection Database = new SQLiteAsyncConnection(Const.DatabasePath, Const.Flags);

            List<DynamicDataUploadTable> existingData = await GetAllQuestions();
            int recordsupdated = 0;
            int recordsinserted = 0;
            bool delete = await Deletequery("delete from DynamicData");

            using (var progress = UserDialogs.Instance.Progress("Retrieving Checklists..."))
            {
                int i = 0;
                foreach (DyanamicData row in item)
                {
                    if(item.Count > 100)
                    {
                        if(i != 100)
                        {
                            i++;
                            progress.PercentComplete = i;
                        }
                        
                    }
                    else
                    {
                        int prog = i / item.Count;
                        prog = prog * 100;
                        progress.PercentComplete = prog;
                    }
                   
                    

                    if (existingData.Count > 0)
                    {


                        recordsupdated++;





                        DynamicDataUploadTable dt = new DynamicDataUploadTable();
                        dt.ColumnValue = row.ColumnValue;
                        dt.Value = row.Value;
                        dt.ColumnName = row.ColumnName;
                        dt.Error = row.Error;
                        dt.TableName = row.TableName;
                        dt.UserId = row.UserId;
                        dt.Version = row.Version;
                        dt.recordId = row.Id;

                        recordsinserted++;
                        await Database.InsertAsync(dt);



                    }
                    else
                    {
                        DynamicDataUploadTable dt = new DynamicDataUploadTable();
                        dt.ColumnValue = row.ColumnValue;
                        dt.Value = row.Value;
                        dt.ColumnName = row.ColumnName;
                        dt.Error = row.Error;
                        dt.TableName = row.TableName;
                        dt.UserId = row.UserId;
                        dt.Version = row.Version;
                        recordsupdated++;
                        await Database.InsertAsync(dt);
                    }

                }
            }
            return "deleted & Updated";

            


        }

        public async static Task<bool> AddToIntermediateList(DynamicDataUploadTable item)
        {
            await Init();
            SQLiteAsyncConnection Database = new SQLiteAsyncConnection(Const.DatabasePath, Const.Flags);


            string test = "SELECT * FROM [DynamicData] Where TableName = '" + item.TableName + "'";

            List<DynamicDataUploadTable> results = await Database.QueryAsync<DynamicDataUploadTable>(test);

            foreach (DynamicDataUploadTable row in results)
            {
                TblIntermediate dt = new TblIntermediate();
                dt.Id = row.Id;
                dt.ColumnName = row.ColumnName;
                dt.ColumnValue = row.ColumnValue;
                dt.Error = row.Error;
                dt.TableName = row.TableName;
                dt.UserId = row.UserId;
                dt.Value = row.Value;
                dt.Version = row.Version;
                dt.Completed = 1;//row.Completed;
                dt.recordId = row.recordId;
                await Database.InsertAsync(dt);
            }

                


            return true;
        }

        public async static Task<bool> DeletefromIntermediateList()
        {
            await Init();
            SQLiteAsyncConnection Database = new SQLiteAsyncConnection(Const.DatabasePath, Const.Flags);

            string test = "delete from TblIntermediate";

            List<DynamicDataUploadTable> results = await Database.QueryAsync<DynamicDataUploadTable>(test);

            return true;
        }

        public async static Task<bool> DeleteCompletedList()
        {
            await Init();
            SQLiteAsyncConnection Database = new SQLiteAsyncConnection(Const.DatabasePath, Const.Flags);

            string test = "delete from TblCompleted";

            List<DynamicDataUploadTable> results = await Database.QueryAsync<DynamicDataUploadTable>(test);

            return true;
        }


        public async static Task<int> AddToCompletedList()
        {

            await Init();
            SQLiteAsyncConnection Database = new SQLiteAsyncConnection(Const.DatabasePath, Const.Flags);


            string test = "SELECT * FROM [TblIntermediate]";

            List<TblIntermediate> results = await Database.QueryAsync<TblIntermediate>(test);

            //await Database.InsertAsync(results);
            Guid uuid = Guid.NewGuid();

            foreach (TblIntermediate row in results)
            {
                TblCompleted dt = new TblCompleted();
                dt.Id = row.Id;
                dt.ColumnName = row.ColumnName;
                dt.ColumnValue = row.ColumnValue;
                dt.Error = row.Error;
                dt.TableName = row.TableName;
                dt.UserId = row.UserId;

                if (row.ColumnName.Contains("Description") && row.Value != "0")
                {
                    dt.Value = "1";
                }
                else
                {
                    dt.Value = row.Value;
                }
                dt.Version = row.Version;
                dt.Completed = row.Completed;
                dt.recordId = row.recordId;
                dt.uuid = uuid.ToString();
                await Database.InsertAsync(dt);
            }
            //TblCompleted lastEntry = Database.Table<TblCompleted>().OrderByDescending(x => x.Id).FirstOrDefaultAsync();

            string updateuuid = "UPDATE tblSignatures SET Fk_Id = '" + uuid + "' WHERE Fk_Id IS NULL OR Fk_Id = ''";

           // string returnlatestPk = "SELECT * FROM [TblCompleted] ORDER BY Pk_Id DESC LIMIT 1";

            List<TblCompleted> returnlatestPkitem = await Database.QueryAsync<TblCompleted>(updateuuid);

            //int PK = 0;

            //foreach (TblCompleted row in returnlatestPkitem)
            //{

            //    PK = row.Pk_Id;
            //}



            return 1;
        }

        public async static Task<List<Signature>> GetSignatures(string uuid)
        {
            List<Signature> result = new List<Signature>();
            try
            {


                await Init();
                SQLiteAsyncConnection Database = new SQLiteAsyncConnection(Const.DatabasePath, Const.Flags);

                string test = "Select * from [tblSignatures] where Fk_Id = '" + uuid + "'";

                //string test = "SELECT * FROM [TblIntermediate]";

                List<tblSignatures> results = await Database.QueryAsync<tblSignatures>(test);

                
                foreach (tblSignatures row in results) 
                { 
                    Signature sig = new Signature();
                    sig.employeeId = row.employeeId;
                    sig.content = row.content;
                    sig.contentType = row.contentType;
                    sig.recordId = 0;

                    result.Add(sig);    
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return result;
            }
        }

        public async static Task<string> addSignatures(string employeeId, string content, string contentType)
        {
            try
            {


                await Init();
                SQLiteAsyncConnection Database = new SQLiteAsyncConnection(Const.DatabasePath, Const.Flags);

                string test = "INSERT INTO tblSignatures(employeeId, content, contentType) VALUES('" + employeeId + "', '" + content + "', '" + contentType + "')";

                //string test = "SELECT * FROM [TblIntermediate]";

                List<tblSignatures> results = await Database.QueryAsync<tblSignatures>(test);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async static Task<string> updateSignatures(int Pk)
        {

            await Init();
            SQLiteAsyncConnection Database = new SQLiteAsyncConnection(Const.DatabasePath, Const.Flags);


            //string test = "SELECT * FROM [tblSignatures] WHERE Fk_Id IS NULL OR Fk_Id = ''";

            string test = "UPDATE tblSignatures SET Fk_Id = '" + Pk + "' WHERE Fk_Id IS NULL OR Fk_Id = ''";


            List<tblSignatures> results = await Database.QueryAsync<tblSignatures>(test);

            return "";
        }




            public async static Task<string> updateQuestion(string updateValue = null, string value = null, string error = null, string columnname = null, string imagebase64 = null, string tablename = null, string contentType = null, string SignatureBase64 = null)
        {
            await Init();
            SQLiteAsyncConnection Database = new SQLiteAsyncConnection(Const.DatabasePath, Const.Flags);

            List<DynamicDataUploadTable> existingData = await GetAllQuestions();
            int recordsupdated = 0;
            int recordsinserted = 0;
            try
            {
                if (error != null && imagebase64 == null)
                {
                    await Database.QueryAsync<DynamicDataUploadTable>("update TblIntermediate set value = '" + value.ToString() + "', Completed = '1' where ColumnValue = '" + updateValue.ToString() + "'");
                    return "success";
                }

                else if (!string.IsNullOrEmpty(contentType) && !string.IsNullOrEmpty(error) && !string.IsNullOrEmpty(columnname) && !string.IsNullOrEmpty(tablename) && !string.IsNullOrEmpty(imagebase64))
                {
                    //await Database.QueryAsync<DynamicDataUploadTable>("update TblIntermediate set value = '" + value.ToString() + "', Completed = '1' where ColumnValue = '" + updateValue.ToString() + "'");
                    string filteredColumnName = columnname.Replace("Description", "ImageId");
                    string filteredColumnNameError = columnname.Replace("Description", "Error");

                    string queryDescription = "update TblIntermediate set value = '" + value.ToString() + "', Completed = '1' where ColumnValue = '" + updateValue.ToString() + "'";
                    string query = "update TblIntermediate set value = '" + imagebase64.ToString() + "', Completed = '1', ColumnValue = '" + contentType.ToString() + "' where  ColumnName = '" + filteredColumnName.ToString() + "' AND TableName = '" + tablename.ToString() + "'";
                    string queryError = "update TblIntermediate set value = '" + error.ToString() + "' where  ColumnName = '" + filteredColumnNameError.ToString() + "' AND TableName = '" + tablename.ToString() + "'";

                    try
                    {

                        await Database.QueryAsync<TblIntermediate>(queryDescription);
                        await Database.QueryAsync<DynamicDataUploadTable>(query);
                        await Database.QueryAsync<DynamicDataUploadTable>(queryError);
                        return "success";
                    }
                    catch (Exception ex)
                    {
                        return "success";
                    }
                }
                else if(!string.IsNullOrEmpty(SignatureBase64))
                {
                    string sigColumn = "SignatureImageId";
                    string querysignature = "update TblIntermediate set value = '" + SignatureBase64.ToString() + "', Completed = '1', ColumnValue = '" + contentType.ToString() + "' where  ColumnName = '" + sigColumn.ToString() + "' AND TableName = '" + tablename.ToString() + "'";
                    //string querysignature = "update TblIntermediate set value = '" + SignatureBase64.ToString() + "' where  ColumnName = '" + sigColumn.ToString() + "' AND TableName = '" + tablename.ToString() + "'";
                    await Database.QueryAsync<DynamicDataUploadTable>(querysignature);

                    string test = "SELECT SignatureImageId,value,Completed,ColumnValue  FROM [TblIntermediate] ";

                    List<DynamicDataUploadTable> results = await Database.QueryAsync<DynamicDataUploadTable>(test);
                    return "success";
                }

                else
                {
                   await Database.QueryAsync<DynamicDataUploadTable>("update TblIntermediate set value = '" + value.ToString() + "', Completed = '1' where ColumnValue = '" + updateValue.ToString() + "'");
                    return "success";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


        }

        public static async Task<List<DynamicDataUploadTable>> FindchecklistID(string Tablename)
        {
            await Init();
            SQLiteAsyncConnection Database = new SQLiteAsyncConnection(Const.DatabasePath, Const.Flags);

            string test = "SELECT * FROM [DynamicData] Where Id != 0";

            var result = await Database.QueryAsync<DynamicDataUploadTable>(test);
            return result;

        }

        public static async Task<bool> SetCompletedChecklist(string Tablename)
        {
            await Init();
            SQLiteAsyncConnection Database = new SQLiteAsyncConnection(Const.DatabasePath, Const.Flags);

            List<DynamicDataUploadTable> s = await Database.QueryAsync<DynamicDataUploadTable>("select * from DynamicData  where TableName = '" + Tablename + "'");

            await Database.QueryAsync<DynamicDataUploadTable>("update DynamicData set  Completed = '1' where TableName = '" + Tablename + "'");
            
            return true;


        }

        public static async Task<int> DeleteItemAsync(DynamicDataUploadTable item)
        {
            await Init();
            SQLiteAsyncConnection Database = new SQLiteAsyncConnection(Const.DatabasePath, Const.Flags);
            return await Database.DeleteAsync(item);
        }
    }
}
