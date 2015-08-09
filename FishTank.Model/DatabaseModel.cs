using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DatabaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Content { get; set; }        
    }
}
