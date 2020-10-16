
#r "nuget: Akka.FSharp"
#load "gossipLine.fs"
#load "gossipFull.fs"
#load "gossip2D.fs"
#load "gossipImp2D.fs"
#load "pushSumFull.fs"
#load "pushSumLine.fs"
#load "pushSum2D.fs"
#load "pushSumImp2D.fs"

(*##################
reading command line inputs
##################*)
let args : string array = fsi.CommandLineArgs |> Array.tail
let numNodes = args.[0] |> int
let topology = args.[1]
let algorithm = args.[2]


match algorithm with
| "gossip" -> match topology with
              |"line" -> GossipLine.createWorker(numNodes)
              |"full" -> GossipFull.createWorker(numNodes)
              |"2D" -> Gossip2D.createWorker(numNodes)
              |"imp2D" -> GossipImp2D.createWorker(numNodes)
              |_ -> printfn "do nothing"
              
| "push-sum" -> match topology with
                |"full" -> PushSumFull.createWorker(numNodes)
                |"imp2D" -> PushSumImp2D.createWorker(numNodes)
                |"2D" -> PushSum2D.createWorker(numNodes) 
                |"line" -> PushSumLine.createWorker(numNodes)
                |_ -> printfn "do nothing"
|_ -> printfn "do nothing"
              