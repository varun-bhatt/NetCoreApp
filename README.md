## Track Personal Expenses - BudgetBuddy
## Overview
BudgetBuddy is a simple and easy-to-use app that helps you keep track of your spending and stay on top of your finances. With BudgetBuddy, you can organize your expenses and understand where your money goes. BudgetBuddy is the perfect companion for anyone aiming to take control of their finances.
## Key Features
**Track Your Spending**
 - Add new expenses whenever you spend money.
 - Update or edit details if something changes.
 - Delete any expenses you no longer need.
 **Organise Your Spending by Categories**
- Create categories like "Groceries," "Transport, etc.. to organise your expenses.
- Update categories whenever you need.
**View All Your Expenses**
- See a full list of everything you've spent, all in one place.
- Filter or sort by date, category, or amount to find what you're looking for.
**Categorise Expenses**
- Easily group expenses into categories to see how much you're spending in each area.
**See a Summary of Your Spending**
- Get a quick overview of your expenses by category to understand your spending habits better.
**Search for Specific Expenses**
- Quickly find specific expenses using keywords, dates, or amounts.
- Save time and avoid scrolling through long lists.
**View Your Spending in Charts**
- See your expenses as interactive charts for a clear and visual understanding of your spending patterns.
BudgetBuddy makes it simple to stay on top of your budget and manage your money wisely. It's like having a personal assistant for your finances!
## Key Features
**Frontend**  - ReactJs
**Backend**  - .Net core
**Database**  - SQL
**Visualization**  - Chart.js
## Installation
### Prerequisites
- Windows machine
- Visual studio code/Rider
- Node.js (v18.x)
- Docker
- .Net 8.0
### Steps
**Backend**
- Clone the repository - https://github.com/varun-bhatt/NetCoreApp.git
- Open it in Visual studio code/Rider and run the application
**Frontend**
- Clone the repository - https://github.com/your-repo/ai-code-challenge.git
- Open it in Visual studio code
- To Install dependencies execute
  ```
   npm install
   ```
- Run the development server
  ```
   npm run dev
   ```
- Open your browser and navigate to
   ```
   http://localhost:3000
   ```
- Install Docker desktop and download `mcr.microsoft.com/mssql/server:2022-latest` image
- Run container in docker consisting SQL server image
- Execute the following SQL script in the SQL server
```
CREATE TABLE ExpenseCategory (
    Id INT IDENTITY(1,1), -- Primary Key
    Name VARCHAR(20) NOT NULL,
	IsDeleted bit NOT NULL, 
    CreatedAt DATETIME2 NOT NULL,
    LastModifiedAt DATETIME2 NULL,
	CONSTRAINT PK_ExpenseCategory PRIMARY KEY CLUSTERED (Id),

);

CREATE TABLE Expenses (
    Id BIGINT IDENTITY(1,1), -- Primary Key
    Name VARCHAR(20) NOT NULL,
    Description VARCHAR(250) NULL,
    Amount DECIMAL(18, 2), -- Check constraint
    ExpenseCategoryId INT NOT NULL, -- Foreign Key to ExpenseCategory
	UserId BigInt NOT NULL,
	IsDeleted bit NOT NULL,
    CreatedAt DATETIME2 NOT NULL,
    LastModifiedAt DATETIME2 NULL,
	CONSTRAINT PK_Expenses PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_Expenses_CategoryId FOREIGN KEY (ExpenseCategoryId) REFERENCES ExpenseCategory(Id),
	CONSTRAINT FK_UserId FOREIGN KEY (UserId) REFERENCES Users(Id),
);

ALTER TABLE ExpenseCategory
ADD CONSTRAINT UK_Name UNIQUE (Name);

CREATE TABLE Users (
    Id BigInt IDENTITY(1,1),
    Name VARCHAR(250) NOT NULL,
	Email Varchar(250) NOT NULL,
    Password NVARCHAR(1000) NOT NULL,
	CreatedAt DATETIME2 NOT NULL,
	CONSTRAINT PK_Users PRIMARY KEY CLUSTERED (Id),
	CONSTRAINT UK_Email UNIQUE (Email)
);
```
   
## Usage
- The application features a login screen where users can enter their email and password.
- Upon successful authentication, users will be welcomed on the main page.
- Start adding expenses under categories and viewing reports.

## Database Schema Design
![Database-design](https://github.com/user-attachments/assets/07fc9f4e-ac8a-415c-9c44-589806cc093a)

