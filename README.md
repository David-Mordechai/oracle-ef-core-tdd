# oracle-ef-core-tdd

## How to configure docker with oracle container 

download docker and install
open powershell and in powershell type this commands

### 1. Log into Docker hub (in order to access oracle repository)
 docker login

### 2. Download image
 docker pull store/oracle/database-enterprise:12.2.0.1

### 3. Run image
 docker run -d -p 1521:1521 --name oracle store/oracle/database-enterprise:12.2.0.1

### 4. Connect to container
 docker exec -it oracle bash -c "source /home/oracle/.bashrc; sqlplus /nolog"

### 5. Copy below script to open SQL shell, run this commands one by one

 # connect sys as sysdba;
 # -- Here enter the password as 'Oradoc_db1'
 # alter session set "_ORACLE_SCRIPT"=true;
 # create user ora_doc_owner identified by ora_doc_owner;
 # GRANT ALL PRIVILEGES TO ora_doc_owner;

### 6. How to run/stop oracle container and how to see witch containers we have
  6.1 docker container ls -1 
  6.2 docker start oracle
  6.3 docker stop oracle

### 7. Configure SQL Developer

 Username: ora_doc_owner
 Password: ora_doc_owner
 Hostname: localhost
 Port: 1521
 Service name: ORCLCDB.localdomain
