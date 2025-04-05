CREATE TABLE User (
    UserID INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(255) NOT NULL,
    Role ENUM('Tester', 'Developer', 'Admin') NOT NULL
);

CREATE TABLE Project (
    ProjectID INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(255) NOT NULL,
    Description TEXT
);

-- Linking Users to Projects
CREATE TABLE User_Project (
    UserID INT,
    ProjectID INT,
    Role ENUM('Tester', 'Developer', 'Admin'),
    PRIMARY KEY (UserID, ProjectID),
    FOREIGN KEY (UserID) REFERENCES User(UserID),
    FOREIGN KEY (ProjectID) REFERENCES Project(ProjectID)
);

CREATE TABLE TestCase (
    TestCaseID INT PRIMARY KEY AUTO_INCREMENT,
    ProjectID INT,
    CreatedBy INT,
    Description TEXT NOT NULL,
    State ENUM('Pending', 'Running', 'Completed'),
    FOREIGN KEY (ProjectID) REFERENCES Project(ProjectID),
    FOREIGN KEY (CreatedBy) REFERENCES User(UserID)
);

-- History table to track state changes
CREATE TABLE TestCaseHistory (
    HistoryID INT PRIMARY KEY AUTO_INCREMENT,
    TestCaseID INT,
    ChangedBy INT,
    OldState ENUM('Pending', 'Running', 'Completed'),
    NewState ENUM('Pending', 'Running', 'Completed'),
    ChangeDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (TestCaseID) REFERENCES TestCase(TestCaseID),
    FOREIGN KEY (ChangedBy) REFERENCES User(UserID)
);

CREATE TABLE TestReport (
    ReportID INT PRIMARY KEY AUTO_INCREMENT,
    TestCaseID INT,
    ReviewedBy INT,
    State ENUM('Pending', 'Running', 'Completed'),
    Result TEXT NOT NULL,
    FOREIGN KEY (TestCaseID) REFERENCES TestCase(TestCaseID),
    FOREIGN KEY (ReviewedBy) REFERENCES User(UserID)
);

