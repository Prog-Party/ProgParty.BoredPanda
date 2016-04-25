using ProgParty.BoredPanda.ApiUniversal.Result;
using ProgParty.BoredPanda.ApiUniversal.Scrape;
using System.Collections.Generic;

namespace ProgParty.BoredPanda.ApiUniversal.Execute
{
    public class OverviewExecute
    {
        public Parameter.OverviewParameter Parameters = new Parameter.OverviewParameter();

        public List<OverviewResult> Result;

        public bool Execute()
        {
            try
            {
                Result = new OverviewScrape(Parameters).Execute();
                return true;
            }
            catch(System.Exception e)
            {
                return false;
            }
        }
    }
}
