﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task.Models.Models
{
    public class TaskDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Developer { get; set; }
        public string State { get; set; }
        public DateTime? DateCreate { get; set; }
    }
}
