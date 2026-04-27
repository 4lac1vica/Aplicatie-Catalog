# cataLOG

**Tools : Visual Studio (IDE), ASP.NET, Entity Framework, REACT.** 

[jump to **ENGLISH** README](https://github.com/4lac1vica/Aplicatie-Catalog?tab=readme-ov-file#en)

## RO 

Aplicatie pentru gestionarea unui catalog cu studenti. 

Acest proiect are ca si scop creearea unei aplicatii care propune gestionarea unui catalog de studenti. In cadrul acestei aplicatii sunt disponibile diverse **functionalitati**, in functie de rolul pe care il are utilizatorul. Partea de *frontend* este scrisa in : **HTML** si **JavaScript**, folosind **React**, iar partea de *backend* este scrisa in **C#**, folosind **ASP.NET** si **Entity Framework**. 
Persistenta datelor este asigurata de conexiunea la baza de date, gestionata cu ajutorul **Microsoft SQL Server**. Mai multe detalii se pot gasi in fisierul **Deliverables2026.docx file**, sau **Deliverables2026_DX.pdf** care este documentatia proiectului/aplicatiei.

**FUNCTIONALITATI :**

1) **Student**
   - Login
   - Logout
   - Delete Account
   - Edit Profile
   - Vede Notele 
   - Inregistrare
   - Vede Detaliile Profilului 
   - Cauta/Vede alti utilizatori
   - Vede Absente 
   - Primeste Notificari

   
2) **Profesor**
   - Login
   - Logout
   - Delete Account
   - Edit Profile
   - Pune Notele
   - Inregistrare
   - Vede Detaliile Profilului
   - Cauta/Vede alti utilizatori
   - Pune absente 
   - Primeste Notificari
   
     
3) **Utilizator Anonim**
   - Cauta si vede studenti/profesori si detaliile acestora (detaliile esentiale precum contactul sunt ascunse)

4) **Admin**

   - Login
   - Adauga Materii
   - Sterge Materii
   - Sterge Useri (Studenti / Profesori)
   

## Use-Case Diagram 


<img width="1351" height="678" alt="image" src="https://github.com/user-attachments/assets/8dec21b0-b6eb-492b-870d-29486fb4c285" />

**Alte detalii**

-> aplicatia foloseste mai multe backenduri, doua **API** si o **aplicatie frontend REACT**. 



## Class Diagram 



## EN

Application for managing a student catalog.

This project aims to create an application that allows the management of a student catalog. Within this application, various **features** are available, depending on the role of the user. The *frontend* part is written in **HTML** and **JavaScript**, using **Razor Pages**, while the *backend* part is written in **C#**, using **ASP.NET** and **Entity Framework**. Data persistence is ensured by the database connection, managed with the help of **Microsoft SQL Server**. More details are available in the **Deliverables2026.docx** or **Deliverables2026_DX.pdf** file, which is the full documentation.

**FUNCTIONALITIES:**

   1) **Student**
      - Login
      - Logout
      - Delete Account
      - Edit Profile
      - Received Grades
      - Register
      - Account details
      - Search and See other users
      - Can see the absences (TBI)
      - Notifs (TBI)


   2) **Teacher**
      - Login
      - Logout
      - Delete Account
      - Edit Profile
      - Grades Students
      - Register
      - Account details
      - Search and See other users
      - Can put the absences (TBI)
      - Notifs (TBI)

   3) **Anonymus User**

      - Search and See the registered users and their details (personal details are hidden)
      
   4) **Admin**

      - Login
      - Adds Subjects
      - Deletes Subjects
      - Remove Users (Students / Teachers)


## Use Case Diagram

<img width="1351" height="678" alt="image" src="https://github.com/user-attachments/assets/38517e84-d1f2-4946-a1af-52b82865bd19" />

**Other details**

-> this app uses 2 backends and 1 frontend : **2 x API** and **1 x React Frontend App**.



