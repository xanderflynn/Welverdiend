using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Wel.Data
{
    [Table("TblCompleted")]
    public class TblCompleted
    {
        [PrimaryKey, AutoIncrement]
        [Column("Pk_Id")]
        public int Pk_Id { get; set; }
        [Column("Id")]
        public int Id { get; set; }
        [Column("ColumnName")]
        public string ColumnName { get; set; }
        [Column("ColumnValue")]
        public string ColumnValue { get; set; }
        [Column("Error")]
        public string Error { get; set; }
        [Column("TableName")]
        public string TableName { get; set; }
        [Column("UserId")]
        public int UserId { get; set; }
        [Column("Value")]
        public string Value { get; set; }
        [Column("Version")]
        public string Version { get; set; }
        [Column("Completed")]
        public int Completed { get; set; }
        [Column("recordId")]
        public int recordId { get; set; }
        [Column("uuid")]
        public string uuid { get; set; }
    }
}

