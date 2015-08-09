using SQLite.Net.Attributes;

namespace Model
{
    public class DatabaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Content { get; set; }        
    }
}
