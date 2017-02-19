open System.Text
open System.IO

type Html =
    | Text of string
    | Attr of string * string
    | Tag of string * Html list * Html list

let HtmlToString (html:Html) : string =
    let stringBuilder = new StringBuilder()

    let rec toString (html:Html) (indent:int) : unit =
        let spaces = String.replicate indent " "
        let newLine = System.Environment.NewLine
        match html with
            | Text(txt) -> stringBuilder.Append(txt) |> ignore
            | Attr(name, value) -> stringBuilder.Append(sprintf " %s=\"%s\"" name value) |> ignore
            | Tag(tag, [], [Text s]) ->
                stringBuilder.Append(sprintf "%s<%s>%s</%s>%s" spaces tag s tag newLine) |> ignore
            | Tag(tag, attrs, []) ->
                stringBuilder.Append(sprintf "%s<%s" spaces tag) |> ignore
                for attr in attrs do
                    toString attr indent
                stringBuilder.Append(sprintf "/>%s" newLine) |> ignore
                ()
            | Tag(tag, attrs, tags) ->
                stringBuilder.Append(sprintf "%s<%s" spaces tag) |> ignore
                for attr in attrs do
                    toString attr indent
                stringBuilder.Append(sprintf ">%s" newLine) |> ignore
                for tag in tags do
                    toString tag (indent + 4)
                stringBuilder.Append(sprintf "%s</%s>%s" spaces tag newLine) |> ignore
                ()
            
    do toString html 0
    stringBuilder.ToString()

Tag("html", 
    [Attr("xmlns", "http://www.w3.org/1999/xhtm")], 
    [
        Tag("head", 
            [], 
            [
                Tag("title", 
                    [], 
                    [Text("GDNUG Talk")])
            ])
        Tag("body", 
            [], 
            [
                Tag("h1", 
                    [], 
                    [Text("Hello world!")])
                Tag("form", 
                    [Attr("id", "form1")],
                    [Tag("input", 
                        [
                        Attr("type", "text")
                        Attr("name", "name")], 
                        [])
                    ])
            ])
    ])
|> HtmlToString |> printfn "%A"

let tag name attrs elems = Tag(name, attrs, elems)
let (%=) name value = Attr(name,value)
let (~%) s = [Text(s.ToString())]

tag "html" 
    ["xmlns"%="http://www.w3.org/1999/xhtm"] 
    [
        tag "head" 
            [] 
            [
                tag "title" 
                    [] 
                    %("GDNUG Talk")
            ]
        tag "body" 
            [] 
            [
                tag "h1" 
                    [] 
                    %("Hello world!")
                tag "form" 
                    ["id"%="form1"]
                    [tag "input" 
                        [
                        "type"%="text"
                        "name"%="name"] 
                        []
                    ]
            ]
    ]
|> HtmlToString  |> printfn "%A"

let html attrs elems = tag "html" attrs elems
let head attrs elems = tag "head" attrs elems
let title attrs elems = tag "title" attrs elems
let body attrs elems = tag "body" attrs elems
let h1 attrs elems = tag "h1" attrs elems
let form attrs elems = tag "form" attrs elems  
let input attrs elems = tag "input" attrs elems  

html 
    ["xmlns"%="http://www.w3.org/1999/xhtm"] 
    [
        head 
            [] 
            [
                title 
                    [] 
                    %("GDNUG Talk")
            ]
        body 
            [] 
            [
                h1 
                    [] 
                    %("Hello world!")
                form 
                    ["id"%="form1"]
                    [input 
                        [
                        "type"%="text"
                        "name"%="name"] 
                        []
                    ]
            ]
    ]
|> HtmlToString  |> printfn "%A"


let ShowHtmlInBrowser (html:string) =
    let name = System.Guid.NewGuid().ToString()
    let path = System.IO.Path.GetTempPath() + name + ".html"
    use writer = File.CreateText(path)
    writer.Write(html)
    writer.Close()
    System.Diagnostics.Process.Start(path) |> ignore

let ul attrs elems = tag "ul" attrs elems
let li attrs elems = tag "li" attrs elems
    
let timesTable n =
  html []
    [
      head [] [title [] %(sprintf "%d Times table" n)]
      body [] 
        [
          h1 [] %(sprintf "%d Times table" n)
          ul [] 
            [
              for i in 1..12 -> 
                li [] 
                  %(sprintf "%d x %d = %d" n i (n*i))]
        ]
    ]
timesTable 17 |> HtmlToString |> ShowHtmlInBrowser

let table attrs elems = tag "table" attrs elems 
let thead attrs elems = tag "thead" attrs elems 
let tbody attrs elems = tag "tbody" attrs elems
let th attrs elems = tag "th" attrs elems 
let tr attrs elems = tag "tr" attrs elems 
let td attrs elems = tag "td" attrs elems

html
    []
    [
        head 
            []
            [title [] %"Times table"]
        body 
            [] 
            [
                h1 [] %"Times table"
                table 
                    [
                        "border"%="1"
                        "style"%="border-collapse:collapse;"
                        "cellpadding"%="3"
                    ]
                    [ 
                    thead 
                        [] 
                        [
                        tr 
                            [] 
                            (th [] %""::
                                [for i in 1..20 -> 
                                    th ["style"%="text-align:right;"] %i])
                        ]
                    tbody 
                        [] 
                        [
                        for i in 1..20 ->
                            tr [] (th ["style"%="text-align:right;"] %i::
                                [for j in 1..20 ->
                                    td ["style"%="text-align:right;"] %(i*j)])
                        ]
                    ]
                ]
    ]
|> HtmlToString |> ShowHtmlInBrowser

