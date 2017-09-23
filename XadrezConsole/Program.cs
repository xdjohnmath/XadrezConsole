using System;
using tabuleiro;
using xadrex;

namespace XadrezConsole {
    class Program {
        static void Main(string[] args) {

            try {


                PartidaDeXadrez partida = new PartidaDeXadrez();

                Tela.ImprimirTabuleiro(partida.tab);

               // Tela.ImprimirTabuleiro(tab);
            } catch (TabuleiroException e) {
                Console.WriteLine(e.Message);
            }

            Console.Read();
        }
    }
}
