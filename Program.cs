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
    // Represents a project entity with properties and methods.
    class Project
    {
        private int project_id; // Unique identifier for the project.
        private string name; // Name of the project.
        private string description; // Description of the project.
        private string state; // Current state of the project.

        // Public property to access the project ID.
        public int Project_id
        {
            get { return project_id; }
        }

        // Public property to access the project name.
        public string Name
        {
            get { return name; }
        }

        // Public property to access the project description.
        public string Description
        {
            get { return description; }
        }

        // Public property to access the project state.
        public string State
        {
            get { return state; }
        }

        // Constructor to initialize a project object.
        public Project(int id, string nm, string dscrptn, string st)
        {
            project_id = id;
            name = nm;
            description = dscrptn;
            state = st;
        }

        // Overrides the ToString method to provide a readable representation of the project.
        public override string ToString()
        {
            return $"Project ID: {project_id}, Name: {name}, State: {state}";
        }
    }


    // Data Access Layer (DAL) - Handles database connection and queries.
    class DataService
    {
        private MySqlConnection myConnection; // Connection to the MySQL database.

        // Constructor initializes the database connection.
        public DataService()
        {
            String connstr = "server=localhost;user=root;database=software_test;port=3306;password=;";
            myConnection = new MySqlConnection();
            myConnection.ConnectionString = connstr;
            myConnection.Open(); // Opens the database connection.
        }

        // Helper method to fetch data from the database for a list of fields.
        private MySqlDataReader GetData(string[] fields, string table)
        {
            MySqlCommand myCommand = new MySqlCommand();
            myCommand.Connection = myConnection;

            // Constructs the SELECT query dynamically based on the fields.
            myCommand.CommandText = "SELECT ";
            foreach (string s in fields)
                myCommand.CommandText += s + ", ";
            myCommand.CommandText = myCommand.CommandText.Remove(myCommand.CommandText.LastIndexOf(","));
            myCommand.CommandText += " FROM " + table;

            myCommand.CommandType = CommandType.Text;
            return myCommand.ExecuteReader(); // Executes the query and returns the result.
        }

        // Helper method to fetch data with a WHERE clause for a specific value.
        private MySqlDataReader GetDataWhereString(string[] fields, string table, string columnName, string value)
        {
            MySqlCommand myCommand = new MySqlCommand();
            myCommand.Connection = myConnection;

            // Constructs the SELECT query with a WHERE clause.
            myCommand.CommandText = "SELECT ";
            foreach (string s in fields)
                myCommand.CommandText += s + ", ";
            myCommand.CommandText = myCommand.CommandText.Remove(myCommand.CommandText.LastIndexOf(","));
            myCommand.CommandText += $" FROM {table} WHERE {columnName} = @value";

            myCommand.CommandType = CommandType.Text;
            myCommand.Parameters.AddWithValue("@value", value); // Prevents SQL injection.
            return myCommand.ExecuteReader();
        }

        // Fetches all projects from the database.
        public List<Project> GetAllProjects()
        {
            List<Project> projectList = new List<Project>();
            string[] fields = { "project_id", "name", "description", "state" };
            string table = "project";

            using (MySqlDataReader myReader = GetData(fields, table))
            {
                while (myReader.Read()) // Reads each row from the result set.
                {
                    int projectId = Convert.ToInt32(myReader["project_id"].ToString());
                    string name = myReader["name"].ToString();
                    string description = myReader["description"].ToString();
                    string state = myReader["state"].ToString();

                    Project newProject = new Project(projectId, name, description, state);
                    projectList.Add(newProject);
                }
            }

            return projectList;
        }

        // Fetches a project by its name.
        public Project GetProjectByName(string projectName)
        {
            Project newProject = null;
            string[] fields = { "project_id", "name", "description", "state" };
            string table = "project";

            MySqlDataReader myReader = null;
            try
            {
                myReader = GetDataWhereString(fields, table, "name", projectName);
                if (myReader.Read()) // Reads the first matching row.
                {
                    int projectId = Convert.ToInt32(myReader["project_id"].ToString());
                    string name = myReader["name"].ToString();
                    string description = myReader["description"].ToString();
                    string state = myReader["state"].ToString();
                    newProject = new Project(projectId, name, description, state);
                }
            }
            finally
            {
                if (myReader != null && !myReader.IsClosed)
                {
                    myReader.Close(); // Ensures the reader is closed after use.
                }
            }
            return newProject;
        }

        // Adds a new project to the database.
        public void AddProject(Project project)
        {
            string query = "INSERT INTO project (name, description, state) VALUES (@name, @description, @state);";
            using (MySqlCommand cmd = new MySqlCommand(query, myConnection))
            {
                cmd.Parameters.AddWithValue("@name", project.Name);
                cmd.Parameters.AddWithValue("@description", project.Description);
                cmd.Parameters.AddWithValue("@state", project.State);
                cmd.ExecuteNonQuery(); // Executes the INSERT query.
            }
        }

        // Removes a project from the database by its ID.
        public void RemoveProject(int projectId)
        {
            string query = "DELETE FROM project WHERE project_id = @projectId;";
            using (MySqlCommand cmd = new MySqlCommand(query, myConnection))
            {
                cmd.Parameters.AddWithValue("@projectId", projectId);
                cmd.ExecuteNonQuery(); // Executes the DELETE query.
            }
        }

        // Resets the auto-increment value for the project table.
        public void ResetAutoIncrement()
        {
            string query = "SELECT MAX(project_id) FROM project;";
            int maxId = 0;
            using (MySqlCommand cmd = new MySqlCommand(query, myConnection))
            {
                maxId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            query = $"ALTER TABLE project AUTO_INCREMENT = {maxId + 1};";
            using (MySqlCommand cmd = new MySqlCommand(query, myConnection))
            {
                cmd.ExecuteNonQuery(); // Updates the auto-increment value.
            }
        }
    }


    // Business Logic Layer (BLL) - Handles business logic and interacts with the DAL.
    class MyApplication
    {
        DataService myDataService; // Connection to the Data Access Layer.

        public MyApplication()
        {
            myDataService = new DataService();
        }

        // Retrieves all projects and concatenates their string representations.
        public string GetAllProjects()
        {
            string projects = "";
            foreach (Project p in myDataService.GetAllProjects())
                projects += p.ToString() + "\n";
            return projects.Trim();
        }

        // Retrieves a specific project by its name.
        public Project GetProjectDataByName(string projectName)
        {
            return myDataService.GetProjectByName(projectName);
        }

        // Adds a new project with a default state.
        public void AddProject(string name, string description)
        {
            string defaultState = "To be implemented";
            Project newProject = new Project(0, name, description, defaultState);
            myDataService.AddProject(newProject);
        }

        // Removes a project by its ID and resets the auto-increment value.
        public void RemoveProject(int projectId)
        {
            myDataService.RemoveProject(projectId);
            myDataService.ResetAutoIncrement();
        }
    }

    // User Interface Layer (UI) - Handles user interaction.
    class UI
    {
        MyApplication myApp = new MyApplication(); // Connection to the Business Logic Layer.

        // Displays the main menu options.
        public void ShowMenu()
        {
            Console.WriteLine("Please enter the appropriate number for usage:");
            Console.WriteLine("1. Show all projects");
            Console.WriteLine("2. Show project details by name");
            Console.WriteLine("3. Add new project");
            Console.WriteLine("4. Remove project");
            Console.WriteLine("5. Stop app");
        }

        // Main loop to handle user commands.
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
                            Console.WriteLine("Project not found. Please try again.");
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
                        Console.WriteLine("Closing...");
                        return;

                    default:
                        Console.WriteLine("Invalid input, please try again.");
                        break;
                }
                ShowMenu();
                command = Console.ReadLine();
            }
        }

        // Handles adding a new project.
        public void AddNewProject()
        {
            Console.Write("Enter project name: ");
            string name = Console.ReadLine();
            Console.Write("Enter project description: ");
            string description = Console.ReadLine();

            myApp.AddProject(name, description);
            Console.WriteLine("Project added successfully! Default state 'To be implemented'.");
        }

        // Handles removing a project by its ID.
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


    // Entry point of the application.
    class Program
    {
        static void Main(string[] args)
        {
            UI testUI = new UI();
            testUI.Run();
        }
    }
}
