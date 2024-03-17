using System;
using System.Collections.Generic;
using System.Text;

namespace Wel.Data
{
    public  class SendData
    {
        public static string UUID { get; set; }

        public static List<SendingData> datatosend { get; set; }
    }

    public class SendingData
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
}
