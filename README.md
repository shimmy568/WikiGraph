# WikiGraph
A program that maps Wikipedia for using a web crawler or sorts, then graphs the data and stuff I guess

The actual web crawler is pretty much done. I just have to run it and collect some data and then play around with said data. 

Going to add nice images and stuff here later.

# Interface

The interface for this program is done through the cli.

## Data collection
> dotnet run 1

## Data processing
> dotnet run 2 [type]

Will list the types of processing possible if no type is given.

## Data repair
> dotnet run 3

Will repair the data by looking for edges that point to nodes that don't exsit and look through
the html of all the nodes for urls that haven't been searched. Will add urls that failed to load as well