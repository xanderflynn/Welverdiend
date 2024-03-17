using System;
using System.Collections.Generic;
using System.Text;

namespace Wel.Data
{

    public class dynamicDataRoot
    {
        public int statusCode { get; set; }
        public bool success { get; set; }
        public string value { get; set; }
    }



    public class DyanamicData
    {
        public int Id { get; set; }
        public string ColumnName { get; set; }
        public string ColumnValue { get; set; }
        public string Error { get; set; }
        public string TableName { get; set; }
        public int UserId { get; set; }
        public string Value { get; set; }
        public string Version { get; set; }
    }

    public class Root
    {
        public List<DyanamicData> MyArray { get; set; }
    }

}
