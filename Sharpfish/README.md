# Sharpfish
Interact with UCI Chess Engines, in this case Stockfish, through a wrapper library which provides intuitive methods.

## Usage examples

#### Creating a new Sharpfish instance.
```cs
static void Main(string[] args) {
	IStockfishEngine sharpfish = new Sharpfish.StockfishEngine(@"path\to\stockfish")
}
```

#### Getting output
```cs
await sharpfish.setFenPosition("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
string bestMove = await sharpfish.GetBestMove(); // e2e4
```
#### 

