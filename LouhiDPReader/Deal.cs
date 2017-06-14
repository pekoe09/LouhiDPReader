using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouhiDPReader
{
    class Deal
    {
        public int DbId { get; set; }
        public String Name { get; set; }
        public DealPhase Phase { get; set; }
        public int? PredecessorId { get; set; }
        public int? SuccessorId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? DisposalTime { get; set; }

        public Deal (int dbId, String name, DealPhase phase, int? predecessorId, int? successorId, DateTime creationTime, DateTime? disposalTime)
        {
            this.DbId = dbId;
            this.Name = name;
            this.Phase = phase;
            this.PredecessorId = predecessorId;
            this.SuccessorId = successorId;
            this.CreationTime = creationTime;
            this.DisposalTime = disposalTime;
        }

        public static Deal HydrateDeal(String dealStr)
        {
            String[] dealParts = dealStr.Split(new char[] {';'});
            DealPhase p = (DealPhase)Enum.Parse(typeof(DealPhase), dealParts[2]);
            int? pred = dealParts[3].Length > 0 && !dealParts[3].ToUpper().Equals("NULL") ? (int?)Int32.Parse(dealParts[3]) : null;
            int? suc = dealParts[4].Length > 0 && !dealParts[4].ToUpper().Equals("NULL") ? (int?)Int32.Parse(dealParts[4]) : null;
            DateTime ct = DateTime.Parse(dealParts[5]);
            DateTime? dt = dealParts[6].Length > 0 && !dealParts[6].ToUpper().Equals("NULL") ? (DateTime?)DateTime.Parse(dealParts[6]) : null;

            Deal newDeal = new Deal(
                Int32.Parse(dealParts[0]),
                dealParts[1],
                p,
                pred,
                suc,
                ct,
                dt
                );

            return newDeal;
        }

    }

}
