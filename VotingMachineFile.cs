using System.Text.Json;


namespace Web_Assignment_1
{
    internal class VotingMachineFile
    {
        private List<Candidate> candidates;
        public VotingMachineFile()
        {
            candidates = new List<Candidate>();
            if (!System.IO.File.Exists("Candidates.txt"))
            {
                System.IO.File.Create("Candidates.txt").Close();
            }
            StreamReader sr = new StreamReader("Candidates.txt");
            string line = sr.ReadLine();
            Candidate c = new Candidate();
            while (line != null)
            {
                c = JsonSerializer.Deserialize<Candidate>(line);
                candidates.Add(c);
                line = sr.ReadLine();
            }
            sr.Close();
        }
        private string inputValidater(string input, string inputType)
        {
            while (input.Length == 0)
            {
                Console.Write($"Invalid input, please enter {inputType} again: ");
                input = Console.ReadLine();
            }
            for (int i = 0; i < input.Length; i++)
            {
                if (!char.IsLetter(input[i]) && input[i] != ' ')
                {
                    Console.Write($"Invalid input, please enter {inputType} again: ");
                    input = Console.ReadLine();
                    i = 0;
                }
            }
            return input;
        }
        private string ValidateCnic(string cnic)
        {

            try
            {
                while (cnic.Length != 15)
                {
                    Console.WriteLine("Valid CNIC format: XXXXX-XXXXXXX-X");
                    Console.Write("Invalid CNIC, please enter again: ");
                    cnic = Console.ReadLine();
                }
                while (cnic[5] != '-' || cnic[13] != '-')
                {
                    Console.WriteLine("Valid CNIC format: XXXXX-XXXXXXX-X");
                    Console.Write("Invalid CNIC, please enter again: ");
                    cnic = Console.ReadLine();
                }
                for (int i = 0; i < cnic.Length; i++)
                {
                    if (i == 5 || i == 13)
                        continue;
                    if (!char.IsDigit(cnic[i]))
                    {
                        Console.WriteLine("Valid CNIC format: XXXXX-XXXXXXX-X");
                        Console.Write("Invalid CNIC, please enter again: ");
                        cnic = Console.ReadLine();
                        i = 0;
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return cnic;
        }
        public void castVote(Voter v, Candidate c)
        {
            try
            {
                List<Voter> voters = new List<Voter>();
                if (!System.IO.File.Exists("Voters.txt"))
                {
                    System.IO.File.Create("Voters.txt").Close();
                }
                if (!System.IO.File.Exists("Candidates.txt"))
                {
                    System.IO.File.Create("Candidates.txt").Close();
                }
                if (candidates.Count == 0)
                {
                    Console.WriteLine("\nNo candidates found!\n");
                    return;
                }
                string voterCnic = v.Cnic;
                v.Cnic = ValidateCnic(v.Cnic);
                StreamReader sr = new StreamReader("Voters.txt");
                string line = sr.ReadLine();
                Voter voter = new Voter();
                while (line != null)
                {
                    voter = JsonSerializer.Deserialize<Voter>(line);
                    voters.Add(voter);
                    line = sr.ReadLine();
                }
                sr.Close();
                bool found = false;
                foreach (Voter voter1 in voters)
                {
                    if (voter1.Cnic == v.Cnic)
                    {
                        found = true;
                        v = voter1;
                        break;
                    }
                }
                if (!found)
                {
                    Console.WriteLine("\nNo voter found with this CNIC!\n");
                    Console.WriteLine("Please add voter first!\n");
                    this.addVoter();
                    StreamReader sr1 = new StreamReader("Voters.txt");
                    string line1 = sr1.ReadLine();
                    Voter voter1 = new Voter();
                    voters.Clear();
                    while (line1 != null)
                    {
                        voter1 = JsonSerializer.Deserialize<Voter>(line1);
                        voters.Add(voter1);
                        line1 = sr1.ReadLine();
                    }
                    sr1.Close();
                    foreach (Voter newVoter in voters)
                    {
                        if (newVoter.Cnic == v.Cnic)
                        {
                            v = newVoter;
                            break;
                        }
                    }
                }
                if (v.SelectedPartyName != "")
                {
                    Console.WriteLine("\nYou have already casted your vote!\n");
                    return;
                }
            label:
                this.displayCandidates();
                Console.Write("Enter CandidateID to cast vote: ");
                string id = Program.IntegerValidation(Console.ReadLine());
                int candidateID = int.Parse(id);
                bool candidateFound = false;
                foreach (Candidate candidate in candidates)
                {
                    if (candidate.CandidateID == candidateID)
                    {
                        candidateFound = true;
                        c = candidate;
                        break;
                    }
                }
                if (candidateFound)
                {
                    c.IncrementVotes();
                    File.WriteAllText("Voters.txt", "");
                    foreach (Voter voter1 in voters)
                    {
                        if (voter1.Cnic == voterCnic)
                        {
                            voter1.castVote(c.Party);
                        }
                        File.AppendAllText("Voters.txt", JsonSerializer.Serialize(voter1) + "\n");
                    }
                    File.WriteAllText("Candidates.txt", "");
                    foreach (Candidate candidate in candidates)
                    {
                        string json = JsonSerializer.Serialize(candidate);
                        File.AppendAllText("Candidates.txt", json + "\n");
                    }
                    Console.WriteLine("\nVote casted successfully!\n");
                }
                else
                {
                    Console.WriteLine("\nNo candidate found with this ID!\n");
                    goto label;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
        public void addVoter()
        {
            try
            {
                List<Voter> voters = new List<Voter>();
                if (!System.IO.File.Exists("Voters.txt"))
                {
                    System.IO.File.Create("Voters.txt").Close();
                }

                Console.Write("Enter Voter Name: ");
                string voterName = inputValidater(Console.ReadLine(), "Voter Name");
                Console.Write("Enter CNIC: ");
                string cnic = ValidateCnic(Console.ReadLine());
                StreamReader sr = new StreamReader("Voters.txt");
                string line = sr.ReadLine();
                Voter v = new Voter();
                while (line != null)
                {
                    v = JsonSerializer.Deserialize<Voter>(line);
                    if (v.Cnic == cnic)
                    {
                        Console.WriteLine("Voter already exists with this CNIC!\n");
                        sr.Close();
                        return;
                    }
                    line = sr.ReadLine();
                }
                sr.Close();
                Voter newVoter = new Voter { Cnic = cnic, VoterName = voterName, SelectedPartyName = "" };
                voters.Add(newVoter);
                string json = JsonSerializer.Serialize(newVoter);
                File.AppendAllText("Voters.txt", json + "\n");
                Console.WriteLine("\nVoter added successfully!\n");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


        }
        public void updateVoter(string cnic)
        {
            try
            {
                cnic = ValidateCnic(cnic);
                Console.Write("Enter new Voter Name: ");
                string voterName = inputValidater(Console.ReadLine(), "Voter Name");
                List<Voter> voters = new List<Voter>();
                if (!System.IO.File.Exists("Voters.txt"))
                {
                    System.IO.File.Create("Voters.txt").Close();
                }
                StreamReader sr = new StreamReader("Voters.txt");
                string line = sr.ReadLine();
                Voter v = new Voter();
                while (line != null)
                {
                    v = JsonSerializer.Deserialize<Voter>(line);
                    voters.Add(v);
                    line = sr.ReadLine();
                }
                sr.Close();
                bool found = false;
                foreach (Voter voter in voters)
                {
                    if (voter.Cnic == cnic)
                    {
                        voter.VoterName = voterName;
                        found = true;
                        break;
                    }
                }
                if (found)
                {
                    File.WriteAllText("Voters.txt", "");
                    foreach (Voter voter in voters)
                    {
                        string json = JsonSerializer.Serialize(voter);
                        File.AppendAllText("Voters.txt", json + "\n");
                    }
                    Console.WriteLine("\nVoter updated successfully!\n");
                }
                else
                {
                    Console.WriteLine("\nNo voter found with this CNIC!\n");
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


        }
        public void displayVoters()
        {
            try
            {
                List<Voter> voters = new List<Voter>();
                if (!System.IO.File.Exists("Voters.txt"))
                {
                    System.IO.File.Create("Voters.txt").Close();
                }
                StreamReader sr = new StreamReader("Voters.txt");
                string line = sr.ReadLine();
                Voter v = new Voter();
                while (line != null)
                {
                    v = JsonSerializer.Deserialize<Voter>(line);
                    voters.Add(v);
                    line = sr.ReadLine();
                }
                sr.Close();
                if (voters.Count == 0)
                {
                    Console.WriteLine("\nNo voters found!\n");
                    return;
                }

                int count = 0;
                foreach (Voter voter in voters)
                {
                    Console.WriteLine(++count + ". " + voter);
                }
                Console.WriteLine("\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


        }
        public void deleteVoter(string cnic)
        {
            try
            {
                cnic = ValidateCnic(cnic);
                List<Voter> voters = new List<Voter>();
                if (!System.IO.File.Exists("Voters.txt"))
                {
                    System.IO.File.Create("Voters.txt").Close();
                }
                StreamReader sr = new StreamReader("Voters.txt");
                string line = sr.ReadLine();
                Voter v = new Voter();
                while (line != null)
                {
                    v = JsonSerializer.Deserialize<Voter>(line);
                    voters.Add(v);
                    line = sr.ReadLine();
                }
                sr.Close();
                bool found = false;
                foreach (Voter voter in voters)
                {
                    if (voter.Cnic == cnic)
                    {
                        voters.Remove(voter);
                        found = true;
                        break;
                    }
                }
                if (found)
                {
                    File.WriteAllText("Voters.txt", "");
                    foreach (Voter voter in voters)
                    {
                        File.AppendAllText("Voters.txt", JsonSerializer.Serialize(voter) + "\n");
                    }
                    Console.WriteLine("\nVoter deleted successfully!\n");
                }
                else
                {
                    Console.WriteLine("\nNo voter found with this CNIC!\n");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


        }
        public void declareWinner()
        {
            int max = 0;
            List<Candidate> winners = new List<Candidate>();
            foreach (Candidate c in candidates)
            {
                if (c.Votes >= max)
                {
                    max = c.Votes;
                    winners.Add(c);
                }
            }
            if (max == 0 || winners.Count == 0)
            {
                Console.WriteLine("\nNo votes casted yet!\n");
            }
            else if (winners.Count == 1)
            {
                Console.WriteLine($"\nWinner: {winners[0].Name}  Votes: {winners[0].Votes}   Party: {winners[0].Party}");
            }
            else
            {
                Console.WriteLine("\nTie between: ");
                foreach (Candidate c in winners)
                {
                    Console.WriteLine(c.Name + " Votes: " + c.Votes + "  Party: " + c.Party);
                }
            }
        }
        public void insertCandidate()
        {
            try
            {
                if (!System.IO.File.Exists("Candidates.txt"))
                {
                    System.IO.File.Create("Candidates.txt").Close();
                }

                Console.WriteLine("Enter candidate details: ");
                Console.Write("Name: ");
                string name = inputValidater(Console.ReadLine(), "Name");
                Console.Write("Party: ");
                string party = inputValidater(Console.ReadLine(), "Name");
                Candidate c = new Candidate(name, party);
            label:
                foreach (Candidate candidate in candidates)
                {
                    if (candidate.CandidateID == c.CandidateID)
                    {
                        c.CandidateID = c.GenerateCandidateID();
                        goto label;
                    }

                }
                this.candidates.Add(c);
                File.AppendAllText("Candidates.txt", JsonSerializer.Serialize(c) + "\n");
                Console.WriteLine("\nCandidate added successfully!\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void readCandidate(int id)
        {
            foreach (Candidate c in candidates)
            {
                if (c.CandidateID == id)
                {
                    Console.WriteLine("\nCandidateID\t\tName\t\tParty\t\tVotes");
                    Console.WriteLine(c);
                    Console.WriteLine("\n");
                    return;
                }
            }
            Console.WriteLine("No candidate found with ID " + id + "\n");
        }
        public void displayCandidates()
        {
            if (candidates.Count == 0)
            {
                Console.WriteLine("\nNo candidates found!\n");
                return;
            }
            Console.WriteLine("\nList of Candidates: \n");
            Console.WriteLine("CandidateID\t\tName\t\tParty\t\tVotes");
            foreach (Candidate c in candidates)
            {
                Console.WriteLine(c);
            }
            Console.WriteLine("\n");

        }
        public void updateCandidate(Candidate c, int id)
        {
            try
            {
                if (candidates.Count == 0)
                {
                    Console.WriteLine("\nNo candidates found!\n");
                    return;
                }
                Console.Write("Enter new Name: ");
                string name = inputValidater(Console.ReadLine(), "Name");
                Console.Write("Enter new Party: ");
                string party = inputValidater(Console.ReadLine(), "Party");
                bool found = false;
                c.Name = name;
                c.Party = party;
                foreach (Candidate candidate in candidates)
                {
                    if (candidate.CandidateID == id)
                    {
                        candidate.Name = c.Name;
                        candidate.Party = c.Party;
                        found = true;
                        break;
                    }
                }
                if (found)
                {
                    File.WriteAllText("Candidates.txt", "");
                    foreach (Candidate candidate in candidates)
                    {
                        File.AppendAllText("Candidates.txt", JsonSerializer.Serialize(candidate) + "\n");
                    }
                    Console.WriteLine("\nCandidate updated successfully!\n");
                }
                else
                {
                    Console.WriteLine("\nNo candidate found with this ID!\n");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void deleteCandidate(int id)
        {
            try
            {
                if (candidates.Count == 0)
                {
                    Console.WriteLine("\nNo candidates found!\n");
                    return;
                }
                bool found = false;
                foreach (Candidate candidate in candidates)
                {
                    if (candidate.CandidateID == id)
                    {
                        candidates.Remove(candidate);
                        found = true;
                        break;
                    }
                }
                if (found)
                {
                    File.WriteAllText("Candidates.txt", "");
                    foreach (Candidate candidate in candidates)
                    {
                        File.AppendAllText("Candidates.txt", JsonSerializer.Serialize(candidate) + "\n");
                    }
                    Console.WriteLine("\nCandidate deleted successfully!\n");
                }
                else
                {
                    Console.WriteLine("\nNo candidate found with this ID!\n");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}