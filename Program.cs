using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using System.Data;

namespace software_testmng_system
{
    //just one the main class
    class Project
    {
        private int project_id;
        private string name;
        private string description;
        private string state;

        //properties
        //found short version for writing properties: public int Project_id => project_id;
        public int Project_id
        {
            get { return project_id; }
        }

        public string Name
        {
            get { return name; }
        }

        public string Description
        {
            get { return description; }
        }

        public string State
        {
            get { return state; }
        }

        //just constructor
        public Project(int id, string nm, string dscrptn, string st)
        {

            project_id = id;
            name = nm;
            description = dscrptn;
            state = st;
        }

        public override string ToString()
        {
            return $"Project ID: {project_id}, Name: {name}, State: {state}";
        }
    }


    // if u read our concept of 3tier u know we will have 3 layer UI, DAL and BLL.
    // i marked here places where they start, abviously i could made mistakes, guys please also doublecheck my code 

    // Data Access Layer (DAL) - handles DB connection and queries
    class DataService
    {
        private MySqlConnection myConnection;

        // constructor which initializes the database connection.don't forget to start xampp (server,mysql) firstly
        public DataService()
        {
            String connstr;

            connstr = "server=localhost;user=root;database=software_test;port=3306;password=;";

            myConnection = new MySqlConnection(); //MySqlConnection included in MySQL.Data package. Install MySQL.Data firstly
            myConnection.ConnectionString = connstr; //ConnectionString-property of MySqlConnection how to connect to the database.
            myConnection.Open();
        }



        //helper method to get data from the database(execute the SQL query and return a MySqlDataReader)
        //for list (not particular value)
        private MySqlDataReader GetData(string[] fields, string table)
        {
            MySqlCommand myCommand = new MySqlCommand();

            myCommand.Connection = myConnection;
            // SQL query string
            myCommand.CommandText = "SELECT ";

            foreach (string s in fields)
                myCommand.CommandText += s + ", ";

            myCommand.CommandText = myCommand.CommandText.Remove(myCommand.CommandText.LastIndexOf(","));
            myCommand.CommandText += " FROM " + table;
            // CommandType requires namespace System.Data
            myCommand.CommandType = CommandType.Text;

            // Execute the SQL request command and
            // store the output in myReader object
            MySqlDataReader myReader;
            myReader = myCommand.ExecuteReader();

            return myReader;
        }

        //helper method to get data from the database(execute the SQL query and return a MySqlDataReader)
        //for  particular value in our case Name of project
        private MySqlDataReader GetDataWhereString(string[] fields, string table, string columnName, string value)
        {
            MySqlCommand myCommand = new MySqlCommand();

            myCommand.Connection = myConnection;
            // SQL query string
            myCommand.CommandText = "SELECT ";

            foreach (string s in fields)
                myCommand.CommandText += s + ", ";

            myCommand.CommandText = myCommand.CommandText.Remove(myCommand.CommandText.LastIndexOf(","));
            myCommand.CommandText += $" FROM {table} WHERE {columnName} = @value";
            // CommandType requires namespace System.Data
            myCommand.CommandType = CommandType.Text;

            // Add parameter to prevent SQL injection
            myCommand.Parameters.AddWithValue("@value", value);

            // Execute the SQL request command and
            // store the output in myReader object
            MySqlDataReader myReader;
            myReader = myCommand.ExecuteReader();

            return myReader;
        }

        // method to fetch all projects 
        public List<Project> GetAllProjects()
        {
            List<Project> projectList = new List<Project>();

            string[] fields = { "project_id", "name", "description", "state" };
            string table = "project";

            using (MySqlDataReader myReader = GetData(fields, table))
            {
                bool notEoF = myReader.Read(); // Read first row
                while (notEoF) // Continue reading until the last row
                {
                    int projectId = Convert.ToInt32(myReader["project_id"].ToString());
                    string name = myReader["name"].ToString();
                    string description = myReader["description"].ToString();
                    string state = myReader["state"].ToString();

                    Project newProject = new Project(projectId, name, description, state);
                    projectList.Add(newProject);

                    notEoF = myReader.Read(); // Read next row
                }
            }

            return projectList;
        }

        // method to fetch project by name
        public Project GetProjectByName(string projectName)
        {
            Project newProject = null;
            string[] fields = { "project_id", "name", "description", "state" };
            string table = "project";

            MySqlDataReader myReader = null;
            try
            {
                myReader = GetDataWhereString(fields, table, "name", projectName);
                bool notEoF = myReader.Read();
                while (notEoF)
                {
                    int projectId = Convert.ToInt32(myReader["project_id"].ToString());
                    string name = myReader["name"].ToString();
                    string description = myReader["description"].ToString();
                    string state = myReader["state"].ToString();
                    newProject = new Project(projectId, name, description, state);
                    break;
                }
            }
            finally
            {
                if (myReader != null && !myReader.IsClosed)
                {
                    myReader.Close();
                }
            }
            return newProject;
        }

        public void AddProject(Project project)
        {
            string query = "INSERT INTO project (name, description, state) VALUES (@name, @description, @state);";
            using (MySqlCommand cmd = new MySqlCommand(query, myConnection))
            {
                cmd.Parameters.AddWithValue("@name", project.Name);
                cmd.Parameters.AddWithValue("@description", project.Description);
                cmd.Parameters.AddWithValue("@state", project.State);
                cmd.ExecuteNonQuery();
            }
        }

        public void RemoveProject(int projectId)
        {
            string query = "DELETE FROM project WHERE project_id = @projectId;";

            using (MySqlCommand cmd = new MySqlCommand(query, myConnection))
            {
                cmd.Parameters.AddWithValue("@projectId", projectId);
                cmd.ExecuteNonQuery();
            }
        }

        public void ResetAutoIncrement()
        {
            // Find the highest existing ID
            string query = "SELECT MAX(project_id) FROM project;";
            int maxId = 0;
            using (MySqlCommand cmd = new MySqlCommand(query, myConnection))
            {
                maxId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            // Reset auto-increment to the next highest value
            query = $"ALTER TABLE project AUTO_INCREMENT = {maxId + 1};";
            using (MySqlCommand cmd = new MySqlCommand(query, myConnection))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }


    // Business Logic Layer (BLL) - handles the business logic and interacts with the Data Access Layer (DAL)
    class MyApplication
    {
        //connection to the database
        DataService myDataService;

        public MyApplication()
        {
            myDataService = new DataService();
        }

        //This method get all projects from the database using the GetAllProjects method of DataService
        //It concatenates the string representation of each project (using ToString()) into a single string, separated by new lines.
        public string GetAllProjects()
        {
            string projects = "";
            foreach (Project p in myDataService.GetAllProjects())
                projects += p.ToString() + "\n";
            return projects.Trim();
        }
        //same but for particular project
        public Project GetProjectDataByName(string projectName)
        {
            return myDataService.GetProjectByName(projectName);
        }

        public void AddProject(string name, string description)
        {
            string defaultState = "To be implemented";
            Project newProject = new Project(0, name, description, defaultState); // Assuming project_id is auto-incremented
            myDataService.AddProject(newProject);
        }
        public void RemoveProject(int projectId)
        {
            myDataService.RemoveProject(projectId);
            myDataService.ResetAutoIncrement();

        }

    }

    // layer UI nothing to comment i guess
    class UI
    {
        //connection with BLL
        MyApplication myApp = new MyApplication();

        public void ShowMenu()
        {
            Console.WriteLine("What would you like to do? Please enter the appropriate number:");
            Console.WriteLine("1. Show all projects");
            Console.WriteLine("2. Show project details by name");
            Console.WriteLine("3. Add new project");
            Console.WriteLine("4. Remove project");
            Console.WriteLine("5. Stop app");
        }

        public void Run()
        {
            ShowMenu();
            string command = Console.ReadLine();

            while (true)
            {
                switch (command)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine(myApp.GetAllProjects());
                        break;

                    case "2":
                        Console.Clear();
                        Console.Write("Enter project name: ");
                        string projectName = Console.ReadLine();
                        Project project = myApp.GetProjectDataByName(projectName);
                        if (project != null)
                            Console.WriteLine($"Project ID: {project.Project_id}, Name: {project.Name}, Description: {project.Description}, State: {project.State}");
                        else
                            Console.WriteLine("Project not found.");
                        break;

                    case "3":
                        Console.Clear();
                        AddNewProject();
                        break;

                    case "4":
                        Console.Clear();
                        RemoveProject();
                        break;

                    case "5":
                        Console.WriteLine("Exiting...");
                        return;

                    default:
                        Console.WriteLine("Invalid input, try again.");
                        break;
                }
                ShowMenu();
                command = Console.ReadLine();
            }
        }
        public void AddNewProject()
        {
            Console.Write("Enter project name: ");
            string name = Console.ReadLine();
            Console.Write("Enter project description: ");
            string description = Console.ReadLine();

            myApp.AddProject(name, description);
            Console.WriteLine("Project added successfully with default state 'To be implemented'.");
        }

        public void RemoveProject()
        {
            Console.Clear();
            Console.WriteLine("Current projects:");

            string projects = myApp.GetAllProjects();

            if (string.IsNullOrWhiteSpace(projects))
            {
                Console.WriteLine("No projects available to remove.");
                return;
            }

            Console.WriteLine(projects);
            Console.Write("\nEnter the Project ID to remove: ");

            if (int.TryParse(Console.ReadLine(), out int projectId))
            {
                myApp.RemoveProject(projectId);
                Console.WriteLine($"Project with ID {projectId} removed successfully.");
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid Project ID.");
            }
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            UI testUI = new UI();
            testUI.Run();
        }
    }
}
