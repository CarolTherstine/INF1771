using MathNet.Numerics;
using System.Reflection;
using System.Text;
using T1.DataTypes;
using T1.Helpers;
using T1_AA.DataTypes;
const int x = 82;
List<string> mapa = new List<string>();
montaMapa();
var Characthers = Enumeration.GetAll<Characther>().ToList();
var Terrenos = Enumeration.GetAll<TipoTerreno>();
var Etapas = Enumeration.GetAll<Etapa>();
var resultados = new List<float>();
//montaCaminho('0', '1');
escolheMelhorPersonagens();
/*foreach (var etapa in Etapas)
{
	var custo = 0.0f;
	var dif = etapa.dificuldade;
	var totalAgilidade = 0.0f;
	foreach (var personagem in Characthers)
    {
		if (personagem.energia > 0)
        {
			personagem.DiminuiEnergia(1);
			totalAgilidade += personagem.agility;
        }
		custo = dif / totalAgilidade;
    }
	//Inicio da jornada sempre comeca no 1
	montaCaminho('1', etapa.identificador);
	//Jornada sempre termina no i
	montaCaminho(etapa.identificador, 'I');
}*/
void montaMapa()
{
    string text;
    string filepath = @"C:\Users\CarolinaBrandao\source\repos\T1\T1\mapa.txt";
    var fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
    {
        String line;
            while ((line = streamReader.ReadLine()) != null)
        {
            mapa.Add(line);
        }
    }
}
void escolheMelhorPersonagens()
{
	List<List<OrdenacaoPersonagens>> ListasDePersonagens = new List<List<OrdenacaoPersonagens>>();
	List<OrdenacaoPersonagens> ListaTotal = new List<OrdenacaoPersonagens>();
	List<Characther> personagensCansados = new List<Characther>();
	for (var i =4; i >= 2; i--)
    {
		var temp = MontaListaDeOrnacao(i);
		ListasDePersonagens.Add(temp);
		ListaTotal.AddRange(temp);
    }
	var ListaTotalCopy = ListaTotal.OrderByDescending(c => c.somaAgilidade);
	foreach (var item in ListaTotalCopy)
	{
		if (item.personagens.Intersect(personagensCansados).Any())
		{
			ListaTotal.Remove(item);
		}
		else
        {

		foreach (var personagem in item.personagens)
		{
			if (personagem.energia >= 2)
			{
				personagem.DiminuiEnergia(1);
			}
			else if (personagem.energia == 1)
			{
				personagensCansados.Add(personagem);
			}
        }
		}
	}
	foreach (var item in ListaTotal)
    {
		foreach (var personagem in item.personagens)
        {
			Console.Write(personagem + ", ");
        }
		Console.Write(item.somaAgilidade);
		Console.WriteLine();
    }
}

List<OrdenacaoPersonagens> MontaListaDeOrnacao(int n)
{
    List<Characther> data = new List<Characther>();
	for (var i =0; i < n; i++)
    {
		data.Add(new Characther());
    }
	List<OrdenacaoPersonagens> ordenacaoPersonagens = new List<OrdenacaoPersonagens>();
	combinationUtil(Characthers, data, 0, Characthers.Count() -1, 0, n, ref ordenacaoPersonagens);
	ordenacaoPersonagens.OrderBy(c => c.somaAgilidade);
	return ordenacaoPersonagens;
}

static void combinationUtil(List<Characther> arr, List<Characther> data,
								int inicio, int fim,
								int index, int r, ref List<OrdenacaoPersonagens> ordenacaoPersonagens)
{
	if (index == r)
	{
		for (int j = 0; j < r; j++)
        {
			Console.Write(data[j] + " ");
        }
			OrdenacaoPersonagens temp = new OrdenacaoPersonagens
			{
				personagens = data,
				somaAgilidade = data.Sum(c => c.agility)
			};
			ordenacaoPersonagens.Add(temp);
		Console.WriteLine("");
		return;
	}
	for (int i = inicio; i <= fim &&
			  fim - i + 1 >= r - index; i++)
	{
		data[index] = arr[i];
		combinationUtil(arr, data, i + 1,
						fim, index + 1, r, ref ordenacaoPersonagens);
	}
}
void mostraMapa(Tile atual)
{
	var tile = atual;
	var tempo = 0;
//	Console.WriteLine("Retracing steps backwards...");
	while (true)
	{
		Console.WriteLine($"{tile.X} : {tile.Y}");
		//			if (mapa[tile.Y][tile.X] == ' ')
		//		{
		tempo += PegaCusto(tile.X, tile.Y);
		var newMapRow = mapa[tile.Y].ToCharArray();
		newMapRow[tile.X] = '*';
		mapa[tile.Y] = new string(newMapRow);
		//	}
		tile = tile.Parent;
		if (tile == null)
		{
			Console.WriteLine("Mapa final:");
			mapa.ForEach(x => Console.WriteLine(x));
			Console.WriteLine("Done!");
			Console.WriteLine($"Custo total de tempo foi : {tempo}");
			return;
		}
	}
}
int PegaCusto(int x,int y)
{
	foreach (var terreno in Terrenos)
    {
		if (string.Equals(mapa[y][x], terreno.letraRepresentante))
        {
			return terreno.tempo;
        }
    }
	return 0;
}
void montaCaminho(char inicial, char final)
{
	var inicio = new Tile();
	inicio.Y = mapa.FindIndex(x => x.Contains(inicial));
	inicio.X = mapa[inicio.Y].IndexOf(inicial);
	var fim = new Tile();
	fim.Y = mapa.FindIndex(x => x.Contains(final));
	fim.X = mapa[fim.Y].IndexOf(final);

	inicio.SetDistance(fim.X, fim.Y);
	var naoVisitados = new List<Tile>();
	naoVisitados.Add(inicio);
	var visitados = new List<Tile>();
	while (naoVisitados.Any())
	{
		var atual = naoVisitados.OrderBy(x => x.CostDistance).First();
		if (atual.X == fim.X && atual.Y == fim.Y)
		{
			mostraMapa(atual);	
		}
		visitados.Add(atual);
		naoVisitados.Remove(atual);
		var andaveis = MostraPossiveisTiles(mapa, atual, fim);
		foreach (var atualAndavel in andaveis)
		{
			if (visitados.Any(x => x.X == atualAndavel.X && x.Y == atualAndavel.Y))
				continue;
			if (naoVisitados.Any(x => x.X == atualAndavel.X && x.Y == atualAndavel.Y))
			{
				var existingTile = naoVisitados.First(x => x.X == atualAndavel.X && x.Y == atualAndavel.Y);
				if (existingTile.CostDistance > atual.CostDistance)
				{
					naoVisitados.Remove(existingTile);
					naoVisitados.Add(atualAndavel);
				}
			}
			else
			{
				//We've never seen this tile before so add it to the list. 
				naoVisitados.Add(atualAndavel);
			}
		}
	}
	Console.WriteLine("Erro, sem caminho");
}
static List<Tile> MostraPossiveisTiles(List<string> mapa, Tile atual, Tile proximo)
{
	var TilesPossiveis = new List<Tile>()
	{
		new Tile { X = atual.X, Y = atual.Y - 1, Parent = atual, Cost = atual.Cost + 1 },
		new Tile { X = atual.X, Y = atual.Y + 1, Parent = atual, Cost = atual.Cost + 1},
		new Tile { X = atual.X - 1, Y = atual.Y, Parent = atual, Cost = atual.Cost + 1 },
		new Tile { X = atual.X + 1, Y = atual.Y, Parent = atual, Cost = atual.Cost + 1 },
	};
	TilesPossiveis.ForEach(tile => tile.SetDistance(proximo.X, proximo.Y));
	var maxX = mapa.First().Length - 1;
	var maxY = mapa.Count - 1;
	return TilesPossiveis
			.Where(tile => tile.X >= 0 && tile.X <= maxX)
			.Where(tile => tile.Y >= 0 && tile.Y <= maxY)
			.ToList();
}