﻿using System.ComponentModel.DataAnnotations;

namespace PersonTable.Data.Entities
{
    public class Email
    {
        public int Id { get; set; }
        public string Address { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
