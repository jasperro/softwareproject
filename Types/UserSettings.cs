﻿using System;

namespace SoftwareProject.Types
{
    public class Usersettings
    {
        public Usersettings(int? simulatietijd)
        {
            SimTime = simulatietijd ?? (int)DateTimeOffset.UtcNow.Subtract(TimeSpan.FromDays(50)).ToUnixTimeSeconds();
        }

        public int SimTime { get; set; }
    }
}