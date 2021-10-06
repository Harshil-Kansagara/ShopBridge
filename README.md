# Shop Bridge 
**Shop Bridge** is a web application that helps track the different items for sale. It is an inventory management system which keeps records of the items with their name, description and price.

Backend functionality of this project is build using netcoreapp3.1 Framework following MVC architecture and Frontend funcitonality of this project is build using Angular 12+ language.

---
## Table of Contents 
- [System Requirements](#-system-requirements)
- [Setup](#-setup)
- [Run Project](#-run-project)
- [Run Tests](#-run-tests)
- [Time Tracking](#-time-tracking)
---

## ⚙ System Requirements
* IDE -> **Visual Studio 2019**
* Databe -> **PostgreSQL**
* **IIS** should be installed.
* **Node** should be installed.
* **PostgreSQL** should be installed.
---

## 🛠 Setup
1. Download or clone the project from this repository.
2. Right-click on downloaded zip file. Click Properties. Check the checkbox for **Unblock**. Click Apply.
	> You can skip this step if you are cloning the repository.
3. Open **pgAdmin 4**.
	> Create new Database, named as **ShopBridge**.
4. Open **ShopBridge.sln** file via Visual Studio.
5. Open **Package Manager Console** in Visual Studio _(**Tools > NuGet Package Manager > Package Manager Console**)_.
	> Make sure Default Project is selected as **ShopBridge** in the Package Manager Console.
6. Copy the below command and paste it in the Package Manager Console window.
    
	`PM > update-database`
	
	> NOTE : After pasting, hit *Enter*.
7. Open **Shop-Bridge-Client** folder in cmd console.
8. Copy the below command and paste it in the cmd console.
    
	`npm install`
---

## ⌛ Run Project
* For running backend project, open Visual Studio. Make sure ShopBridge is selected as a startup project and press **F5** in the keyboard.
* For running forntend project, Open **Shop-Bridge-Client** folder in cmd console and Copy the below command and paste it in the cmd console.
  
  `ng serve`
---

## 🧪 Run Tests

* Go to _**Test > Test Explorer**_.
* Click on **Run All Test** icon.
---

## 🕔 Time Tracking

* Backend Functionality with Unit Test case (Followed TDD approach) - 6 hours
* Frontend Functionality and Presentation - 2 hours
* Backend-Frontend Integration - 3 hours
