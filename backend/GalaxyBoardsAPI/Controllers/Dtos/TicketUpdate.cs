﻿using GalaxyBoardsAPI.Controllers.DTOs;
using GalaxyBoardsAPI.Data.Entities;

namespace GalaxyBoardsAPI.Controllers.Dtos
{
    public class TicketUpdate
    {
        public required string name { get; set; }
        public required string status { get; set; }
        public required string description { get; set; }
    }
}