module GossipImp2D
open System
open Akka.FSharp
open Akka.Actor
let system = System.create "system" (Configuration.defaultConfig())
open System.Collections.Generic
open System.Diagnostics

let mutable index = 1
let mutable actorArray = [||]
let mutable actorNum = 0
let mutable set1 = HashSet<int>()
let mutable set2= HashSet<int>()
type GossipMsg = GossipMsg of string *int
let mutable flag=true
let mutable msgReachedCount=0

(*#############################
function to select a randome node
from the array
###############################*)
let getRandNode (arr:List<int>) =
  let ramdom = Random()
  let ran=(ramdom.Next(0,arr.Count))
  arr.Item(ran)

(*#############################
function to select a calculate 
a random neighbour
###############################*)
let calcNeighbor idx:int =
     let list = new List<int>()
     let nodeSqrt = actorNum |>double |> sqrt |>int
     if (idx + nodeSqrt)<actorNum then 
        let nbr1 = idx + nodeSqrt
        list.Add(nbr1)        
     if (idx - nodeSqrt)>=0 then
        let nbr2 = idx - nodeSqrt
        list.Add(nbr2)  
     if ((idx + 1)<actorNum) then
        let nbr3 = idx + 1
        list.Add(nbr3)     
     if (idx - 1)>=0 then
        let nbr4 = idx - 1  
        list.Add(nbr4)
     let nextRandom = new Random()
     let mutable nbr5 = nextRandom.Next(0,actorNum) 
     while(not(list.Contains(nbr5)))do
           nbr5 <- nextRandom.Next(0,actorNum)
           list.Add(nbr5)
     let nbr = getRandNode list
     nbr 

(*#############################
function to track the node
convergence
###############################*)
let SetInserter (mailbox:Actor<_>)=
    let rec loop()=actor{
        let! idx = mailbox.Receive()
        if(set1.Count<>actorNum)then
            set1.Add(idx) |>ignore
            if(set1.Count=actorNum)then
                printfn "converged"
                flag<-false
        return! loop()
    }
    loop()

let setInserterActor=spawn system "inserter" SetInserter

(*#############################
worker actor defination
###############################*)
let workeractor (mailbox: Actor<_>)=
    let rec loop(state) = actor {
        let! m = mailbox.Receive()
        match m with
            |GossipMsg(msg,idx)->
                if(state<(actorNum*5)) then
                                      
                    let mutable ran=calcNeighbor(idx)
                    actorArray.[ran]<!GossipMsg("hello",ran)
                    setInserterActor<!ran
            
        return! loop(state+1)
    }
    loop(0)

(*#############################
function to generate worker actors 
actors = number of nodes
###############################*)
let createWorker(numNodes) = 
      let num=numNodes|>double |> sqrt |>int
      actorNum <- num*num 
      actorArray <- [| for i in 0 .. actorNum-1 -> spawn system (string i) workeractor|]
      let NodeRandom = new Random()
      let mutable ran = NodeRandom.Next(0,actorNum)
      let baseActor=actorArray.[ran]
      set1.Add(ran)|>ignore
      baseActor<!GossipMsg("hello",ran)
      let timer = new Stopwatch()
      timer.Start()
      let mutable x=0
      while flag do
          x<-0
      timer.Stop()
      printfn "elapsed %d ms" timer.ElapsedMilliseconds
      
