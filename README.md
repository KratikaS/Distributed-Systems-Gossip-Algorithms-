# DOSProject2Gossip
Team members
1. Kratika Singhal, UFID: 69535971, singhalk@ufl.edu
2. Shrishail Zalake, UFID: 07466343, shrishailzalake@ufl.edu

The aim of this project is to determine the convergence gossip algorithms through a simulator based on actors written in F#. We have implemented gossip and push-sum algorithm for line, full, 2D and imperfect 2D network. In our implementation, we are sending message to a random neighbor in each topology and stopping the code when convergence is achieved.

•	We were able to achieve 100% convergence for Gossip for all the four network topologies (line, full, 2D grid, imperfect 2D grid). 

•	Convergence in gossip algorithm is achieved when the spread is 100%. Spread is concluded as 100 percent when all the nodes have been infected with the gossip at least once.

•	We were able to achieve 100% convergence for push-sum algorithm for all the four topologies. 

•	Convergence in push-sum algorithm is achieved when the s/w ratio for all the actors do not change more than 10-10 in three consecutive rounds. 
