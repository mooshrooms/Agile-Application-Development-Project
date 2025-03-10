﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.AccessControl;

namespace testing
{
    // Currently I do not know how to implement the database connection in C#. I will ask the teacher for help tmr.
    // Class representing a customer
    class Customer
    {
        // Private fields for customer ID, name, role ID, and role
        private int custID;
        private string name;
        private int roleID;
        private string role;

        // Public property to get the customer's name
        public string Name
        {
            get { return name; }
        }

        // Public property to get the customer's role
        public string Role
        {
            get { return role; }
        }

        // Public property to get the customer's ID
        public int CustID
        {
            get { return custID; }
        }

        // Constructor to initialize a customer with ID, name, and role
        public Customer(int c, string nm, string rl)
        {
            custID = c;
            name = nm;
            role = rl;
        }

        // Method to add a role to the customer
        public void addRole(int rl_id, string rl)
        {
            roleID = rl_id;
            // Loop to ensure a valid role ID is entered
            while (roleID != 1 && roleID != 2 && roleID != 3)
            {
                Console.WriteLine("Invalid role ID. Please enter a valid role ID.");
                roleID = Convert.ToInt32(Console.ReadLine());
                // Assign role based on role ID
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

        // Method to remove a role from the customer
        public void removeRole(int rl_id_remove, string nm)
        {
            role = null;
        }
    }

    // Class representing a project
    class Project
    {
        // Private fields for project ID, name, description, and status
        private int projID;
        private string projName;
        private string projDesc;
        private string projStatus;

        // Public property to get the project's name
        public string ProjName
        {
            get { return projName; }
        }
        // Public property to get the project's description
        public string ProjDesc
        {
            get { return projDesc; }
        }
        // Public property to get the project's status
        public string ProjStatus
        {
            get { return projStatus; }
        }
        // Public property to get the project's ID
        public int ProjID
        {
            get { return projID; }
        }
        // Constructor to initialize a project with ID, name, description, and status
        public Project(int p, string pn, string pd, string ps)
        {
            projID = p;
            projName = pn;
            projDesc = pd;
            projStatus = ps;
        }
        // Method to add a project
        public void addProject(string pn, string pd, string ps, string psd)
        {
            projName = pn;
            projDesc = pd;
            projStatus = ps;
        }
        // Method to delete a project
        public void deleteProject()
        {
            projName = null;
            projDesc = null;
            projStatus = null;
        }
        // Method to get the project ID from user input
        public void getProjectID()
        {
            Console.WriteLine("Enter the project ID: ");
            projID = Convert.ToInt32(Console.ReadLine());
        }
        // Method to get the project's name
        public string getProjectName()
        {
            return projName;
        }
        // Override ToString method to return the project's name
        public override string ToString()
        {
            return projName;
        }
        // Method to edit a project's details
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

    // Class representing a test case
    class testCase
    {
        // Private fields for test case ID, state, and instruction
        private int caseID;
        private string caseState;
        private string caseInstruction;

        // Public property to get the test case's state
        public string CaseState
        {
            get { return caseState; }
        }
        // Public property to get the test case's instruction
        public string CaseInstruction
        {
            get { return caseInstruction; }
        }
        // Public property to get the test case's ID
        public int CaseID
        {
            get { return caseID; }
        }
        // Constructor to initialize a test case with ID, state, and instruction
        public testCase(int c, string cs, string ci)
        {
            caseID = c;
            caseState = cs;
            caseInstruction = ci;
        }
        // Method to add a test case
        public void addTestCase(string cs, string ci)
        {
            caseState = cs;
            caseInstruction = ci;
        }
        // Method to assign a test case
        public void assignTestCase(string cs, string ci)
        {
            caseState = cs;
            caseInstruction = ci;
        }

        // Method to delete a test case
        public void deleteTestCase()
        {
            caseState = null;
            caseInstruction = null;
        }
        // Method to get the test case ID from user input
        public void getCaseID()
        {
            Console.WriteLine("Enter the test case ID: ");
            caseID = Convert.ToInt32(Console.ReadLine());
        }
    }

    // Class representing a test report, inheriting from testCase
    class testReport : testCase
    {
        // Private fields for report ID, state, instruction, and result
        private int reportID;
        private string reportState;
        private string reportInstruction;
        private string reportResult;
        // Public property to get the report's state
        public string ReportState
        {
            get { return reportState; }
        }
        // Public property to get the report's instruction
        public string ReportInstruction
        {
            get { return reportInstruction; }
        }
        // Public property to get the report's result
        public string ReportResult
        {
            get { return reportResult; }
        }
        // Public property to get the report's ID
        public int ReportID
        {
            get { return reportID; }
        }
        // Constructor to initialize a test report with ID, state, instruction, and result
        public testReport(int r, string rs, string ri, string rr) : base(r, rs, ri)
        {
            reportID = r;
            reportState = rs;
            reportInstruction = ri;
            reportResult = rr;
        }
        // Method to add a test report
        public void addTestReport(string rs, string ri, string rr)
        {
            reportState = rs;
            reportInstruction = ri;
            reportResult = rr;
        }
        // Method to delete a test report
        public void deleteTestReport()
        {
            reportState = null;
            reportInstruction = null;
            reportResult = null;
        }
        // Method to get the test report ID from user input
        public void getReportID()
        {
            Console.WriteLine("Enter the test report ID: ");
            reportID = Convert.ToInt32(Console.ReadLine());
        }
        // Nested class representing test history, inheriting from testReport
        class testHistory : testReport
        {
            // Private fields for history ID, state, instruction, and result
            private int historyID;
            private string historyState;
            private string historyInstruction;
            private string historyResult;

            // Constructor to initialize test history with ID, state, instruction, and result
            public testHistory(int r, string rs, string ri, string rr) : base(r, rs, ri, rr)
            {
            }

            // Public property to get the history's state
            public string HistoryState
            {
                get { return historyState; }
            }
            // Public property to get the history's instruction
            public string HistoryInstruction
            {
                get { return historyInstruction; }
            }
            // Public property to get the history's result
            public string HistoryResult
            {
                get { return historyResult; }
            }
        }

        // Nested class representing test status, inheriting from testReport
        class testStatus : testReport
        {
            // Private fields for status ID, state, instruction, and result
            private int statusID;
            private string statusState;
            private string statusInstruction;
            private string statusResult;
            // Constructor to initialize test status with ID, state, instruction, and result
            public testStatus(int r, string rs, string ri, string rr) : base(r, rs, ri, rr)
            {
            }
            // Public property to get the status's state
            public string StatusState
            {
                get { return statusState; }
            }
            // Public property to get the status's instruction
            public string StatusInstruction
            {
                get { return statusInstruction; }
            }
            // Public property to get the status's result
            public string StatusResult
            {
                get { return statusResult; }
            }
        }
    }

    // Class representing a test component
    class testComponent
    {
        // Private fields for component ID, name, and state
        private int compID;
        private string compName;
        private string compState;

        // Public property to get the component's name
        public string CompName
        {
            get { return compName; }
        }
        // Public property to get the component's state
        public string CompState
        {
            get { return compState; }
        }
        // Public property to get the component's ID
        public int CompID
        {
            get { return compID; }
        }

        // Constructor to initialize a test component with ID, name, and state
        public testComponent(int c, string cn, string cs)
        {
            compID = c;
            compName = cn;
            compState = cs;
        }
        // Method to add a component
        public void addComponent(string cn, string cs)
        {
            compName = cn;
            compState = cs;
        }
        // Method to delete a component
        public void deleteComponent()
        {
            compName = null;
            compState = null;
        }
    }

    // Main program class
    class Program
    {
        // Main method, entry point of the program
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}