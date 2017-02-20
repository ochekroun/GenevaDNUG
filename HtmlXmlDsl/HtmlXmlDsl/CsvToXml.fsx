//#load @"C:\Work\HtmlDsl\packages\FsLab.1.0.2\FsLab.fsx"
#load @"..\packages\FsLab.1.0.2\FsLab.fsx"

open FSharp.Data
open System.IO
open System.Diagnostics
open System.Text

type Xml =
    | Text of string
    | Attr of string * string
    | Elem of string * Xml list * Xml list

let XmlToString xml : string =
    let stringBuilder = new StringBuilder()

    let rec toString xml indent : unit =
        let spaces = String.replicate indent " "
        let newLine = System.Environment.NewLine
        match xml with
            | Text(txt) -> stringBuilder.Append(txt) |> ignore
            | Attr(name, value) -> stringBuilder.Append(sprintf " %s=\"%s\"" name value) |> ignore
            | Elem(tag, attrs, [Text s]) ->
                stringBuilder.Append(sprintf "%s<%s" spaces tag) |> ignore
                for attr in attrs do
                    toString attr indent
                stringBuilder.Append(sprintf ">%s</%s>%s" s tag newLine) |> ignore
            | Elem(tag, attrs, []) ->
                stringBuilder.Append(sprintf "%s<%s" spaces tag) |> ignore
                for attr in attrs do
                    toString attr indent
                stringBuilder.Append(sprintf "/>%s" newLine) |> ignore
                ()
            | Elem(tag, attrs, elems) ->
                stringBuilder.Append(sprintf "%s<%s" spaces tag) |> ignore
                for attr in attrs do
                    toString attr indent
                stringBuilder.Append(sprintf ">%s" newLine) |> ignore
                for elem in elems do
                    toString elem (indent + 1)
                stringBuilder.Append(sprintf "%s</%s>%s" spaces tag newLine) |> ignore
                ()
            
    do toString xml 0
    stringBuilder.ToString()

let elem name attrs elems = Elem(name, attrs, elems)
let (~%) s = [Text(s.ToString())]
let (%=) name value = Attr(name,value)
let xml = elem "xml"
let value = elem "value"
let date value = Attr("Date", value)
let high value = Attr("High", value)

let ShowXml xml =
    let name = System.Guid.NewGuid().ToString()
    let path = System.IO.Path.GetTempPath() + name + ".xml"
    File.WriteAllText(path, xml)
    Process.Start(path)


//type Stocks = CsvProvider<"http://ichart.finance.yahoo.com/table.csv?s=MSFT">
//let msft = Stocks.Load("http://ichart.finance.yahoo.com/table.csv?s=MSFT")
//msft.Save(filePath)

[<Literal>]
let filePath = __SOURCE_DIRECTORY__+ @"\msft.csv"
type Stocks = CsvProvider<filePath>
let msft = Stocks.Load(filePath)

xml []
  [ 
    for row in msft.Rows ->
      value 
        [
           date (row.Date.ToString("yyyyMMdd"))
           high (string(row.High.ToString("N2")))
        ]
        %(string(row.Close.ToString("N2")))
  ]
|> XmlToString |> ShowXml
