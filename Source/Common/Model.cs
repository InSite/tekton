﻿using System;

namespace Common
{
    public class Model
    {
        public Guid Identifier { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
    }
}