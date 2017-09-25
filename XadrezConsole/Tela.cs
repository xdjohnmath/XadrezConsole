using System;
using System.Collections.Generic;
using tabuleiro;
using xadrex;

namespace XadrezConsole {
    class Tela {

        public static void ImprimirPartida(PartidaDeXadrez partida) {
            ImprimirTabuleiro(partida.tab);
            Console.WriteLine();
            ImprimirPecasCapturadas(partida);
            Console.WriteLine("Turno: " + partida.turno);
            if (!partida.terminada) {
                Console.WriteLine("Aguardando jogada: " + partida.jogadorAtual);
                if (partida.xeque) {
                    Console.WriteLine("XEQUE!");
                }
            }
            else {
                Console.WriteLine("XEQUEMATE!");
                Console.WriteLine("Vencedor: " + partida.jogadorAtual);
            }
        }

        public static void ImprimirPecasCapturadas(PartidaDeXadrez partida) {
            Console.WriteLine("Peças Capturadas: ");
            Console.Write("Brancas: ");
            ImprimirConjunto(partida.PecasCapturadas(Cor.Branca));
            Console.WriteLine();
            Console.Write("Pretas: ");
            ConsoleColor aux = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            ImprimirConjunto(partida.PecasCapturadas(Cor.Preta));
            Console.ForegroundColor = aux;
            Console.WriteLine();
            Console.WriteLine();
        }

        public static void ImprimirConjunto(HashSet<Peca> conjunto) {
            Console.Write("[");

            foreach (Peca x in conjunto) {
                Console.Write(x + " ");
            }
            Console.Write("]");
        }

        public static void ImprimirTabuleiro(Tabuleiro tab) {

            for (int i = 0 ; i < tab.linhas ; i++) {
                Console.Write(8 - i + " ");
                for (int j = 0 ; j < tab.colunas ; j++) {
                    ImprimirPeca(tab.peca(i, j));

                }
                Console.WriteLine();
            }
            Console.WriteLine("  A B C D E F G H");
        }

        public static void ImprimirTabuleiro(Tabuleiro tab, bool[,] posicoesPossiveis) {

            ConsoleColor fundoOriginal = Console.BackgroundColor;
            ConsoleColor fundoAlterado = ConsoleColor.DarkGray;

            for (int i = 0 ; i < tab.linhas ; i++) {
                Console.Write(8 - i + " ");
                for (int j = 0 ; j < tab.colunas ; j++) {
                    if (posicoesPossiveis[i, j]) {
                        Console.BackgroundColor = fundoAlterado;
                    }
                    else {
                        Console.BackgroundColor = fundoOriginal;
                    }
                    ImprimirPeca(tab.peca(i, j));
                    Console.BackgroundColor = fundoOriginal;
                }
                Console.WriteLine();
            }
            Console.WriteLine("  A B C D E F G H");
            Console.BackgroundColor = fundoOriginal;
        }

        public static PosicaoXadrez LerPosicaoXadrez() {
            string s = Console.ReadLine();
            char coluna = s[0];
            int linha = int.Parse(s[1] + "");
            return new PosicaoXadrez(coluna, linha);

        }

        public static void ImprimirPeca(Peca peca) {

            if (peca == null) {
                Console.Write("- ");
            }
            else {
                if (peca.cor == Cor.Branca) {
                    Console.Write(peca);
                }
                else {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write(peca);
                    Console.ForegroundColor = aux;
                }
                Console.Write(" ");
            }
        }

    }
}
