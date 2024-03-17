using System;
using System.Collections.Generic;
using System.Text;

namespace Wel.Data
{
    //internal class AddQuestionsUpload
    //{
    //}

    public class addquestionsRoot
    {
        public string columnName { get; set; }
        public string columnValue { get; set; }
        public int recordId { get; set; }
        public string tableName { get; set; }
        public int userId { get; set; }
        public string uuid { get; set; }
        public List<Signature> signatures { get; set; }
        public string value { get; set; }
        public string version { get; set; }
    }

    public class Signature
    {
        public int employeeId { get; set; }
        public string content { get; set; }
        public string contentType { get; set; }
        public int recordId { get; set; }
    }

}
