
namespace Web_Assignment_1
{
    internal class Voter
    {
        private string voterName;
        private string cnic;
        private string selectedPartyName;

        public Voter()
        {
            voterName = "";
            cnic = "";
            selectedPartyName = "";
        }
        public Voter(string cnic)
        {
            voterName = "";
            this.cnic = cnic;
            selectedPartyName = "";
        }

        public Voter(string voterName, string cnic, string selectedPartyName)
        {
            this.voterName = voterName;
            this.cnic = cnic;
            this.selectedPartyName = selectedPartyName;
        }
        public string Cnic
        {
            get => cnic;
            set => cnic = value;
        }
        public string VoterName
        {
            get => voterName;
            set => voterName = value;
        }

        public string SelectedPartyName
        {
            get => selectedPartyName;
            set => selectedPartyName = value;
        }

        public string getCnic()
        {
            return cnic;
        }

        public void setCnic(string cnic)
        {
            this.cnic = cnic;
        }

        public void castVote(string partyname)
        {
            selectedPartyName = partyname;
        }

        public bool hasVoted()
        {
            if (selectedPartyName != "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override string ToString()
        {
            return voterName + " - CNIC: " + cnic;
        }
    }
}