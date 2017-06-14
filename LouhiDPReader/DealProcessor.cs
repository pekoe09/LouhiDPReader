using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LouhiDPReader
{
    class DealProcessor
    {
        private Dictionary<int, Deal> deals = new Dictionary<int, Deal>();
        private Dictionary<int, Dictionary<DealPhase, Deal>> dealVersions = new Dictionary<int, Dictionary<DealPhase, Deal>>();
        private List<DealPhaseTransition> transitions = new List<DealPhaseTransition>();
        private string path;

        internal static bool FilePathIsOk(string path)
        {
            return File.Exists(path);
        }

        internal void ProcessDeals(string path)
        {
            this.path = path;
            ReadDeals();
            CreatePhaseTransitions();
            CreatePhaseTransitionFile();
        }

        private void ReadDeals()
        {
            int counter = 0;
            string line;
            StreamReader file = new StreamReader(File.Open(path, FileMode.Open),Encoding.GetEncoding("iso-8859-1"));
            while ((line = file.ReadLine()) != null)
            {
                AddDeal(line);
                counter++;
            }
            file.Close();
            Console.WriteLine("Finished reading the Deal data; {0} deal records read.", counter);
        }

        private void AddDeal(string line)
        {
            Deal newDeal = Deal.HydrateDeal(line);
            deals.Add(newDeal.DbId, newDeal);
        }

        private void CreatePhaseTransitions()
        {
            foreach(KeyValuePair<int, Deal> kvp in deals)
            {
                if (!kvp.Value.SuccessorId.HasValue && !kvp.Value.DisposalTime.HasValue)
                {
                    GeneratePhaseTransition(kvp.Value, kvp.Value, null);
                    FillPhaseGaps(kvp.Value);
                }
            }
        }

        private void FillPhaseGaps(Deal deal)
        {
            for(int i = 1; i <= (int)DealPhase.Valmis; i++)
            {
                if (!dealVersions[deal.DbId].ContainsKey((DealPhase)i))
                {
                    DealPhaseTransition dpt = new DealPhaseTransition(
                        deal.DbId, deal.DbId, deal.Name, null, null, (DealPhase)i);
                    transitions.Add(dpt);
                    dealVersions[deal.DbId].Add((DealPhase)i, deal);
                }
            }
        }

        private void GeneratePhaseTransition(Deal deal, Deal currentDeal, DateTime? dateOut)
        {
            if (!deal.PredecessorId.HasValue)
            {
                StorePhaseTransition(deal, currentDeal, dateOut);
            }
            else
            {
                Deal nextDeal = deals[(int)deal.PredecessorId];
                if (deal.Phase != nextDeal.Phase)
                {
                    StorePhaseTransition(deal, currentDeal, dateOut);
                    dateOut = deal.CreationTime;
                }
                GeneratePhaseTransition(nextDeal, currentDeal, dateOut);
            }
        }

        private void StorePhaseTransition(Deal deal, Deal currentDeal, DateTime? dateOut)
        {
            if (!dealVersions.ContainsKey(currentDeal.DbId)
                    || !dealVersions[currentDeal.DbId].ContainsKey(deal.Phase))
            {
                DealPhaseTransition dpt = new DealPhaseTransition(
                    deal.DbId, currentDeal.DbId, currentDeal.Name, deal.CreationTime, dateOut, deal.Phase);
                transitions.Add(dpt);
                if (!dealVersions.ContainsKey(currentDeal.DbId))
                    dealVersions.Add(currentDeal.DbId, new Dictionary<DealPhase, Deal>());
                dealVersions[currentDeal.DbId].Add(deal.Phase, deal);
            }
        }

        private void CreatePhaseTransitionFile()
        {
            FileInfo sourceFile = new FileInfo(path);
            String newFilePath = String.Format("{0}\\PhaseTransitions{1}.csv", sourceFile.DirectoryName, DateTime.Now.ToString("yyyyMMddhhmmssfff"));
            
            StreamWriter file = new StreamWriter(File.Open(newFilePath, FileMode.OpenOrCreate), Encoding.GetEncoding("iso-8859-1"));
            int counter = 0;
            foreach (DealPhaseTransition dpt in transitions)
            {
                file.WriteLine(dpt.ToString());
                counter++;
            }
            file.Close();
            Console.WriteLine(String.Format("Finished writing to file {0}; wrote {1} transition records.", newFilePath, counter));
        }
    }
}
