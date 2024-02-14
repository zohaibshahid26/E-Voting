
namespace Web_Assignment_1
{
    internal class Candidate
    {
        private int candidateID;
        private string name;
        private string party;
        private int votes;

        public int CandidateID
        {
            get => candidateID;
            set => candidateID = value;
        }
        public string Name
        {
            get => name;
            set => name = value;
        }
        public string Party
        {
            get => party;
            set => party = value;
        }
        public int Votes
        {
            get => votes;
            set => votes = value;
        }
        public int GenerateCandidateID()
        {
            Random rnd = new Random();
            return rnd.Next(1, 10000);
        }
        public Candidate()
        {
            this.name = "";
            this.party = "";
            this.votes = 0;
            this.candidateID = this.GenerateCandidateID();
        }

        public Candidate(string name, string party)
        {
            this.name = name;
            this.party = party;
            this.votes = 0;
            this.candidateID = this.GenerateCandidateID();
        }
        public void IncrementVotes()
        {
            votes++;
        }
        public override string ToString()
        {
            return CandidateID + "\t\t\t" + name + "\t" + party + "\t\t" + votes;
        }
    }
}