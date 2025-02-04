namespace TaskManagementStystem.Models
{
    public class LogModel
    {
        public int Id { get; set; } 
        public DateTime TimeStamp { get; set; }  
        public string Message { get; set; }
        public string MessageTemplate { get; set; }  
        public int TotalRecords { get; set; }  
    }

}
