using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMrpSimulatorApp.Models
{
    public class ScheduleDto
    {
        public int SchIdx { get; set; }
        public string PlantCode { get; set; }
        public string PlantName { get; set; } // from settings.CodeName
        public DateOnly SchDate { get; set; }
        public int LoadTime { get; set; }
        public TimeOnly? SchStartTime { get; set; }
        public TimeOnly? SchEntTime { get; set; }
        public string SchFacilityId { get; set; }
        public string SchFacilityName { get; set; }
        public int SchAmount { get; set; }
        public DateTime? RegDt { get; set; }
        public DateTime? ModDt { get; set; }
    }

}
