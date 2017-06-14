using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouhiDPReader
{
    class DealPhaseTransition
    {
        public int DealId { get; set; }
        public int CurrentDealId { get; set; }
        public string CurrentDealName { get; set; }
        public DateTime? Creationtime { get; set; }
        public DateTime? DateOut { get; set; }
        public DealPhase Phase { get; set; }

        public DealPhaseTransition(int dealId, int currentDealId, string currentDealName, 
            DateTime? creationTime, DateTime? dateOut, DealPhase phase)
        {
            this.DealId = dealId;
            this.CurrentDealId = currentDealId;
            this.CurrentDealName = currentDealName;
            this.Creationtime = creationTime;
            this.DateOut = dateOut;
            this.Phase = phase;
        }

        public override string ToString()
        {
            String phaseStr = Phase.ToString();
            if (Phase == DealPhase.ColdProspect)
                phaseStr = "Kylmä prospekti";
            else if (Phase == DealPhase.HotProspect)
                phaseStr = "Kuuma prospekti";
            StringBuilder sb = new StringBuilder(1000);
            sb.Append(CurrentDealId);
            sb.Append(";");
            sb.Append(CurrentDealName);
            sb.Append(";");
            sb.Append(phaseStr);
            sb.Append(";");
            sb.Append(Creationtime.HasValue ? ((DateTime)Creationtime).ToString("dd.MM.yyyy") : "");
            sb.Append(";");
            sb.Append(DateOut.HasValue ? ((DateTime)DateOut).ToString("dd.MM.yyyy") : "");
            return sb.ToString();
        }
    }
}
