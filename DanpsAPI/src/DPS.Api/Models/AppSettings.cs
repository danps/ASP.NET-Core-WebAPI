﻿namespace DPS.Api.Models
{
    public class AppSettings
    {
        public AppSettings()
        {
            
        }

        public string Secret { get; set; }
        public int ExpiracaoHoras { get; set; }
        public string Emissor { get; set; }
        public string ValidoEm { get; set; }
    }
}