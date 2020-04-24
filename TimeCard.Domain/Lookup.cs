using System;
namespace TimeCard.Domain
{
    public class Lookup
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string Descr { get; set; }
        public string Val { get; set; }
        public bool Active { get; set; }
    }
}
