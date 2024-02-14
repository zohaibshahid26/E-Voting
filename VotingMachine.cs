using System.Data.SqlClient;


namespace Web_Assignment_1
{
    internal class VotingMachine
    {
        private List<Candidate> candidates;
        public VotingMachine()
        {
            candidates = new List<Candidate>();
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog = VotingSystem; Integrated Security = True";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string query = "select * from Candidate";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Candidate candidate = new Candidate();
                    candidate.CandidateID = Convert.ToInt32(dr[0]);
                    candidate.Name = dr[1].ToString();
                    candidate.Party = dr[2].ToString();
                    candidate.Votes = Convert.ToInt32(dr[3]);
                    candidates.Add(candidate);
                }
            }
            con.Close();
            dr.Close();

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
                if (candidates.Count == 0)
                {
                    Console.WriteLine("No candidates found! Please add candidates first!");
                    return;
                }

                string voterCnic = v.getCnic();
                v.Cnic = ValidateCnic(voterCnic);
                string connectionString = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog = VotingSystem; Integrated Security = True";
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                string query = $"select * from Voter where cnic = '{v.Cnic}'";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (!dr.HasRows)
                {
                    Console.WriteLine("Voter not found!.First Register your vote!");
                    this.addVoter();
                }
                dr.Close();
                cmd = new SqlCommand(query, con);

                dr = cmd.ExecuteReader();
                if (!dr.Read())
                {
                    Console.WriteLine("No voter found with CNIC " + voterCnic);
                    return;
                }
                Console.WriteLine("\nVoter Details ");
                Console.WriteLine("Voter Name: " + dr[1] + " \nVoter CNIC: " + dr[0]);

                if (dr[2].ToString() == "")
                {
                    dr.Close();
                here:
                    displayCandidates();
                    Console.Write("Enter Candidate ID: ");
                    int id = Int32.Parse(Program.IntegerValidation(Console.ReadLine()));
                    foreach (Candidate candidate in candidates)
                    {
                        if (candidate.CandidateID == id)
                        {
                            candidate.IncrementVotes();
                            query = "update Candidate set Votes = " + candidate.Votes + " where CandidateID=" + id;
                            cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                            v.castVote(candidate.Party);
                            query = $"update Voter set selectedPartyName = '{candidate.Party}' where cnic = '{v.Cnic}'";
                            cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                            Console.WriteLine("Vote casted successfully!");
                            return;
                        }
                    }

                    Console.WriteLine($"No candidate found with ID {id}");
                    goto here;

                }
                else
                {
                    Console.WriteLine("You have already casted your vote!\n");
                }
                con.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
        public void addVoter()
        {

            Console.WriteLine("Enter voter details: ");
            Console.Write("Name: ");
            string name = inputValidater(Console.ReadLine(), "Name");
            Console.Write("CNIC: ");
            string cnic = ValidateCnic(Console.ReadLine());
            Voter v = new Voter(name, cnic, "");
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog = VotingSystem; Integrated Security = True";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            try
            {
                string query = $"insert into Voter values('{cnic}','{name}','')";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Voter added successfully!\n");
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Violation of PRIMARY KEY constraint"))
                {
                    Console.WriteLine("Voter already exists\n");

                }
                else
                {
                    Console.WriteLine(e.Message);
                }

            }
            con.Close();
        }
        public void updateVoter(string cnic)
        {
            try
            {
                cnic = ValidateCnic(cnic);
                Console.WriteLine("Enter New Voter Details: ");
                Console.Write("Name: ");
                string name = inputValidater(Console.ReadLine(), "Name");
                string connectionString = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog = VotingSystem; Integrated Security = True";

                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                string query = $"update Voter set voterName = '{name}' where cnic = '{cnic}'";
                SqlCommand cmd = new SqlCommand(query, con);
                if (cmd.ExecuteNonQuery() == 0)
                {
                    Console.WriteLine($"No voter with CNIC {cnic} found!");
                }
                else
                {
                    Console.WriteLine("Voter updated successfully!");
                }
                con.Close();
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
                string connectionString = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog = VotingSystem; Integrated Security = True";
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                string query = "select * from Voter";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (!dr.HasRows)
                {
                    Console.WriteLine("No voters found");
                }
                else
                {
                    List<Voter> voters = new List<Voter>();
                    while (dr.Read())
                    {
                        voters.Add(new Voter(dr[1].ToString(), dr[0].ToString(), dr[2].ToString()));
                    }
                    int count = 0;
                    foreach (Voter v in voters)
                    {
                        Console.WriteLine(++count + ". " + v);
                    }
                }
                Console.WriteLine("\n");
                con.Close();
                dr.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
        public void deleteVoter(string cnic)
        {
            cnic = ValidateCnic(cnic);
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog = VotingSystem; Integrated Security = True";
            SqlConnection con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                string query = "delete from Voter where cnic = '" + cnic + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                if (cmd.ExecuteNonQuery() == 0)
                {
                    Console.WriteLine($"No voter with CNIC {cnic} found!");
                }
                else
                {
                    Console.WriteLine("Voter deleted successfully!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            con.Close();
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
            if (winners.Count == 0)
            {
                Console.WriteLine("No votes have been casted yet!");
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
                string connectionString = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog = VotingSystem; Integrated Security = True";
                SqlConnection con = new SqlConnection(connectionString);
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

                con.Open();
                string query = $"insert into Candidate values({c.CandidateID},'{c.Name}','{c.Party}',{c.Votes})";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Candidate added successfully!");
                con.Close();
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Violation of PRIMARY KEY constraint"))
                {
                    Console.WriteLine("Candidate already exists\n");
                }
                else
                {
                    Console.WriteLine(e.Message);
                }
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
                Console.WriteLine("\nNo candidates found!");
                return;
            }
            Console.WriteLine("\nList of Candidates: ");
            Console.WriteLine("CandidateID\t\tName\t\tParty\t\tVotes");
            foreach (Candidate c in candidates)
            {
                Console.WriteLine(c);
            }
            Console.WriteLine("\n");
        }
        public void updateCandidate(Candidate c, int id)
        {
            Console.WriteLine("Enter new candidate details: ");
            Console.Write("Name: ");
            string name = inputValidater(Console.ReadLine(), "Name");
            Console.Write("Party: ");
            string party = inputValidater(Console.ReadLine(), "Name");
            c.Name = name;
            c.Party = party;
            c.CandidateID = id;
            for (int i = 0; i < candidates.Count; i++)
            {
                if (candidates[i].CandidateID == id)
                {
                    candidates[i] = c;
                    break;
                }
            }

            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog = VotingSystem; Integrated Security = True";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string query = $"update Candidate set Name = '{c.Name}', Party = '{c.Party}' where CandidateID = {id}";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            if (cmd.ExecuteNonQuery() == 0)
            {
                Console.WriteLine("No candidate found");
            }
            else
            {
                Console.WriteLine("Candidate updated Succesfully!");
            }
            con.Close();
        }
        public void deleteCandidate(int id)
        {
            foreach (Candidate c in candidates)
            {
                if (c.CandidateID == id)
                {
                    candidates.Remove(c);
                    break;
                }
            }
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog = VotingSystem; Integrated Security = True";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string query = "delete from Candidate where CandidateID = " + id;
            SqlCommand cmd = new SqlCommand(query, con);
            if (cmd.ExecuteNonQuery() == 0)
            {
                Console.WriteLine("No candidate found");
            }
            else
            {
                Console.WriteLine("Candidate deleted Succesfully!");
            }
            con.Close();
        }
    }
}