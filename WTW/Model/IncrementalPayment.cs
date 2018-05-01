﻿namespace WTW.Model
{
    public class IncrementalPayment
    {
        public string Product { get; set; }
        public int OriginYear { get; set; }
        public int DevelopmentYear { get; set; }
        public decimal IncrementalValue { get; set; }
    }
}
