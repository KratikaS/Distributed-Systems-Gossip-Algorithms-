# DOSProject2Gossip
Team members
1. Kratika Singhal, UFID: 69535971, singhalk@ufl.edu
2. Shrishail Zalake, UFID: 07466343, shrishailzalake@ufl.edu

What is working
•	We were able to achieve 100% convergence for Gossip for all the four network topologies (line, full, 2D grid, imperfect 2D grid). 
•	Convergence in gossip algorithm is achieved when the spread is 100%. Spread is concluded as 100 percent when all the nodes have been infected with the gossip at least once
•	We were able to achieve 100% convergence for push-sum algorithm for all the four topologies. 
•	Convergence in push-sum algorithm is achieved when the s/w ratio for all the actors do not change more than 10-10 in three consecutive rounds. 

Largest Network achieved
Gossip algorithm
•	Line network: 10000 number of nodes
•	Full network: 100000 number of nodes
•	2D grid network: 100000 number of nodes
•	Imperfect 2D grid network: 100000 number of nodes
Push-Sum algorithm
•	Line network: 500 number of nodes
•	Full network: 100000 number of nodes
•	2D grid network: 1000 number of nodes
•	Imperfect 2D grid network: 10000 number of nodes
Instructions to run
We are executing the code through command line interface using the command 
dotnet fsi --langversion:preview program.fsx <numNodes> <topology> <algorithm>

