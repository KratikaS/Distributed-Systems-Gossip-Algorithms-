module GossipLine
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
let getRandNode =
  let ramdom = Random()
  fun (arr : int[]) -> arr.[ramdom.Next(arr.Length)]

(*#############################
function to select a calculate 
a random neighbour
###############################*)
let calcNeighbor idx:int =
     let mutable nbr = 0
     if idx = 0 then nbr <- idx+1
     elif idx = (actorNum-1) then nbr <- idx-1
     else 
         let arr = [|idx-1; idx+1|]
         nbr <- getRandNode arr
     nbr

(*#############################
function to track the node
convergence
###############################*)
let SetInserter (mailbox:Actor<_>)=
    let rec loop()=actor{
        let! idx = mailbox.Receive()
        //printfn "%i" set1.Count
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
      actorNum <- numNodes 
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
