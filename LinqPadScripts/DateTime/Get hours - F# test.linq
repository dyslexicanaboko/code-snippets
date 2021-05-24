<Query Kind="FSharpProgram" />

type TimeBlock = {Start:string; End:string}

let GetHours (date:DateTime) (blocks:List<TimeBlock>) : double =
    let dblHours = 0
    let strDate = date.ToString("yyyy-MM-dd")
    let dtmStart = DateTime.Now
    let dtmEnd = DateTime.Now
    
    //Have to figure out how to perform a loop
    for tb in blocks do
        dtmStart = Convert.ToDateTime(strDate + " " + tb.Start)
        dtmEnd = Convert.ToDateTime(strDate + " " + tb.End)
        dblHours = dblHours + (dtmEnd - dtmStart).TotalHours
    
    dblHours

//Should be 1 hour
let t = {Start="10:00"; End="11:00"}

//List of one element of TimeBlock
let lst : TimeBlock list = [t]

//Operate on that list
let x = GetHours DateTime.Today lst

x.Dump();