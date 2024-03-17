using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wel.Data
{
    [Table("tblSignatures")]
    public class tblSignatures
    {
        [PrimaryKey, AutoIncrement]
        [Column("Pk_Id")]
        public int Pk_Id { get; set; }
        [Column("employeeId")]
        public int employeeId { get; set; }
        [Column("content")]
        public string content { get; set; }
        [Column("contentType")]
        public string contentType { get; set; }
        [Column("recordId")]
        public string recordId { get; set; }
        [Column("Fk_Id")]
        public int Fk_Id { get; set; }
    }
}

