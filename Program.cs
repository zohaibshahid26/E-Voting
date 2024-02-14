using Web_Assignment_1;

class Program
{
    //These methods are used to perform different operations on the voting machine
    //These methods take object as parameter and then call the respective method of the object this object can be of VotingMachine or VotingMachineFile.

    static void AddVoter(Object vm)
    {
        Console.Clear();
        Console.WriteLine("-------------------------------------");
        Console.WriteLine("1. Add Voter");
        Console.WriteLine("-------------------------------------");
        vm.GetType().GetMethod("addVoter").Invoke(vm, null);

    }

    //This method is used to update voter
    static void UpdateVoter(Object vm)
    {
        Console.Clear();
        Console.WriteLine("-------------------------------------");
        Console.WriteLine("2. Update Voter");
        Console.WriteLine("-------------------------------------");
        Console.Write("Enter CNIC to Update: ");
        string cnic = Console.ReadLine();
        vm.GetType().GetMethod("updateVoter").Invoke(vm, new object[] { cnic });
    }

    //This method is used to display voters
    static void DisplayVoters(Object vm)
    {
        Console.Clear();
        Console.WriteLine("-------------------------------------");
        Console.WriteLine("3. Display Voters");
        Console.WriteLine("-------------------------------------");
        vm.GetType().GetMethod("displayVoters").Invoke(vm, null);
    }

    //This method is used to delete voter
    static void DeleteVoter(Object vm)
    {
        Console.Clear();
        Console.WriteLine("-------------------------------------");
        Console.WriteLine("4. Delete Voter");
        Console.WriteLine("-------------------------------------");
        Console.Write("Enter CNIC to Delete: ");
        string cnic = Console.ReadLine();
        vm.GetType().GetMethod("deleteVoter").Invoke(vm, new object[] { cnic });
    }

    //This method is used to display candidates
    static void DisplayCandidates(Object vm)
    {
        Console.Clear();
        Console.WriteLine("-------------------------------------");
        Console.WriteLine("5. Display Candidates");
        Console.WriteLine("-------------------------------------");
        vm.GetType().GetMethod("displayCandidates").Invoke(vm, null);

    }

    //This method is used to declare winner
    static void DeclareWinner(Object vm)
    {
        Console.Clear();
        Console.WriteLine("-------------------------------------");
        Console.WriteLine("6. Declare Winner");
        Console.WriteLine("-------------------------------------");
        vm.GetType().GetMethod("declareWinner").Invoke(vm, null);
    }

    //This method is used to insert candidate
    static void InsertCandidate(Object vm)
    {
        Console.Clear();
        Console.WriteLine("-------------------------------------");
        Console.WriteLine("7. Insert Candidate");
        Console.WriteLine("-------------------------------------");
        vm.GetType().GetMethod("insertCandidate").Invoke(vm, null);
    }

    //This method is used to read candidate
    static void ReadCandidate(Object vm)
    {
        Console.Clear();
        Console.WriteLine("-------------------------------------");
        Console.WriteLine("8. Read Candidate");
        Console.WriteLine("-------------------------------------");
        Console.Write("Enter ID of Candidate to Read: ");
        string id = IntegerValidation(Console.ReadLine());
        vm.GetType().GetMethod("readCandidate").Invoke(vm, new object[] { Int32.Parse(id) });
    }

    //This method is used to update candidate
    static void UpdateCandidate(Object vm)
    {
        Console.Clear();
        Console.WriteLine("-------------------------------------");
        Console.WriteLine("9. Update Candidate");
        Console.WriteLine("-------------------------------------");
        Console.Write("Enter ID of Candidate to Update: ");
        string id = IntegerValidation(Console.ReadLine());
        Candidate c = new Candidate();
        vm.GetType().GetMethod("updateCandidate").Invoke(vm, new object[] { c, Int32.Parse(id) });
    }

    //This method is used to delete candidate
    static void DeleteCandidate(Object vm)
    {
        Console.Clear();
        Console.WriteLine("-------------------------------------");
        Console.WriteLine("10. Delete Candidate");
        Console.WriteLine("-------------------------------------");
        Console.Write("Enter ID of Candidate to Delete: ");
        string id = IntegerValidation(Console.ReadLine());
        vm.GetType().GetMethod("deleteCandidate").Invoke(vm, new object[] { Int32.Parse(id) });
    }

    //This method is used to cast vote
    static void CastVote(Object vm)
    {
        Console.Clear();
        Console.WriteLine("-------------------------------------");
        Console.WriteLine("11. Cast Vote");
        Console.WriteLine("-------------------------------------");
        Console.Write("Enter CNIC to Vote: ");
        string cnic = Console.ReadLine();
        Voter voter = new Voter(cnic);
        Candidate candidate = new Candidate();
        vm.GetType().GetMethod("castVote").Invoke(vm, new object[] { voter, candidate });
    }

    //This method is used to validate the candidate id and choice entered by the end user
    static public string IntegerValidation(string id)
    {
        for (int i = 0; i < id.Length; i++)
        {
            if (id[i] < 48 || id[i] > 57)
            {
                Console.WriteLine("Invalid ID");
                Console.Write("Enter Valid ID: ");
                id = Console.ReadLine();
                i = 0;
            }
        }
        return id;
    }

    //Main method
    static void Main(string[] args)
    {
        //Creating object of VotingMachine or VotingMachineFile

        //VotingMachine vm = new VotingMachine();
        VotingMachineFile vm = new VotingMachineFile();

        //This is the menu that is displayed to the user
        while (true)
        {
            Console.WriteLine("-------------------------------Welcome to the Online Voting System---------------------------\n");
            Console.WriteLine("1. Add Voter");
            Console.WriteLine("2. Update Voter");
            Console.WriteLine("3. Display Voters");
            Console.WriteLine("4. Delete Voter");
            Console.WriteLine("5. Display Candidates");
            Console.WriteLine("6. Declare Winner");
            Console.WriteLine("7. Insert Candidate");
            Console.WriteLine("8. Read Candidate");
            Console.WriteLine("9. Update Candidate");
            Console.WriteLine("10. Delete Candidate");
            Console.WriteLine("11. Cast Vote\n");

            Console.Write("Enter your choice from 1-11: ");
            string id = IntegerValidation(Console.ReadLine());
            int choice = Int32.Parse(id);
            switch (choice)
            {
                case 1:
                    AddVoter(vm);
                    break;
                case 2:
                    UpdateVoter(vm);
                    break;
                case 3:
                    DisplayVoters(vm);
                    break;
                case 4:
                    DeleteVoter(vm);
                    break;
                case 5:
                    DisplayCandidates(vm);
                    break;
                case 6:
                    DeclareWinner(vm);
                    return;
                case 7:
                    InsertCandidate(vm);
                    break;
                case 8:
                    ReadCandidate(vm);
                    break;
                case 9:
                    UpdateCandidate(vm);
                    break;
                case 10:
                    DeleteCandidate(vm);
                    break;
                case 11:
                    CastVote(vm);
                    break;
            }

        }


    }

}