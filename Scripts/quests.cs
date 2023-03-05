using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Quest_System_TEST
{
    public class Quest
    {
        public int questID = 1;
        public string name = "default";
        public string objective = "default";
        public List<int> nextQuests;
        public Quest(int ID, string nameInput, string objectiveInput) //values will be assigned from the addQuest subroutine
        {
            questID = ID;
            name = nameInput;
            objective = objectiveInput;
            nextQuests = new List<int>(); //like a tree
        }
        public void addNextQuest(int ID)
        {
            nextQuests.Add(ID); //will be called with addQuest()
        }
        public void removeQuest(int num) // can be used as checklist
        {
            nextQuests.Remove(num);
        }
        public void listCurrentQuest()
        {
            Console.WriteLine("Current quest: {0}, {1}", name, objective);
        }
    }

    class Program
    {
        static Dictionary<int, Quest> getQuestsFromFile()
        {
            Dictionary<int, Quest> quests = new Dictionary<int, Quest>();
            const string filename = "quests.txt";
            string[] lines = File.ReadAllLines(filename);
            foreach (string i in lines)
            {
                string[] data = i.Split('|');
                List<int> connections = new List<int>(); //for list of questID connections (data[3])
                if (data[3].Contains(','))
                {
                    connections = Array.ConvertAll(data[3].Split(','), int.Parse).ToList();
                }
                else
                {
                    connections.Add(int.Parse(data[3]));
                }
                addQuest(ref quests, int.Parse(data[0]), data[1], data[2], connections); //one connection would be LINEAR, multiple would be choices/checklists
            }


            return quests;
        }

        static void addQuest(ref Dictionary<int, Quest> questLookup, int ID, string name, string objective, List<int> connections)
        {
            questLookup.Add(ID, new Quest(1, name, objective)); //create the class and place inside the dictionary
            foreach (int i in connections)
            {
                questLookup[ID].addNextQuest(i); //add the connections
            }
        }

        static List<int> completeQuest(ref Dictionary<int, Quest> questLookup, int questID)
        {
            //for now this is just getting next quest(s)
            List<int> nextQuests = questLookup[questID].nextQuests;
            Console.WriteLine("*!*DEBUG: Quest Completed!");
            return nextQuests;
        }

        static void Main(string[] args) //TEST CONDITION
        {
            Dictionary<int, Quest> questLookup = getQuestsFromFile();

            //Dictionary<int, Quest> questLookup = new Dictionary<int, Quest>(); // COULD POTENTIALLY READ THIS FROM A FILE
            //addQuest(ref questLookup, 1, "TestQuest", "Complete The Test", new List<int>(new int[] { 2 })); //one connection would be LINEAR, multiple would be choices/checklists
            //addQuest(ref questLookup, 2, "NextQuest", "Test successful!", new List<int>(new int[] { 3 })); //one connection would be LINEAR, multiple would be choices/checklists

            List<int> currentQuest = new List<int> { 1 };

            questLookup[currentQuest[0]].listCurrentQuest();

            currentQuest = completeQuest(ref questLookup, currentQuest[0]); //example

            questLookup[currentQuest[0]].listCurrentQuest();

            //ALTERNATIVE
            //var i = questLookup[currentQuest[0]];
            //Console.WriteLine("Current quest: {0}, {1}", i.name, i.objective);
        }
    }
}
