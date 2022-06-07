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
var Etapas = Enumeration.GetAll<Etapa>().ToList();
Etapas.Remove(Etapa.Etapa0);
var resultados = new List<float>();
List<Tile> tilesDosCaminhos = new List<Tile>();
//montaCaminho('0', '1');
//escolheMelhorPersonagens();
/*foreach (var etapa in Etapas)
{
	var custo = 0.0f;
	var dif = etapa.dificuldade;
	var totalAgilidade = 0.0f;
	/*foreach (var personagem in Characthers)
    {
		if (personagem.energia > 0)
        {
			personagem.DiminuiEnergia(1);
			totalAgilidade += personagem.agility;
        }
		custo = dif / totalAgilidade;
    }*/
//Inicio da jornada sempre comeca no 1
/*
	Console.WriteLine("xinXILAS GORDINHAS E FOFAS");
	Console.WriteLine(etapa.identificador);
	tilesDosCaminhos.Add(montaCaminho('0', etapa.identificador));
	//Jornada sempre termina no i
	montaCaminho(etapa.identificador, 'I');
}*/
Console.WriteLine("Sexo");
SimulatedAnnealing();


void montaMapa()
{
    string text;
    string filepath = @"C:\Users\Carol\Source\Repos\INF1771\T1\mapa.txt";
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
Tile montaCaminho(char inicial, char final)
{
	var inicio = new Tile();
	inicio.Y = mapa.FindIndex(x => x.Contains(inicial));
	inicio.X = mapa[inicio.Y].IndexOf(inicial);
	//inicio.Cost = Terrenos.Where(c => c.letraRepresentante.Equals(mapa[mapa[inicio.Y].IndexOf(inicial)][mapa.FindIndex(x => x.Contains(inicial))])).FirstOrDefault().tempo;
	var fim = new Tile();
	fim.Y = mapa.FindIndex(x => x.Contains(final));
	fim.X = mapa[fim.Y].IndexOf(final);
	//fim.Cost = Terrenos.Where(c => c.letraRepresentante.Equals(mapa[mapa[mapa[fim.Y].IndexOf(final)][mapa.FindIndex(x => x.Contains(final))])).FirstOrDefault().tempo;

	inicio.SetDistance(fim.X, fim.Y);
	var naoVisitados = new List<Tile>();
	naoVisitados.Add(inicio);
	var visitados = new List<Tile>();
	while (naoVisitados.Any())
	{
		var atual = naoVisitados.OrderBy(x => x.CostDistance).First();
		if (atual.X == fim.X && atual.Y == fim.Y)
		{
			return atual;
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
				naoVisitados.Add(atualAndavel);
			}
		}
	}
	Console.WriteLine("Erro, sem caminho");
	return null;
}
List<Tile> MostraPossiveisTiles(List<string> mapa, Tile atual, Tile proximo)
{
	var TilesPossiveis = new List<Tile>()
	{
		new Tile { X = atual.X, Y = atual.Y - 1, Parent = atual, Cost = 0},
		new Tile { X = atual.X, Y = atual.Y + 1, Parent = atual, Cost = 0},
		new Tile { X = atual.X - 1, Y = atual.Y, Parent = atual, Cost = 0},
		new Tile { X = atual.X + 1, Y = atual.Y, Parent = atual, Cost = 0},
	};
	TilesPossiveis.ForEach(tile => tile.SetDistance(proximo.X, proximo.Y));
	var maxX = mapa.First().Length - 1;
	var maxY = mapa.Count - 1;
	var tilespossiveis =  TilesPossiveis
			.Where(tile => tile.X >= 0 && tile.X <= maxX)
			.Where(tile => tile.Y >= 0 && tile.Y <= maxY)
			.ToList();
	tilespossiveis.ForEach(tile => tile.Cost = RetornaPreco(tile.X,tile.Y));
	return tilespossiveis;
}

int RetornaPreco(int x, int y)
{
	var letraRepresentante = mapa[y][x];
	foreach (var terreno in Terrenos)
    {
		if (terreno.letraRepresentante.Equals(letraRepresentante))
        {
			return terreno.tempo;
        }
    }
	return 0;
}

#region Escolha_De_Grupos
void SimulatedAnnealing()
{
	var escalonamento = new List<float>();
	for (var i = 1; i < 2000; i++)
    {
		escalonamento.Add((float)(1 /Math.Pow(i,2)));
    }
	var inicial = Inicia();
	var solucaoFinalParaTimes = Escalonacao(inicial, escalonamento);
	var tempoFinal = SomaTotalTempo(solucaoFinalParaTimes);
	Console.WriteLine($"O TEMPO FINAL 1 FOI {tempoFinal}");
}


Characther SorteiaTime(Etapa etapa, List<Characther> personagens, List<Characther> time)
{
	var rand = new Random();
	var iteration = 0;
	var personagem = rand.Next(0, 6);
	while (personagens[personagem].energia == 0 || time.Contains(personagens[personagem]))
    {
		personagem = rand.Next(0, 6);
		iteration++;
		if (iteration > 16)
        {
			break;
        }
		
    }
	return personagens[personagem];
}

List<KeyValuePair<Etapa, List<Characther>>> Inicia()
{
	List<KeyValuePair<Etapa, List<Characther>>> ordenacaoFinal = new List<KeyValuePair<Etapa, List<Characther>>>();
	List<Characther> personagens = Enumeration.GetAll<Characther>().ToList();
	foreach (var etapa in Etapas)
    {
		List<Characther> personagensSortidos = new List<Characther>();
		for (var i = 0; i < 2; i++)
        {
			var sorteio = SorteiaTime(etapa, personagens, personagensSortidos);
			if (sorteio.energia > 0)
            {
				sorteio.DiminuiEnergia(1);
				personagensSortidos.Add(sorteio);
            }
			
        }
		ordenacaoFinal.Add(new KeyValuePair<Etapa,List<Characther>>(etapa, personagensSortidos));
    }
	return ordenacaoFinal;
}
List<KeyValuePair<Etapa, List<Characther>>> InverteUmPersonagem(List<KeyValuePair<Etapa, List<Characther>>> personagens)
{
	for (var i =0; i <300; i++) 
	{ 
		var random1 = new Random();
		var random2 = new Random();
		var random3 = new Random();
		var indice1 = random1.Next(0, 1);
		var indice2 = random2.Next(0, 1);
		var indice3 = random2.Next(0, 1);
		var troca1 = personagens[indice1];
		var personagemTirar1 = personagens[indice1].Value[indice3];
		var troca2 = personagens[indice2];
		var personagemTirar2 = personagens[indice2].Value[indice3];
		if (!troca2.Value.Contains(personagemTirar1) && !troca1.Value.Contains(personagemTirar2))
		{
			List<Characther> aux = new List<Characther>();
			foreach (var personagen1 in troca1.Value)
			{
				aux.Add(personagen1);
			}
			troca1.Value.Clear();
			foreach (var personagem2 in troca2.Value)
			{
				troca1.Value.Add(personagem2);
			}
			troca1.Value.Clear();
			foreach (var final in aux)
			{
				troca2.Value.Add(final);
			}
			break;
		}
	}
	return personagens;
}
List<KeyValuePair<Etapa,List<Characther>>> InvertePersonagens (List<KeyValuePair<Etapa, List<Characther>>> personagens)
{
	for (var i = 0; i < 300; i++)
	{
		var random1 = new Random();
		var random2 = new Random();
		var indice1 = random1.Next(0, 1);
		var indice2 = random2.Next(0, 1);
		var troca1 = personagens[indice1];
		var troca2 = personagens[indice2];

		if (!troca1.Value.Intersect(troca2.Value).Any())
        {
			List<Characther> aux = new List<Characther>();
			foreach (var personagen1 in troca1.Value)
			{
				aux.Add(personagen1);
			}
			troca1.Value.Clear();
			foreach (var personagem2 in troca2.Value)
			{
				troca1.Value.Add(personagem2);
			}
			troca2.Value.Clear();
			foreach (var final in aux)
			{
				troca2.Value.Add(final);
			}
			break;
		}
    }
	return personagens;
}
	
List<KeyValuePair<Etapa, List<Characther>>> Disturb(List<KeyValuePair<Etapa, List<Characther>>> atual)
{
	var novo = atual;
	for (var i = 0; i < 1000; i++)
    {
		if (i.IsEven())
        {
			novo = InvertePersonagens(atual);
        }
		else
        {
			novo = InverteUmPersonagem(atual);
        }
		if (SomaTotalTempo(novo) < SomaTotalTempo(atual))
        {
			atual = novo;
        }
    }
	return atual;
}

List<KeyValuePair<Etapa, List<Characther>>> Escalonacao (List<KeyValuePair<Etapa, List<Characther>>> equipes, List<float> escalonamento)
{
	var atual = equipes;
	var melhorSolucao = equipes; 
	for (var i =0;i < escalonamento.Count;i++)
    {
		var t = escalonamento[i];
		if (t == 0)
        {
			return atual;
        }
		var rand = new Random();
		//pertubcao
		var proximo = new List<KeyValuePair<Etapa, List<Characther>>>();
		proximo = Disturb(atual);
		var tempoAtual = SomaTotalTempo(equipes);
		var tempoProximo = SomaTotalTempo(proximo);
		var tempoNovo = tempoProximo - tempoAtual;
		if (tempoNovo < 0)
		{
			atual = proximo;
			melhorSolucao = proximo;
		}
		else if (rand.NextDouble() < Math.Exp((-1 * tempoNovo)/t))
        {
			atual = proximo;
        }
    }
	return melhorSolucao;
}

float SomaTotalTempo (List<KeyValuePair<Etapa, List<Characther>>> equipes)
{
	var tempoTotal = 0.0f;
	foreach (var equipe in equipes)
    {
		var agilidades = somaAgilidade(equipe.Value);
		var tempo = equipe.Key.dificuldade / agilidades;
		tempoTotal += tempo;
    }
	return tempoTotal;
}	

float somaAgilidade(List<Characther> personagens)
{
	var sum = 0.0f;
	foreach (var personagem in personagens)
    {
		sum += personagem.agility;
    }
	return sum;
}

#endregion