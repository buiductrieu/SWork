﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWork.Data.DTO.AuthDTO
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Username { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
