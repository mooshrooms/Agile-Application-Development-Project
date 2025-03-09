using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace testing
{
    class Customer
    {
        private int custID;
        private string name;
        private int roleID;
        private string role;

        public string Name
        {
            get { return name; }
        }

        public string Role
        {
            get { return role; }
        }

        public int CustID
        {
            get { return custID; }
        }

        public Customer(int c, string nm, string rl)
        {

            custID = c;
            name = nm;
            role = rl;
        }

        public void addRole(int rl_id, string rl)
        {
            roleID = rl_id;
            while (roleID != 1 && roleID != 2 && roleID != 3)
            {
                Console.WriteLine("Invalid role ID. Please enter a valid role ID.");
                roleID = Convert.ToInt32(Console.ReadLine());
                switch (roleID)
                {
                    case 1:
                        rl = "Admin";
                        break;
                    case 2:
                        rl = "User";
                        break;
                    case 3:
                        rl = "Guest";
                        break;
                    default:
                        Console.WriteLine("Invalid role ID. Please enter a valid role ID.");
                        break;
                }
            }
            role = rl;
        }

        public void removeRole(int rl_id_remove, string nm)
        {
            
            role = null;
        }   
    }

    class Project
    {
        private int projID;
        private string projName;
        private string projDesc;
        private string projStatus;

        public string ProjName
        {
            get { return projName; }
        }
        public string ProjDesc
        {
            get { return projDesc; }
        }
        public string ProjStatus
        {
            get { return projStatus; }
        }
        public int ProjID
        {
            get { return projID; }
        }
        public Project(int p, string pn, string pd, string ps)
        {
            projID = p;
            projName = pn;
            projDesc = pd;
            projStatus = ps;
        }
        public void addProject(string pn, string pd, string ps, string psd)
        {
            projName = pn;
            projDesc = pd;
            projStatus = ps;
        }
        public void deleteProject()
        {
            projName = null;
            projDesc = null;
            projStatus = null;
        }
        public void getProjectID()
        {
            Console.WriteLine("Enter the project ID: ");
            projID = Convert.ToInt32(Console.ReadLine());
        }
        public string getProjectName()
        {
            return projName;
        }
        public override string ToString()
        {
            return projName;
        }
        public void editProject()
        {
            Console.WriteLine("Enter the project name: ");
            projName = Console.ReadLine();
            Console.WriteLine("Enter the project description: ");
            projDesc = Console.ReadLine();
            Console.WriteLine("Enter the project status: ");
            projStatus = Console.ReadLine();
        }
    }

    class testCase
    {
        private int caseID;
        private string caseState;
        private string caseInstruction;


        public string CaseState
        {
            get { return caseState; }
        }
        public string CaseInstruction
        {
            get { return caseInstruction; }
        }
        public int CaseID
        {
            get { return caseID; }
        }
        public testCase(int c, string cs, string ci)
        {
            caseID = c;
            caseState = cs;
            caseInstruction = ci;
        }
        public void addTestCase(string cs, string ci)
        {
            caseState = cs;
            caseInstruction = ci;
        }
        public void deleteTestCase()
        {
            caseState = null;
            caseInstruction = null;
        }
        public void getCaseID()
        {
            Console.WriteLine("Enter the test case ID: ");
            caseID = Convert.ToInt32(Console.ReadLine());
        }
    }




    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}