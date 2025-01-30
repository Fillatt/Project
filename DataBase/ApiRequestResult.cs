using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public class ApiRequestResult
    {
        public int Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int JokeId { get; set; }

        public required string Type { get; set; }

        public required string Setup { get; set; }

        public required string Punchline { get; set; }
    }
}
