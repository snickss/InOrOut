using Microsoft.Azure.Cosmos.Table;

namespace FunctionApp1
{
    public class PlayerEntity : TableEntity
    {
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}