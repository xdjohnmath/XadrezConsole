using System;
using tabuleiro;

namespace xadrex{
    class PartidaDeXadrez {

        public Tabuleiro tab { get; private set; }
        private int turno;
        private Cor jogadorAtual;
        public bool terminada { get; private set; }

        public PartidaDeXadrez() {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            ColocarPecas();
            
        }

        public void ExecutarMovimento(Posicao origem, Posicao destino) {
            Peca p = tab.RetirarPeca(origem);
            p.IncrementarQuantidadeDeMovimentos();
            Peca pecaCapturaa = tab.RetirarPeca(destino);
            tab.ColocarPeca(p, destino);

        }

        private void ColocarPecas() {

            tab.ColocarPeca(new Torre(tab, Cor.Branca), new PosicaoXadrez('C',1).toPosicao());

            tab.ColocarPeca(new Rei(tab, Cor.Preta), new PosicaoXadrez('D', 1).toPosicao());


        }
    }
}
