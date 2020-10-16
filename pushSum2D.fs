module PushSum2D
open System
open Akka.FSharp
open Akka.Actor
let system = System.create "system" (Configuration.defaultConfig())
open System.Collections.Generic
let mutable index = 1
let mutable actorArray = [||]
let mutable actorNum = 0
open System.Diagnostics

let mutable set1 = HashSet<int>()
let mutable set2= HashSet<int>()
type GossipMsg = GossipMsg of float * float *int
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
    let nbr = getRandNode list
    nbr 

(*#############################
function to track the node
convergence
###############################*)
let SetInserter (mailbox:Actor<_>)=
    let rec loop()=actor{
        let! (idx,s,w) = mailbox.Receive()
        (*printfn "%i" idx*)
        if(set1.Count<>actorNum)then
            set1.Add(idx) |>ignore
            if(set1.Count=actorNum)then
                printfn "s = %f, w = %f" s w
               // printfn "%f" s
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
    let mutable S=mailbox.Context.Self.Path.Name|>float
    let mutable W=1.0
    let rec loop(S,W,count) = actor {
        //printfn "%f , %f , %f" S W (S/W)
        let! GossipMsg(s,w,idx) = mailbox.Receive()
        let tempS=S+s
        let tempW=W+w
        let mutable f=false
        let previous = float(S)/float(W)
        let current = float(tempS)/float(tempW) 
        if((abs(current - previous)) < (float(10) ** float(-10))) then
            f<-true
        let mutable ran=calcNeighbor(idx)
        actorArray.[ran]<!GossipMsg(float(tempS)/float(2),float(tempW)/float(2),ran)
        if(f)then
            if(count+1=3)then
                setInserterActor<!(idx,(float(tempS)/float(2)),(float(tempW)/float(2)))
            return! loop(tempS-(float(tempS)/float(2)),tempW-(float(tempW)/float(2)),count+1)
        else        
            return! loop(tempS-(float(tempS)/float(2)),tempW-(float(tempW)/float(2)),0)
    }
    loop(S,W,0)


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
      baseActor<!GossipMsg(0.0,0.0,ran)
      let timer = new Stopwatch()
      timer.Start()
      let mutable x=0
      while flag do
          x<-0
      timer.Stop()
      printfn "elapsed %d ms" timer.ElapsedMilliseconds
      