using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;

namespace EngraveMemory.Engine
{
    [Table("Memorial")]
    public class Memorial
    {
        public Memorial()
        {
        }

        public Memorial(string header, string content, DateTime timestamp)
        {
            Header = header;
            Content = content;
            Timestamp = timestamp;
            LastRepeatDate = DateTime.Now;
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime LastRepeatDate { get; set; }
        
    }
}